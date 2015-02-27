using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace battleships
{
	public class AiTester
	{
		//private static readonly Logger resultsLog = LogManager.GetLogger("results");
		//public event Action<string> logMessage;
		public event Action<Game> visualizeIt;
		private readonly Settings settings;

		public AiTester(Settings settings)
		{
			this.settings = settings;
		}

		public Statistics TestSingleFile(Game[] games, Ai ai)
		{
			var vis = new GameVisualizer();
			var badShots = 0;
			var crashes = 0;
			var gamesPlayed = 0;
			var shots = new List<int>();
			for (var gameIndex = 0; gameIndex < games.Length; gameIndex++)
			{
				var game = games[gameIndex];
				RunGameToEnd(game, vis);
				gamesPlayed++;
				badShots += game.BadShots;
				if (game.AiCrashed)
				{
					crashes++;
					if (crashes > settings.CrashLimit) break;
					ai.Dispose();
					//ai.registerProcess += monitor.Register;
				}
				else
					shots.Add(game.TurnsCount);
				if (settings.Verbose)
				{
					Console.WriteLine(
						"Game #{3,4}: Turns {0,4}, BadShots {1}{2}",
						game.TurnsCount, game.BadShots, game.AiCrashed ? ", Crashed" : "", gameIndex);
				}
			}
			ai.Dispose();
			return new Statistics(ai.Name, shots, crashes, badShots, gamesPlayed, settings);
		}

		private void RunGameToEnd(Game game, GameVisualizer vis)
		{
			while (!game.IsOver())
			{
				game.MakeStep();
				if (settings.Interactive)
				{
					if (visualizeIt != null)
						visualizeIt(game);
					if (game.AiCrashed)
						Console.WriteLine(game.LastError.Message);
					Console.ReadKey();
				}
			}
		}
	}
}