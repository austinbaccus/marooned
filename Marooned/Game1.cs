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
        public GraphicsDeviceManager Graphics { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        public State CurrentState { get; private set; }
        private State _nextState;

        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;

        //Level_01 menuMap;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            BulletFactory.content = Content;
        }

        public void ChangeState(State state)
        {
            _nextState = state;
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

            Graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            Graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            Graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Load map
            Tiles.Content = Content;

            CurrentState = new MenuState(this, Graphics.GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
#if DEBUG
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#endif

            // State
            if (_nextState != null)
            {
                CurrentState = _nextState;
                _nextState = null;
            }
            CurrentState.Update(gameTime);
            CurrentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            CurrentState.Draw(gameTime, SpriteBatch);

            base.Draw(gameTime);
        }
    }
}
