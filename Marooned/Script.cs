using System;
using System.Collections.Generic;
using Marooned.Actions;

namespace Marooned
{
    public class Script
    {
        public PriorityQueue<IAction, float> Actions { get; set; }

        public Script()
        {
            Actions = new PriorityQueue<IAction, float>();
        }

        public Script(IEnumerable<Tuple<IAction, float>> actions)
        {
            foreach (var actionTime in actions)
            {
                Enqueue(actionTime.Item1, actionTime.Item2);
            }
        }

        public IAction Peek()
        {
            return Actions.Peek();
        }

        public void Enqueue(IAction action, float time)
        {
            Actions.Enqueue(action, time);
        }

        public IAction Dequeue()
        {
            return Actions.Dequeue();
        }

        public void ExecuteAll(GameContext gameContext)
        {
            while (Actions.Count > 0)
            {
                Dequeue().Execute(gameContext);
            }
        }

        public void ExecuteFirst(GameContext gameContext)
        {
            if (Actions.Count > 0)
            {
                Peek().Execute(gameContext);
            }
        }

        public IAction Execute(GameContext gameContext)
        {
            IAction action = Dequeue();
            action.Execute(gameContext);
            return action;
        }
    }
}
