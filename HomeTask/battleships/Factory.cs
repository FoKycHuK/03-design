using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships
{
    public class Factory
    {
        public Game CreateGame(Map map, Ai ai)
        {
            return new Game(map, ai);
        }

        public Ai CreateAi(string exePath, ProcessMonitor monitor)
        {
            return new Ai(exePath, monitor);
        }
    }
}
