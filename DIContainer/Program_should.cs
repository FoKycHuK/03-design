using FakeItEasy;
using NUnit.Framework;
using System;

namespace DIContainer
{
    [TestFixture]
    public class Program_should
    {
        [Test]
        public void run_command_by_name()
        {
            var command = A.Fake<ICommand>();
            A.CallTo(() => command.Name).Returns("cmd");
            var program = new Program(new CommandLineArgs("CMD"), Console.Out, command);

            program.Run();

            A.CallTo(() => command.Execute()).MustHaveHappened();
        }

        [Test]
        public void dont_run_command_wiht_other_name()
        {
            var command = A.Fake<ICommand>();
            A.CallTo(() => command.Name).Returns("cmd");
            var program = new Program(new CommandLineArgs("anotherCmd"), Console.Out, command);

            program.Run();

            A.CallTo(() => command.Execute()).MustNotHaveHappened();
        }
    }
}