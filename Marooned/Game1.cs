using Marooned.Factories;
using Marooned.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Marooned
{
    public class Game1 : Game
    {
        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameContext _gameContext;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            BulletFactory.content = Content;
            EnemyFactory.content = Content;
        }

        protected override void Initialize()
        {
            // Allows for FPS higher than 60
            TargetElapsedTime = TimeSpan.FromSeconds(1f / 144f);
            // Makes the game run the same regardless of FPS (assuming we take into account <c>gameTime</c> for update calculations)
            IsFixedTimeStep = true;
            Window.IsBorderless = false;
            Window.AllowUserResizing = true;

            base.Initialize();

            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.ApplyChanges();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _gameContext = new GameContext(this, _graphics, _spriteBatch, Content);
            _gameContext.StateManager.ChangeState(new MenuState(_gameContext));
        }

        protected override void LoadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
#if DEBUG
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#endif
            _gameContext.GameTime = gameTime;

            _gameContext.StateManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _gameContext.GameTime = gameTime;

            _gameContext.StateManager.Draw();

            base.Draw(gameTime);
        }
    }
}
