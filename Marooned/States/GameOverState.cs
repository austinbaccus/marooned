using System;
using System.Collections.Generic;
using Marooned.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned.States
{
    public class GameOverState : State
    {
        public InteractiveState BackgroundState { get; set; }
        public List<ComponentOld> components;

        private Button _retryButton;
        private Button _returnMenuButton;

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            View = new GameOverView(this);
            var buttonTexture = content.Load<Texture2D>("Controls/Button");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");

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
        }

        private void RetryButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(InteractiveState.CreateDefaultState(game, graphicsDevice, content));
        }

        private void ReturnMenuButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(new MenuState(game, graphicsDevice, content));
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _retryButton.Position = Utils.GetCenterPos(_retryButton.Rectangle.Width, _retryButton.Rectangle.Height, graphicsDevice.Viewport) - new Vector2(0, _retryButton.Rectangle.Height / 2);
            _returnMenuButton.Position = Utils.GetCenterPos(_returnMenuButton.Rectangle.Width, _returnMenuButton.Rectangle.Height, graphicsDevice.Viewport) + new Vector2(0, _retryButton.Rectangle.Height / 2);
            foreach (var component in components)
                component.Update(gameTime);
        }

        public override List<ComponentOld> GetComponents()
        {
            return components;
        }
    }
}
