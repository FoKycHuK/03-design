using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer.Commands
{
    public class HelpCommand : ICommand
    {
        Lazy<ICommand[]> commands;
        public HelpCommand(Lazy<ICommand[]> commands)
        {
            this.commands = commands;
        }
        public string Name
        {
            get { return "Help"; }
        }

        public void Execute()
        {
            foreach (var command in commands.Value)
                Console.WriteLine(command.Name);
        }
    }
}
