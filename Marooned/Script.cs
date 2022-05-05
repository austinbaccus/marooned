using System.Collections.Generic;
using Marooned.Actions;

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

        public void ExecuteAll(GameContext gameContext)
        {
            foreach (var action in Actions)
            {
                action.Execute(gameContext);
            }
            Actions.Clear();
        }

        public void ExecuteFirst(GameContext gameContext)
        {
            if (Actions.Count > 0)
            {
                Actions.Dequeue().Execute(gameContext);
            }
        }
    }
}
