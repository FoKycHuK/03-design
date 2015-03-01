using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace battleships
{
	public class AiTester
	{
		Action<Game> visualizeIt;
		readonly Settings settings;
		int gameIndex = 0;
		string aiName;

		public AiTester(Settings settings, Action<Game> visualizeIt, string aiName)
		{
			this.settings = settings;
			this.visualizeIt = visualizeIt;
			this.aiName = aiName;
		}

		public Statistics RunGameToEnd(Game game)
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
			if (settings.Verbose)
			{
				Console.WriteLine(
					"Game #{3,4}: Turns {0,4}, BadShots {1}{2}",
					game.TurnsCount, game.BadShots, game.AiCrashed ? ", Crashed" : "", gameIndex);
			}
			gameIndex++;
			return new Statistics(aiName, new List<int>() {game.TurnsCount}, game.AiCrashed ? 1 : 0, game.BadShots, 1) ;
		}
	}
}