using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Marooned.Controls;
using Microsoft.Xna.Framework.Media;

namespace Marooned.States
{
    public class MenuState : State
    {
        private List<Component> components;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            View = new MenuView(this);
            var buttonTexture = content.Load<Texture2D>("Controls/Button");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Text = "New Game",
            };
            newGameButton.Click += NewGameButton_Click;

            var loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Text = "Load Game",
            };
            loadGameButton.Click += LoadGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Text = "Quit Game",
            };
            quitGameButton.Click += QuitGameButton_Click;

            components = new List<Component>()
            {
                newGameButton,
                loadGameButton,
                quitGameButton,
            };

            LoadMusic();
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Load Game");
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(InteractiveState.CreateDefaultState(game, graphicsDevice, content));
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < components.Count; i++)
            {
                Button button = (Button)components[i];
                button.Position = Utils.GetCenterPos(button.Rectangle.Width, button.Rectangle.Height, graphicsDevice.Viewport)
                                - new Vector2(0, components.Count * button.Rectangle.Height / 2)
                                + i * new Vector2(0, button.Rectangle.Height);
            }
            foreach (var component in components)
                component.Update(gameTime);
        }

        public override List<Component> GetComponents()
        {
            return components;
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            game.Exit();
        }

        private void LoadMusic()
        {
            Uri uri = new Uri("Content/Sounds/Music/ConcernedApe - Stardew Valley 1.5 Original Soundtrack - 01 Ginger Island.mp3", UriKind.Relative);
            Song song = Song.FromUri("mySong", uri);
            MediaPlayer.Play(song);
            MediaPlayer.ActiveSongChanged += (s, e) => {
                song.Dispose();
                System.Diagnostics.Debug.WriteLine("Song ended and disposed");
            };
        }
    }
}
