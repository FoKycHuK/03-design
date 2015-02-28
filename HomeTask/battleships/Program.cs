using NLog;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Ninject;

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
			//Func<Map, Ai, Game> createGame = (map, ai) => new Game(map, ai);
			//Func<string, ProcessMonitor, Ai> createAi = (path, monitor) => new Ai(path, monitor);
			var container = new StandardKernel();
			container.Bind<Settings>().To<Settings>().WithConstructorArgument("settings.txt");
			var settings = container.Get<Settings>();
			var monitor = container.Get<ProcessMonitor>();
			container.Bind<Ai>().To<Ai>()
				.WithConstructorArgument(aiPath)
				.WithConstructorArgument(monitor);
			container.Bind<Random>().ToConstant(new Random(settings.RandomSeed));
			//container.Bind<Func<Map, Ai, Game>>().ToMethod(ctx => createGame);
			//container.Bind<Func<string, ProcessMonitor, Ai>>().ToMethod(ctx => createAi);
			//var settings = new Settings("settings.txt");
			//var logger = LogManager.GetLogger("results");
			//var tester = new AiTester(settings, logger);
			//var gameVis = new GameVisualizer();
			//var gen = new MapGenerator(settings, new Random(settings.RandomSeed));
			//var monitor = new ProcessMonitor(
			//	TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount), 
			//	settings.MemoryLimit);
			if (File.Exists(aiPath))
			{
				container.Get<AiTester>().TestSingleFile();
			}
			else
				Console.WriteLine("No AI exe-file " + aiPath);
		}
	}
}