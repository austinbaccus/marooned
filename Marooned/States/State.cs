using DefaultEcs;
using DefaultEcs.System;
using Marooned.Controllers;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;

namespace Marooned
{
    public abstract class State : ILifeCycle
    {
        public State(GameContext gameContext, ISystem<GameContext> systems = null, int? maxCapacity = null)
        {
            GameContext = gameContext;

            GameContext.Content = new ContentManager(GameContext.Game.Services, Game1.CONTENT_DIRECTORY);

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
        public OrthographicCamera Camera { get; set; }
        public InputController InputController { get; set; }
        public ISystem<GameContext> Systems { get; protected set; }
        public World World { get; protected set; }
        public bool UpdateEnabled { get; set; } = true;
        public bool DrawEnabled { get; set; } = true;

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent()
        {
            GameContext.Content.Dispose();
            GameContext.Content = GameContext.GlobalContentManager;
        }
        public virtual void Dispose() { }

        public abstract void Update();
        public abstract void Draw();
    }
}
