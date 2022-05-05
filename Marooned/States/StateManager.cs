using System.Collections.Generic;

namespace Marooned
{
    public class StateManager
    {   
        public StateManager(GameContext gameContext)
        {
            GameContext = gameContext;
        }

        public GameContext GameContext { get; }
        public Stack<State> States { get; private set; } = new Stack<State>();
        public State PreviousState { get; private set; }
        public State CurrentState
        {
            get
            {
                if (IsEmpty()) return null;
                return States.Peek();
            }
        }

        public void SwapState(State state)
        {
            if (!IsEmpty())
            {
                UnloadCurrentState();
                States.Pop();
            }
            States.Push(state);
            LoadState(state);
        }

        public void PushState(State state)
        {
            if (!IsEmpty())
            {
                UnloadCurrentState();
            }
            States.Push(state);
            LoadState(state);
        }

        public void ReturnToPreviousState()
        {
            if (!IsEmpty())
            {
                UnloadCurrentState();
                States.Pop();
                // Need to get previous state before previous
                // state to get the new previous state
                State currentState = States.Pop();
                if (IsEmpty())
                {
                    PreviousState = null;
                }
                else
                {
                    PreviousState = States.Peek();
                }
                PushState(currentState);
                LoadState(currentState);
            }
        }

        public bool IsEmpty()
        {
            return States.Count == 0;
        }

        public void Initialize()
        {
            CurrentState?.Initialize();
        }

        public void LoadContent()
        {
            CurrentState?.LoadContent();
        }

        public void UnloadContent()
        {
            CurrentState?.UnloadContent();
        }

        public void Update()
        {
            CurrentState?.Update();
        }

        public void Draw()
        {
            CurrentState?.Draw();
        }

        private void UnloadCurrentState()
        {
            if (CurrentState != null)
            {
                CurrentState.UnloadContent();
                CurrentState.Dispose();
                PreviousState = CurrentState;
            }
        }

        private void LoadState(State state)
        {
            state.GameContext = GameContext;
            state.Initialize();
            state.LoadContent();
        }
    }
}
