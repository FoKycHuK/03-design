using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships
{
	public class Statistics
	{
		readonly string aiName;
		readonly List<int> shots;
		int crashes;
		int badShots;
		int gamesPlayed;
		public string Message {get; private set;}

		public Statistics(string aiName, List<int> shots, int crashes, int badShots, int gamesPlayed)
		{
			this.aiName = aiName;
			this.shots = shots;
			this.crashes = crashes;
			this.badShots = badShots;
			this.gamesPlayed = gamesPlayed;
		}

		public Statistics AppendStatistics(Statistics statistics)
		{
			if (aiName != statistics.aiName)
				throw new Exception("Статистики относятся к разным Ai");
			shots.AddRange(statistics.shots);
			crashes += statistics.crashes;
			badShots += statistics.badShots;
			gamesPlayed += statistics.gamesPlayed;
			return this;
		}

		public Statistics CalculateStatistics(Settings settings)
		{
			if (shots.Count == 0) shots.Add(1000 * 1000);
			shots.Sort();
			var median = shots.Count % 2 == 1 ? shots[shots.Count / 2] : (shots[shots.Count / 2] + shots[(shots.Count + 1) / 2]) / 2;
			var mean = shots.Average();
			var sigma = Math.Sqrt(shots.Average(s => (s - mean) * (s - mean)));
			var badFraction = (100.0 * badShots) / shots.Sum();
			var crashPenalty = 100.0 * crashes / settings.CrashLimit;
			var efficiencyScore = 100.0 * (settings.Width * settings.Height - mean) / (settings.Width * settings.Height);
			var score = efficiencyScore - crashPenalty - badFraction;
			Message = FormatTableRow(new object[] { aiName, mean, sigma, median, crashes, badFraction, gamesPlayed, score });
			return this;
		}

		public string GetMessageWithHeaders()
		{
			var headers = FormatTableRow(new object[] { "AiName", "Mean", "Sigma", "Median", "Crashes", "Bad%", "Games", "Score" });
			var res = new StringBuilder();
			res.Append("\nScore statistics\n=============\n");
			res.Append(headers);
			res.Append("\n");
			res.Append(Message);
			return res.ToString();
		}

		private string FormatTableRow(object[] values)
		{
			return FormatValue(values[0], 15) 
				+ string.Join(" ", values.Skip(1).Select(v => FormatValue(v, 7)));
		}

		private static string FormatValue(object v, int width)
		{
			return v.ToString().Replace("\t", " ").PadRight(width).Substring(0, width);
		}
	}
}
