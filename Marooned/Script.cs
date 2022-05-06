using System;
using System.Collections.Generic;
using Marooned.Actions;

namespace Marooned
{
    public struct Script
    {
        public PriorityQueue<IAction, double> Actions { get; set; }

        public Script()
        {
            Actions = new PriorityQueue<IAction, double>();
        }

        public Script(IEnumerable<Tuple<IAction, double>> actions) : this()
        {
            Enqueue(actions);
        }

        public bool ShouldExecute(TimeSpan timeElapsed)
        {
            double time;
            if (!Actions.TryPeek(out _, out time)) return false;
            return timeElapsed.Seconds >= time;
        }

        public bool Empty()
        {
            return Actions.Count == 0;
        }

        public IAction Peek()
        {
            return Actions.Peek();
        }

        public void Enqueue(IAction action, double time)
        {
            Actions.Enqueue(action, time);
        }

        public void Enqueue(IEnumerable<Tuple<IAction, double>> actions)
        {
            foreach (var actionTime in actions)
            {
                Actions.Enqueue(actionTime.Item1, actionTime.Item2);
            }
        }

        public IAction Dequeue()
        {
            return Actions.Dequeue();
        }

        public bool TryDequeue(out IAction action, out double time)
        {
            return Actions.TryDequeue(out action, out time);
        }

        public void Clear()
        {
            Actions.Clear();
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
