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
            Interpreter = new Interpreter(this);
        }

        public Game1 Game { get; }
        public GraphicsDeviceManager Graphics { get; }
        public GraphicsDevice GraphicsDevice { get => Graphics.GraphicsDevice; }
        public SpriteBatch SpriteBatch { get; }
        public ContentManager Content { get; set; }
        public ContentManager GlobalContentManager { get; }
        public StateManager StateManager { get; }
        public GameTime GameTime { get; internal set; }
        public Interpreter Interpreter { get; }
    }
}
