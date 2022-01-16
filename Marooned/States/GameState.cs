using Marooned.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Marooned.States
{
    public class GameState : State
    {
        private List<Component> _components;
        private Camera _camera;
        private Player _player;
        private Map _map;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
            _components = new List<Component>();
            LoadContent();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(transformMatrix: _camera.Transform, samplerState: SamplerState.PointClamp);

            _map.Draw(spriteBatch);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            _camera.Follow(_player);
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }

        private void LoadContent()
        {
            _camera = new Camera();
            LoadMap();
            LoadMusic();
            LoadSprites();
        }

        private void LoadSprites()
        {
            var texture = _content.Load<Texture2D>("Sprites/IslandParrot");

            _player = new Player(texture)
            {
                Position = new Vector2(0, 0),
                Speed = 3f,
            };

            _components.Add(_player);
        }
        private void LoadMusic()
        {
            Uri uri = new Uri("Content/Sounds/Music/ConcernedApe - Stardew Valley 1.5 Original Soundtrack - 03 Volcano Mines (Molten Jelly).mp3", UriKind.Relative);
            Song song = Song.FromUri("track_01", uri);
            MediaPlayer.Play(song);
            MediaPlayer.ActiveSongChanged += (s, e) => {
                song.Dispose();
                System.Diagnostics.Debug.WriteLine("Song ended and disposed");
            };
        }
        private void LoadMap()
        {
            _map = new Maps.Level_02();
            _map.Generate();
        }
    }
}
