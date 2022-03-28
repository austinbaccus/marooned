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
        public List<Component> components;

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            View = new GameOverView(this);
            var buttonTexture = content.Load<Texture2D>("Controls/Button");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");

            var retryButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(350, 200),
                Text = "Retry",
            };

            retryButton.Click += RetryButton_Click;

            var returnMenuButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(350, 250),
                Text = "Return to Menu",
            };

            returnMenuButton.Click += ReturnMenuButton_Click;

            components = new List<Component>()
            {
                retryButton,
                returnMenuButton,
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
            foreach (var component in components)
                component.Update(gameTime);
        }

        public override List<Component> GetComponents()
        {
            return components;
        }
    }
}
