using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Marooned.Controls;
using Microsoft.Xna.Framework.Media;
using DefaultEcs.System;

namespace Marooned.States
{
    public class MenuState : State
    {
        private List<ComponentOld> components;

        public MenuState(GameContext gameContext) : base(gameContext)
        {
            var buttonTexture = gameContext.Content.Load<Texture2D>("Controls/Button");
            var buttonFont = gameContext.Content.Load<SpriteFont>("Fonts/Font");

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

            components = new List<ComponentOld>()
            {
                newGameButton,
                loadGameButton,
                quitGameButton,
            };

            LoadMusic();

            Systems = new SequentialSystem<GameContext>(
                // systems here
            );
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Load Game");
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            GameContext.StateManager.ChangeState(InteractiveState.CreateDefaultState(GameContext));
        }

        public override void Update()
        {
            for (int i = 0; i < components.Count; i++)
            {
                Button button = (Button)components[i];
                button.Position = Utils.GetCenterPos(button.Rectangle.Width, button.Rectangle.Height, GameContext.GraphicsDevice.Viewport)
                                - new Vector2(0, components.Count * button.Rectangle.Height / 2)
                                + i * new Vector2(0, button.Rectangle.Height);
            }
            foreach (var component in components)
                component.Update(GameContext);
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            GameContext.Game.Exit();
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

        public override void Draw()
        {
            GameContext.GraphicsDevice.Clear(Color.CornflowerBlue);

            GameContext.SpriteBatch.Begin();

            foreach (var component in components)
            {
                component.Draw(GameContext);
            }

            GameContext.SpriteBatch.End();
        }
    }
}
