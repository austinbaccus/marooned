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
            get {
                if (IsEmpty()) return null;
                return States.Peek();
            }
        }

        public void ChangeState(State state)
        {
            PreviousState = CurrentState;
            PushState(state);
            LoadState(state);
        }

        public void PopState()
        {
            States.Pop();
            // Need to get previous state before previous
            // state to get the new previous state
            State currentState = States.Pop();
            PreviousState = States.Peek();
            ChangeState(currentState);
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

        private void PushState(State state)
        {
            States.Push(state);
        }

        private void LoadState(State state)
        {
            CurrentState.UnloadContent();
            CurrentState.Dispose();
            PreviousState = CurrentState;

            state.GameContext = GameContext;
            state.Initialize();
            state.LoadContent();
        }
    }
}
