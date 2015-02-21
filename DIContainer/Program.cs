using System;
using System.Linq;
using DIContainer.Commands;
using Ninject;
using System.IO;

namespace DIContainer
{
    public class Program
    {
        private readonly CommandLineArgs arguments;
        private readonly ICommand[] commands;
        private readonly TextWriter textWriter;

        public Program(CommandLineArgs arguments, TextWriter textWriter, params ICommand[] commands)
        {
            this.arguments = arguments;
            this.commands = commands;
            this.textWriter = textWriter;
        }

        static void Main(string[] args)
        {
            //var arguments = new CommandLineArgs(args);
            //var printTime = new PrintTimeCommand();
            //var timer = new TimerCommand(arguments);
            //var commands = new ICommand[] { printTime, timer };
            //new Program(arguments, commands).Run();

            var container = new StandardKernel();
            container.Bind<ICommand>().To<PrintTimeCommand>();
            container.Bind<ICommand>().To<TimerCommand>();
            container.Bind<ICommand>().To<HelpCommand>();
            container.Bind<CommandLineArgs>().To<CommandLineArgs>().WithConstructorArgument(args);
            container.Bind<TextWriter>().ToConstant(Console.Out);
            container.Get<Program>().Run();
        }

        public void Run()
        {
            if (arguments.Command == null)
            {
                textWriter.WriteLine("Please specify <command> as the first command line argument");
                return;
            }
            var command = commands.FirstOrDefault(c => c.Name.Equals(arguments.Command, StringComparison.InvariantCultureIgnoreCase));
            if (command == null)
                textWriter.WriteLine("Sorry. Unknown command {0}" + arguments.Command);
            else
                command.Execute();
        }
    }
}
