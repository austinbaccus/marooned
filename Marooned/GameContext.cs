using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned
{
    public class GameContext
    {
        public GameContext(Game1 game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            Game = game;
            Graphics = graphics;
            SpriteBatch = spriteBatch;
            Content = contentManager;
            GameTime = null;

            StateManager = new StateManager(this);
        }

        public Game1 Game { get; }
        public GraphicsDeviceManager Graphics { get; }
        public GraphicsDevice GraphicsDevice { get => Graphics.GraphicsDevice; }
        public SpriteBatch SpriteBatch { get; }
        public ContentManager Content { get; }
        public StateManager StateManager { get; }
        public GameTime GameTime { get; internal set; }
    }
}
