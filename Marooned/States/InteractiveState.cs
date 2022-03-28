﻿using System;
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
using Marooned.Factories;

namespace Marooned.States
{
    public class InteractiveState : State
    {
        public Player player;
        public List<Grunt> grunts = new List<Grunt>();
        public Stack<List<Grunt>> waves = new Stack<List<Grunt>>();
        public List<Sprite> hearts = new List<Sprite>();
        public OrthographicCamera camera;

        private List<Component> components;

        // Tiled
        public TiledMap tiledMap;
        public TiledMapRenderer tiledMapRenderer;

        private Vector2 _spawnPoint = new Vector2(300, 300);

        public InteractiveState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, string mapPath, List<string> songPaths, string playerSpritePath, string playerHitboxSpritePath) : base(game, graphicsDevice, content)
        {
            components = new List<Component>();
            LoadContent();
            LoadSprites(playerSpritePath, playerHitboxSpritePath);
            LoadEnemies();
            LoadMusic(songPaths);
            LoadMap(mapPath);
            LoadLives();
        }

        public float LevelTime { get; private set; }
        public bool MiniBossActive { get; private set; }
        public bool BossActive { get; private set; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);
            tiledMapRenderer.Draw(camera.GetViewMatrix());
            spriteBatch.End();

            // A second spriteBatch.Begin()/End() section is needed to render the player after the map has been rendered.
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);

            // draw misc. components
            foreach (var component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            // draw bullets
            foreach (var bullet in player.BulletList)
            {
                bullet.Draw(gameTime, spriteBatch);
            }

            // draw grunts
            foreach(var grunt in grunts)
            {
                grunt.Draw(gameTime, spriteBatch);

                foreach (var bullet in grunt.BulletList)
                {
                    bullet.Draw(gameTime, spriteBatch);
                }
            }

            spriteBatch.End();

            // draw HUD
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);
            foreach (Sprite heart in hearts)
            {
                heart.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
            for (int i = 0; i < player.BulletList.Count; i++)
            {
                if (player.BulletList[i].IsRemoved)
                {
                    player.BulletList.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < grunts.Count; i++)
            {
                if (grunts[i].IsRemoved)
                {
                    grunts.RemoveAt(i);
                    i--;
                }
            }
            if (grunts.Count <= 0)
            {
                LoadNextWave();
            }
        }

        public override void Update(GameTime gameTime)
        {
            tiledMapRenderer.Update(gameTime);
            // 4 is a magic number to get the player nearly center to the screen
            camera.LookAt(player.Position + player.Hitbox.Offset * 4);

            foreach (var component in components)
            {
                component.Update(gameTime);
            }

            // check player for damage
            foreach (var grunt in grunts)
            {
                grunt.Update(gameTime);

                for (int i = 0; i < grunt.BulletList.Count; i++)
                {
                    Bullet bullet = grunt.BulletList[i];
                    bullet.Update(gameTime);

                    if (!player.IsInvulnerable)
                    {
                        // did it hit the player?
                        if (player.Hitbox.IsTouching(bullet.Hitbox))
                        {
                            player.isHit = true; // Show red damage on grunt

                            player.Lives--;
                            if (player.Lives <= 0)
                            {
                                // "game over man! game over!"
                                GoToMenu();
                            }

                            grunt.BulletList.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }

            // check enemies for damage
            for (int i = 0; i < player.BulletList.Count; i++)
            {
                Bullet bullet = player.BulletList[i];
                bullet.Update(gameTime);

                for (int j = 0; j < grunts.Count; j++)
                {

                    if (grunts[j].Hitbox.IsTouching(bullet.Hitbox))
                    {
                        grunts[j].isHit = true; // Show red damage on grunt
                        grunts[j].Health--;
                        if (grunts[j].Health <= 0)
                        {
                            grunts.RemoveAt(j);
                        }

                        player.BulletList.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }

            // update lives
            UpdateLives();
        }

        private void LoadContent()
        {
            var viewportadapter = new BoxingViewportAdapter(game.Window, graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            camera = new OrthographicCamera(viewportadapter);
            camera.Zoom = 2f;
        }

        private void LoadSprites(string playerSpritePath, string playerHitboxSpritePath)
        {
            var texture = content.Load<Texture2D>(playerSpritePath);
            var hitboxTexture = content.Load<Texture2D>(playerHitboxSpritePath);

            player = new Player(texture, hitboxTexture)
            {
                Position = _spawnPoint,
                Speed = 120f,
                FocusSpeedFactor = 0.5f,
            };
            player.Hitbox = new Hitbox(player)
            {
                Radius = 5,
                Offset = new Vector2(0, 5f),
            };

            components.Add(player);
        }

        private void LoadEnemies()
        {
            List<Grunt> wave1 = new List<Grunt>();
            wave1.Add(EnemyFactory.MakeGrunt("skeleton", new Vector2(225, 100), 5));
            wave1.Add(EnemyFactory.MakeGrunt("skeleton", new Vector2(250, 100), 5));
            wave1.Add(EnemyFactory.MakeGrunt("skeleton", new Vector2(275, 100), 5));

            List<Grunt> wave2 = new List<Grunt>();
            wave2.Add(EnemyFactory.MakeGrunt("skeleton", new Vector2(225, 100), 5));
            wave2.Add(EnemyFactory.MakeGrunt("skeleton", new Vector2(275, 100), 5));
            wave2.Add(EnemyFactory.MakeGrunt("skeleton_dangerous", new Vector2(250, 100), 5));

            List<Grunt> wave3 = new List<Grunt>();
            wave3.Add(EnemyFactory.MakeGrunt("skeleton_mage", new Vector2(250, 100), 5));

            // Boss goes here, replace skeleton_mage with boss assets/props
            List<Grunt> wave4 = new List<Grunt>();
            wave4.Add(EnemyFactory.MakeGrunt("miniboss1", new Vector2(250, 300), 10));

            List<Grunt> wave5 = new List<Grunt>();
            wave5.Add(EnemyFactory.MakeGrunt("boss1", new Vector2(250, 300), 30));

            waves.Push(wave5);
            waves.Push(wave4);
            waves.Push(wave3);
            waves.Push(wave2);
            waves.Push(wave1);
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
            tiledMap = content.Load<TiledMap>(mapPath);
            tiledMapRenderer = new TiledMapRenderer(graphicsDevice, tiledMap);
        }

        private void LoadLives()
        {
            var texture = content.Load<Texture2D>("Sprites/Heart");
            for (int i = 0; i < 5; i++)
            {
                hearts.Add(new Sprite(texture));
                hearts[i].Position.X = (i * 40) + 40;
                hearts[i].Position.Y = 40;
                hearts[i].Scale = 4f;
            }
        }

        private void LoadNextWave()
        {
            if (waves.Count > 0)
            {
                grunts.AddRange(waves.Peek());
                waves.Pop();
            }
            else
            {
                // you won! game over
                GoToMenu();
            }
        }

        private void GoToMenu()
        {
            game.ChangeState(new MenuState(game, graphicsDevice, content));
        }

        private void UpdateLives()
        {
            while (hearts.Count != player.Lives)
            {
                hearts.RemoveAt(hearts.Count - 1);

                // redraw HUD
                for (int i = 0; i < player.Lives; i++)
                {
                    hearts[i].Position.X = (i * 40) + 40;
                    hearts[i].Position.Y = 40;
                    hearts[i].Scale = 4f;
                }

                // respawn player
                player.Position = _spawnPoint;
                player.StartInvulnerableState();

                // despawn existing bullets
                foreach (var grunt in grunts)
                {
                    grunt.BulletList.Clear();
                }
                player.BulletList.Clear();
            }
        }
    }
}
