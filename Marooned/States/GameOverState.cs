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
        public List<ComponentOld> components;

        private Button _retryButton;
        private Button _returnMenuButton;

        public GameOverState(GameContext gameContext) : base(gameContext)
        {
            var buttonTexture = gameContext.Content.Load<Texture2D>("Controls/Button");
            var buttonFont = gameContext.Content.Load<SpriteFont>("Fonts/Font");

            _retryButton = new Button(buttonTexture, buttonFont)
            {
                Text = "Retry",
            };
            _retryButton.Click += RetryButton_Click;

            _returnMenuButton = new Button(buttonTexture, buttonFont)
            {
                Text = "Return to Menu",
            };
            _returnMenuButton.Click += ReturnMenuButton_Click;

            components = new List<ComponentOld>()
            {
                _retryButton,
                _returnMenuButton,
            };

            Systems = new SequentialSystem<GameContext>(
                // systems here
            );
        }

        private void RetryButton_Click(object sender, EventArgs e)
        {
            GameContext.StateManager.ChangeState(InteractiveState.CreateDefaultState(GameContext));
        }

        private void ReturnMenuButton_Click(object sender, EventArgs e)
        {
            GameContext.StateManager.ChangeState(new MenuState(GameContext));
        }

        public override void Update()
        {
            _retryButton.Position = Utils.GetCenterPos(_retryButton.Rectangle.Width, _retryButton.Rectangle.Height, GameContext.GraphicsDevice.Viewport) - new Vector2(0, _retryButton.Rectangle.Height / 2);
            _returnMenuButton.Position = Utils.GetCenterPos(_returnMenuButton.Rectangle.Width, _returnMenuButton.Rectangle.Height, GameContext.GraphicsDevice.Viewport) + new Vector2(0, _retryButton.Rectangle.Height / 2);
            foreach (var component in components)
                component.Update(GameContext);
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
        }
    }
}
