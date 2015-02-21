using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluentTask
{
    public class Behavior
    {
        List<Action> actions = new List<Action>();

        public Behavior Say(string text)
        {
            actions.Add(() => 
            { 
                foreach (var symb in text) 
                    Console.Write(symb);
                Console.WriteLine();
            });
            return this;
        }

        public Behavior UntilKeyPressed(Func<Behavior, Behavior> action)
        {
            actions.Add(() =>
                {
                    while (!Console.KeyAvailable)
                    {
                        action(new Behavior()).Execute();
                    }
                    Console.ReadKey();
                });
            return this;
        }

        public Behavior Jump(JumpHeight height)
        {
            actions.Add(() => new Behavior().Say("Прыгнул " + (JumpHeight.High == height ? "Высоко" : "Низко")).Execute());
            return this;
        }

        public Behavior Delay(TimeSpan time)
        {
            actions.Add(() => Thread.Sleep(time));
            return this;
        }

        public void Execute()
        {
            actions.ForEach(a => a.Invoke());
        }
    }
}
