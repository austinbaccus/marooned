using DefaultEcs;
using DefaultEcs.System;
using System;

namespace Marooned
{
    public abstract class State : IDisposable
    {
        public State(GameContext gameContext, ISystem<GameContext> systems = null, int? maxCapacity = null)
        {
            GameContext = gameContext;
            Systems = systems;

            if (maxCapacity.HasValue)
            {
                World = new World(maxCapacity.Value);
            }
            else
            {
                World = new World();
            }
        }

        public GameContext GameContext { get; internal set; }
        public ISystem<GameContext> Systems { get; protected set; }
        public World World { get; protected set; }

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void Dispose() { }

        public abstract void Update();
        public abstract void Draw();
    }
}
