using System.Collections.Generic;
using Marooned.Actions;

namespace Marooned
{
    public class ScriptOld
    {
        public Queue<IAction> Actions { get; set; }

        public ScriptOld()
        {
            Actions = new Queue<IAction>();
        }

        public ScriptOld(IEnumerable<IAction> actions)
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
