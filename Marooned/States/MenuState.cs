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
        private List<DrawableGameComponent> _components = new List<DrawableGameComponent>();

        public MenuState(GameContext gameContext) : base(gameContext)
        {
            Systems = new SequentialSystem<GameContext>(
                // systems here
            );
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            GameContext.StateManager.SwapState(InteractiveState.CreateDefaultState(GameContext));
        }

        public override void LoadContent()
        {
            LoadMusic();

            var buttonTexture = GameContext.Content.Load<Texture2D>("Controls/Button");
            var buttonFont = GameContext.Content.Load<SpriteFont>("Fonts/Font");

            var newGameButton = new Button(GameContext, buttonTexture, buttonFont)
            {
                Text = "New Game",
            };
            newGameButton.Click += NewGameButton_Click;

            var loadGameButton = new Button(GameContext, buttonTexture, buttonFont)
            {
                Text = "Load Game",
            };
            loadGameButton.Click += LoadGameButton_Click;

            var quitGameButton = new Button(GameContext, buttonTexture, buttonFont)
            {
                Text = "Quit Game",
            };
            quitGameButton.Click += QuitGameButton_Click;

            _components.Add(newGameButton);
            _components.Add(loadGameButton);
            _components.Add(quitGameButton);

            foreach (var component in _components)
            {
                GameContext.Game.Components.Add(component);
            }

            base.LoadContent();
        }

        public override void Dispose()
        {
            GameContext.Game.Components.Clear();
            foreach (var component in _components)
            {
                component.Dispose();
            }

            base.Dispose();
        }

        public override void Update()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                Button button = (Button)_components[i];
                button.Position = Utils.GetCenterPos(button.Rectangle.Width, button.Rectangle.Height, GameContext.GraphicsDevice.Viewport)
                                - new Vector2(0, _components.Count * button.Rectangle.Height / 2)
                                + i * new Vector2(0, button.Rectangle.Height);
            }
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            GameContext.Game.Exit();
        }

        private void LoadMusic()
        {
            var _song = GameContext.Content.Load<Song>("Sounds/Music/ConcernedApe - Stardew Valley 1.5 Original Soundtrack - 01 Ginger Island");
            MediaPlayer.Play(_song);
        }

        public override void Draw()
        {
            GameContext.GraphicsDevice.Clear(Color.CornflowerBlue);
        }
    }
}
