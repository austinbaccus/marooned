using System.Collections.Generic;
using Marooned.Actions;
using Microsoft.Xna.Framework;

namespace Marooned
{
    public class Script
    {
        public Queue<IAction> Actions { get; set; }

        public Script()
        {
            Actions = new Queue<IAction>();
        }

        public Script(IEnumerable<IAction> actions)
        {
            foreach (var action in actions)
            {
                Actions.Enqueue(action);
            }
        }

        public void AddAction(IAction action)
        {
            Actions.Enqueue(action);
        }

        public void ExecuteAll(GameTime gameTime)
        {
            foreach (var action in Actions)
            {
                action.Execute(gameTime);
            }
            Actions.Clear();
        }

        public void ExecuteFirst(GameTime gameTime)
        {
            if (Actions.Count > 0)
            {
                Actions.Dequeue().Execute(gameTime);
            }
        }
    }
}
