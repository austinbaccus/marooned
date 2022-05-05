using System;
using System.Collections.Generic;
using DefaultEcs.System;
using Marooned.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Marooned.States
{
    public class GameOverState : State
    {
        private Button _retryButton;
        private Button _returnMenuButton;

        public GameOverState(GameContext gameContext) : base(gameContext)
        {
            Systems = new SequentialSystem<GameContext>(
                // systems here
            );
        }

        private void RetryButton_Click(object sender, EventArgs e)
        {
            GameContext.StateManager.SwapState(InteractiveState.CreateDefaultState(GameContext));
        }

        private void ReturnMenuButton_Click(object sender, EventArgs e)
        {
            GameContext.StateManager.SwapState(new MenuState(GameContext));
        }

        public override void LoadContent()
        {
            var buttonTexture = GameContext.Content.Load<Texture2D>("Controls/Button");
            var buttonFont = GameContext.Content.Load<SpriteFont>("Fonts/Font");

            _retryButton = new Button(GameContext, buttonTexture, buttonFont)
            {
                Text = "Retry",
            };
            _retryButton.Click += RetryButton_Click;

            _returnMenuButton = new Button(GameContext, buttonTexture, buttonFont)
            {
                Text = "Return to Menu",
            };
            _returnMenuButton.Click += ReturnMenuButton_Click;

            GameContext.Game.Components.Add(_retryButton);
            GameContext.Game.Components.Add(_returnMenuButton);

            base.LoadContent();
        }

        public override void Update()
        {
            _retryButton.Position = Utils.GetCenterPos(_retryButton.Rectangle.Width, _retryButton.Rectangle.Height, GameContext.GraphicsDevice.Viewport) - new Vector2(0, _retryButton.Rectangle.Height / 2);
            _returnMenuButton.Position = Utils.GetCenterPos(_returnMenuButton.Rectangle.Width, _returnMenuButton.Rectangle.Height, GameContext.GraphicsDevice.Viewport) + new Vector2(0, _retryButton.Rectangle.Height / 2);
        }

        public override void Draw()
        {
            GameContext.GraphicsDevice.Clear(Color.CornflowerBlue);

            GameContext.SpriteBatch.Begin();

            GameContext.SpriteBatch.FillRectangle(
                new RectangleF(0f, 0f, GameContext.GraphicsDevice.Viewport.Width, GameContext.GraphicsDevice.Viewport.Height),
                Color.Black * 0.5f
            );

            GameContext.SpriteBatch.End();

            GameContext.StateManager.PreviousState.Draw();
        }

        public override void Dispose()
        {
            GameContext.Game.Components.Clear();

            _retryButton.Dispose();
            _returnMenuButton.Dispose();

            base.Dispose();
        }
    }
}
