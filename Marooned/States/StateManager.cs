using MonoGame.Extended.Collections;

namespace Marooned
{
    public class StateManager
    {   
        public StateManager(GameContext gameContext)
        {
            GameContext = gameContext;
        }

        public GameContext GameContext { get; }
        public Deque<State> States { get; private set; } = new Deque<State>();
        public int Count { get => States.Count; }

        public State CurrentState
        {
            get
            {
                State currentState;
                if (States.GetFront(out currentState))
                {
                    return currentState;
                }
                return null;
            }
        }
        // Note: This represents the state BEFORE the current state inside States.
        // It is NOT necessarily the state that was running BEFORE the current state.
        public State PreviousState { get; private set; }

        public bool PopState()
        {
            if (Count > 0)
            {
                UnloadCurrentState();
                States.RemoveFromFront();
                return true;
            }
            return false;
        }

        public bool PopState(out State outState)
        {
            if (Count > 0)
            {
                UnloadCurrentState();
                return States.RemoveFromFront(out outState);
            }
            outState = null;
            return false;
        }

        public void SwapState(State state)
        {
            PopState();
            PushState(state);
        }

        public void PushState(State state)
        {
            States.AddToFront(state);
            LoadState(state);
        }

        public bool ReturnToPreviousState()
        {
            if (PopState())
            {
                // Need to get previous state before previous
                // state to get the new previous state
                State currentState;
                if (PopState(out currentState))
                {
                    if (Count <= 0)
                    {
                        PreviousState = null;
                    }
                    else
                    {
                        State previousState;
                        if (States.GetFront(out previousState))
                        {
                            PreviousState = previousState;
                        }
                    }
                    States.AddToFront(currentState);
                }
                else
                {
                    PreviousState = null;
                }
                return true;
            }
            return false;
        }

        public void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                PopState();
            }
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
            foreach (State state in States)
            {
                if (state.UpdateEnabled)
                {
                    state?.Update();
                }
            }
        }

        public void Draw()
        {
            foreach (State state in States)
            {
                if (state.DrawEnabled)
                {
                    state?.Draw();
                }
            }
        }

        private void UnloadCurrentState()
        {
            if (CurrentState != null)
            {
                CurrentState.UnloadContent();
                CurrentState.Dispose();
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
