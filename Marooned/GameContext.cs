using Marooned.Interpreter;
using Marooned.Interpreter.Json.Animations;
using Marooned.Interpreter.Json.Entities;
using Marooned.Interpreter.Json.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned
{
    public class GameContext
    {
        public GameContext(Game1 game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, ContentManager globalContentManager)
        {
            Game = game;
            Graphics = graphics;
            SpriteBatch = spriteBatch;
            Content = globalContentManager;
            GlobalContentManager = globalContentManager;
            GameTime = null;

            StateManager = new StateManager(this);
            AnimationsInterpreter = new JsonAnimationInterpreter(this);
            EntitiesInterpreter = new JsonEntitiesInterpreter(this);
            ScriptsInterpreter = new JsonScriptsInterpreter(this);
        }

        public Game1 Game { get; }
        public GraphicsDeviceManager Graphics { get; }
        public GraphicsDevice GraphicsDevice { get => Graphics.GraphicsDevice; }
        public SpriteBatch SpriteBatch { get; }
        public ContentManager Content { get; set; }
        public ContentManager GlobalContentManager { get; }
        public StateManager StateManager { get; }
        public GameTime GameTime { get; internal set; }
        public IAnimationsInterpreter AnimationsInterpreter { get; }
        public IEntitiesInterpreter EntitiesInterpreter { get; }
        public IScriptsInterpreter ScriptsInterpreter { get; }
    }
}
