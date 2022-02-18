using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Marooned.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using Marooned.Sprites.Enemies;

namespace Marooned.States
{
    public class InteractiveState : State
    {
        //private Map _map;
        private Player _player;
        private List<Grunt> _grunts = new List<Grunt>();
        protected Camera _camera;
        private List<Component> _components;

        // Tiled
        private OrthographicCamera _cameraOrtho;
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;

        public InteractiveState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, string mapPath, List<string> songPaths, string playerSpritePath) : base(game, graphicsDevice, content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
            _components = new List<Component>();
            LoadContent();
            LoadSprites(playerSpritePath);
            LoadEnemies();
            LoadMusic(songPaths);
            LoadMap(mapPath);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, transformMatrix: _camera.Transform, samplerState: SamplerState.PointClamp);
            _tiledMapRenderer.Draw(_cameraOrtho.GetViewMatrix());
            spriteBatch.End();

            // A second spriteBatch.Begin()/End() section is needed to render the player after the map has been rendered.
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, transformMatrix: _camera.Transform, samplerState: SamplerState.PointClamp);
            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            foreach (var bullet in _player.BulletList)
            {
                bullet.Draw(gameTime, spriteBatch);
            }
            foreach(var grunt in _grunts)
            {
                grunt.Draw(gameTime, spriteBatch);
                foreach (var bullet in grunt.BulletList)
                {
                    bullet.Draw(gameTime, spriteBatch);
                }
            }
            
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
            for (int i = 0; i < _player.BulletList.Count; i++)
            {
                if (_player.BulletList[i].IsRemoved)
                {
                    _player.BulletList.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < _grunts.Count; i++)
            {
                if (_grunts[i].IsRemoved)
                {
                    _grunts.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
            _camera.Follow(_player);
            Vector2 adj = new Vector2(_player.Position.X+100, _player.Position.Y+100);
            _cameraOrtho.LookAt(adj);

            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
            foreach (var bullet in _player.BulletList)
            {
                bullet.Update(gameTime);
            }
            foreach (var grunt in _grunts)
            {
                grunt.Update(gameTime);
                foreach (var bullet in grunt.BulletList)
                {
                    bullet.Update(gameTime);
                    
                    // did it hit the player?
                    // get distance between bullet and player
                    var distance = Math.Sqrt(Math.Abs((_player.Position.X - bullet.Position.X) + (_player.Position.Y - bullet.Position.Y)));
                    if (distance <= _player.HitboxRadius)
                    {
                        int x = 2;
                    }
                }
            }
        }

        private void LoadContent()
        {
            _camera = new Camera();
            var viewportadapter = new BoxingViewportAdapter(_game.Window, _graphicsDevice, 1800, 1000);
            _cameraOrtho = new OrthographicCamera(viewportadapter);
            _cameraOrtho.Zoom = 3.0f;
        }

        private void LoadSprites(string playerSpritePath)
        {
            var texture = _content.Load<Texture2D>(playerSpritePath);

            _player = new Player(texture)
            {
                Position = new Vector2(100, 100),
                Speed = 2.2f,
            };

            _components.Add(_player);
        }

        private void LoadEnemies()
        {
            var texture = _content.Load<Texture2D>("Sprites/Skeleton");

            var enemy = new Sprites.Enemies.Grunt(texture, Sprites.Enemies.FiringPattern.Pattern.straight, Sprites.Enemies.MovementPattern.Pattern.down_left)
            {
                Position = new Vector2(100, 100),
            };

            _grunts.Add(enemy);
            //_components.Add(enemy);
        }

        private void LoadMusic(List<string> songPaths)
        {
            int songIdx = 0;

            // load first song
            Uri uri = new Uri(songPaths[songIdx], UriKind.Relative);
            Song song = Song.FromUri("track", uri);
            MediaPlayer.Play(song);

            MediaPlayer.ActiveSongChanged += (s, e) => {
                // dispose of current song
                song.Dispose();
                songIdx = (songIdx + 1) % songPaths.Count;
                System.Diagnostics.Debug.WriteLine("Song ended and disposed");

                // play next song
                uri = new Uri(songPaths[songIdx], UriKind.Relative);
                song = Song.FromUri("track", uri);
                MediaPlayer.Play(song);
            };
        }

        private void LoadMap(string mapPath)
        {
            _tiledMap = _content.Load<TiledMap>(mapPath);
            _tiledMapRenderer = new TiledMapRenderer(_graphicsDevice, _tiledMap);
        }
    }
}
