using NLog;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Linq;

namespace battleships
{
	public class Program
	{
		private static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			if (args.Length == 0)
			{
				Console.WriteLine("Usage: {0} <ai.exe>", Process.GetCurrentProcess().ProcessName);
				return;
			}
			var aiPath = args[0];
			if (!File.Exists(aiPath))
			{
				Console.WriteLine("No AI exe-file " + aiPath);
				return;
			}
			var settings = new Settings("settings.txt");
			var vis = new GameVisualizer();
			var monitor = new ProcessMonitor(TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount), settings.MemoryLimit);
			var gen = new MapGenerator(settings, new Random(settings.RandomSeed));
			var ai = new Ai(aiPath);
			ai.registerProcess += monitor.Register;
			var logger = LogManager.GetLogger("results");
			var games = Enumerable.Range(0, settings.GamesCount)
				.Select(x => new Game(gen.GenerateMap(), ai))
				.ToArray();
			var tester = new AiTester(settings);
			tester.visualizeIt += vis.Visualize;
			var statistic = tester.TestSingleFile(games, ai);
			Console.WriteLine(statistic.GetMessageWithHeaders());
			logger.Info(statistic.Message);
		}
	}
}