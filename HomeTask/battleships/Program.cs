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
        public static StandardKernel Container;
		private static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			if (args.Length == 0)
			{
				Console.WriteLine("Usage: {0} <ai.exe>", Process.GetCurrentProcess().ProcessName);
				return;
			}
            Container = new StandardKernel();
            Container.Bind<Settings>().To<Settings>().WithConstructorArgument("settings.txt");
            var settings = Container.Get<Settings>();
            Container.Bind<Logger>().ToConstant(LogManager.GetLogger("results"));
            Container.Bind<Random>().ToConstant(new Random(settings.RandomSeed));
            Container.Bind<ProcessMonitor>().To<ProcessMonitor>()
                .WithConstructorArgument( 
                    TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount))
                .WithConstructorArgument((long)settings.MemoryLimit);
			var aiPath = args[0];
            //var settings = new Settings("settings.txt");
            //var logger = LogManager.GetLogger("results");
            //var tester = new AiTester(settings, logger);
            //var gameVis = new GameVisualizer();
            //var gen = new MapGenerator(settings, new Random(settings.RandomSeed));
            //var monitor = new ProcessMonitor(
            //    TimeSpan.FromSeconds(settings.TimeLimitSeconds * settings.GamesCount), 
            //    settings.MemoryLimit);
            if (File.Exists(aiPath))
            {
                Container.Get<AiTester>().TestSingleFile(
                aiPath,
                Container.Get<MapGenerator>(),
                Container.Get<GameVisualizer>(),
                Container.Get<ProcessMonitor>());
            }
            else
                Console.WriteLine("No AI exe-file " + aiPath);
		}
	}
}