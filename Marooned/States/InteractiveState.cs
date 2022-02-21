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
using MonoGame.Extended.Screens;

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
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;

        public InteractiveState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, string mapPath, List<string> songPaths, string playerSpritePath, string playerHitboxSpritePath) : base(game, graphicsDevice, content)
        {
            _components = new List<Component>();
            LoadContent();
            LoadSprites(playerSpritePath, playerHitboxSpritePath);
            LoadEnemies();
            LoadMusic(songPaths);
            LoadMap(mapPath);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, transformMatrix: _camera.Transform, samplerState: SamplerState.PointClamp);
            _tiledMapRenderer.Draw(_camera.CameraOrtho.GetViewMatrix());
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

        // Draw Rectangle Hitbox
        private void DrawRectangle(SpriteBatch s, float x, float y, float width, float height, Color? color=null)
        {
            s.DrawRectangle(new RectangleF(x, y, width, height), color ?? Color.White);
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
            // 4 is a magic number to get the player nearly center to the screen
            _camera.Follow(_player, _player.Hitbox.Offset * 4, _graphicsDevice);
            Vector2 adj = new Vector2(_player.Position.X, _player.Position.Y);
            _camera.CameraOrtho.LookAt(adj);

            foreach (var component in _components)
            {
                component.Update(gameTime);
            }

            for (int i = 0; i < _player.BulletList.Count; i++)
            {
                Bullet bullet = _player.BulletList[i];
                bullet.Update(gameTime);

                foreach(var grunt in _grunts)
                {
                    if (grunt.Hitbox.IsTouching(bullet.Hitbox))
                    {
                        _player.BulletList.RemoveAt(i);
                        i--;
                    }
                }
            }

            foreach (var grunt in _grunts)
            {
                grunt.Update(gameTime);

                for (int i = 0; i < grunt.BulletList.Count; i++)
                {
                    Bullet bullet = grunt.BulletList[i];
                    bullet.Update(gameTime);
                    
                    // did it hit the player?
                    if (_player.Hitbox.IsTouching(bullet.Hitbox))
                    {
                        grunt.BulletList.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        private void LoadContent()
        {
            var viewportadapter = new BoxingViewportAdapter(_game.Window, _graphicsDevice, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
            _camera = new Camera(new OrthographicCamera(viewportadapter));
        }

        private void LoadSprites(string playerSpritePath, string playerHitboxSpritePath)
        {
            var texture = _content.Load<Texture2D>(playerSpritePath);
            var hitboxTexture = _content.Load<Texture2D>(playerHitboxSpritePath);

            _player = new Player(texture, hitboxTexture)
            {
                Position = new Vector2(100, 100),
                Speed = 120f,
                FocusSpeedFactor = 0.5f,
            };
            _player.Hitbox = new Hitbox(_player)
            {
                Radius = 5,
                Offset = new Vector2(0, 5f),
            };

            _components.Add(_player);
        }

        private void LoadEnemies()
        {
            var texture = _content.Load<Texture2D>("Sprites/Skeleton");
            // TODO: Better, non-hardcoded way of doing this
            Rectangle[] animSources =
            {
                new Rectangle(0, 0, 16, 32)
            };

            var enemy = new Grunt(texture, animSources, FiringPattern.Pattern.straight, MovementPattern.Pattern.down_left)
            {
                Position = new Vector2(100, 100),
#if DEBUG
                HitboxSprite = new Sprite(_content.Load<Texture2D>("Sprites/PlayerHitbox")),
#endif
            };
            enemy.Hitbox = new Hitbox(enemy)
            {
                Radius = 5,
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
