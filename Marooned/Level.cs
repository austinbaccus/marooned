using Marooned.Controllers;
using Marooned.Factories;
using Marooned.Sprites;
using Marooned.Sprites.Enemies;
using Marooned.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System.Collections.Generic;

namespace Marooned
{
    public class Level : ILifeCycle
    {
        private List<ComponentOld> _components; // Might need to remove?
        private Vector2 _spawnPoint = new Vector2(300, 300);
        private string _mapPath;
        private List<string> _songPaths;
        private string _playerSpritePath;
        private string _playerHitboxSpritePath;
        private InputController _inputController;
        private State _state;
        private bool _isHard = false;


        public List<Sprite> Bombs = new List<Sprite>();
        public List<Sprite> Hearts = new List<Sprite>();

        public Level(State state, GameContext gameContext, string mapPath, List<string> songPaths, string playerSpritePath, string playerHitboxSpritePath, bool isHard = false)
        {
            _state = state;
            GameContext = gameContext;

            _components = new List<ComponentOld>();
            _mapPath = mapPath;
            _songPaths = songPaths;
            _playerSpritePath = playerSpritePath;
            _playerHitboxSpritePath = playerHitboxSpritePath;

            _inputController = GameContext.StateManager.CurrentState.InputController;

            //GraphicsDevice graphicsDevice = GameContext.GraphicsDevice;

            //var viewportadapter = new BoxingViewportAdapter(
            //    GameContext.Game.Window,
            //    graphicsDevice,
            //    graphicsDevice.Viewport.Width,
            //    graphicsDevice.Viewport.Height
            //);

            //_camera = GameContext.StateManager.CurrentState.Camera;
            //_camera = new OrthographicCamera(viewportadapter)
            //{
            //    Zoom = 2f,
            //};

            if (isHard)
            {
                _isHard = true;
            }

            Script = new Script();
        }

        public TiledMap TiledMap { get; private set; }
        public Player Player { get; private set; }
        public List<GruntOld> Grunts { get; private set; } = new List<GruntOld>();
        public Stack<List<GruntOld>> Waves { get; private set; } = new Stack<List<GruntOld>>();
        public TiledMapRenderer TiledMapRenderer { get; private set; }

        public GameContext GameContext { get; set; }
        public bool PlayerAlive { get => Player.Lives > 0; }

        public Script Script { get; }

        public void Initialize()
        {
            
        }

        public void LoadContent()
        {
            LoadSprites(_playerSpritePath, _playerHitboxSpritePath);
            LoadEnemies();
            LoadMusic(_songPaths);
            LoadMap(_mapPath);
            LoadBombs();
            LoadLives();
        }

        private void LoadMusic(List<string> songPaths)
        {
            int songIdx = 0;

            // load first song
            Song song = GameContext.Content.Load<Song>(songPaths[songIdx]);
            MediaPlayer.Play(song);
        }

        private void LoadMap(string mapPath)
        {
            TiledMap = GameContext.Content.Load<TiledMap>(mapPath);
            TiledMapRenderer = new TiledMapRenderer(GameContext.GraphicsDevice, TiledMap);
        }

        private void LoadEnemies()
        {
            //List<GruntOld> wave1 = new List<GruntOld>
            //{
            //    EnemyFactory.MakeGrunt(GameContext, _state.World, "skeleton", new Vector2(225, 100)),
            //};

            EnemyFactory.MakeGrunt(GameContext, _state.World, "skeleton", new Vector2(0, 00));

            //Waves.Push(wave1);
        }

        private void LoadSprites(string playerSpritePath, string playerHitboxSpritePath)
        {
            var texture = GameContext.Content.Load<Texture2D>(playerSpritePath);
            var hitboxTexture = GameContext.Content.Load<Texture2D>(playerHitboxSpritePath);

            Player = new Player(GameContext, texture, hitboxTexture, _inputController)
            {
                Position = _spawnPoint,
                Speed = 120f,
                FocusSpeedFactor = 0.5f,
            };
            Player.Hitbox = new Hitbox(Player)
            {
                Radius = 5,
                Offset = new Vector2(0, 5f),
            };

            if (_isHard)
            {
                Player.Bombs = 1;
                Player.Lives = 2;
            }
            else
            {
                Player.Bombs = 3;
                Player.Lives = 5;
            }

            _components.Add(Player);
        }

        public void UnloadContent()
        {
        }

        public void Dispose()
        {
        }

        public void Update()
        {
            _inputController.Update(GameContext);

            TiledMapRenderer.Update(GameContext.GameTime);

            // 4 is a magic number to get the player nearly center to the screen
            GameContext.StateManager.CurrentState.Camera.LookAt(Player.Position + Player.Hitbox.Offset * 4);

            foreach (var component in _components)
            {
                component.Update(GameContext);
            }

            // check player for damage
            foreach (var grunt in Grunts)
            {
                if (!PlayerAlive) break;

                grunt.Update(GameContext);

                //for (int i = 0; i < grunt.BulletList.Count; i++)
                //{
                //    Bullet bullet = grunt.BulletList[i];
                //    bullet.Update(GameContext);

                //    if (!Player.IsInvulnerable)
                //    {
                //        // did it hit the player?
                //        if (Player.Hitbox.IsTouching(bullet.Hitbox))
                //        {
                //            Player.isHit = true; // Show red damage on grunt

                //            Player.Lives--;
                //            if (Player.Lives <= 0)
                //            {
                //                // "game over man! game over!"
                //                OnDeath();
                //                break;
                //            }

                //            grunt.BulletList.RemoveAt(i);
                //            i--;
                //        }
                //    }
                //}
            }

            if (PlayerAlive)
            {
                //// check enemies for damage
                //for (int i = 0; i < Player.BulletList.Count; i++)
                //{
                //    Bullet bullet = Player.BulletList[i];
                //    bullet.Update(GameContext);

                //    for (int j = 0; j < Grunts.Count; j++)
                //    {
                //        if (Grunts[j].Hitbox.IsTouching(bullet.Hitbox))
                //        {
                //            Grunts[j].isHit = true; // Show red damage on grunt
                //            Grunts[j].Health--;
                //            if (Grunts[j].Health <= 0)
                //            {
                //                Grunts.RemoveAt(j);
                //                j--;
                //            }

                //            Player.BulletList.RemoveAt(i);
                //            i--;
                //            break;
                //        }
                //    }
                //}
            }
            UpdateLives();
            UpdateBombs();

            PostUpdate();
        }

        public void OnDeath()
        {
            _components.Remove(Player);
            //UpdateEnabled = false;
            GameContext.StateManager.PushState(new GameOverState(GameContext));
        }

        public void PostUpdate()
        {
            //// remove sprites if they're not needed
            //for (int i = 0; i < Player.BulletList.Count; i++)
            //{
            //    if (Player.BulletList[i].IsRemoved)
            //    {
            //        Player.BulletList.RemoveAt(i);
            //        i--;
            //    }
            //}
            for (int i = 0; i < Grunts.Count; i++)
            {
                if (Grunts[i].IsRemoved)
                {
                    Grunts.RemoveAt(i);
                    i--;
                }
            }
            if (Grunts.Count <= 0)
            {
                LoadNextWave();
            }
        }

        private void LoadNextWave()
        {
            //if (Waves.Count > 0)
            //{
            //    Grunts.AddRange(Waves.Peek());
            //    Waves.Pop();
            //}
            //else
            //{
            //    // you won! game over
            //    GameContext.StateManager.SwapState(new MenuState(GameContext));
            //}
        }

        public void Draw()
        {
            

            DrawMap();

            GameContext.SpriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);
            foreach (Sprite heart in Hearts)
            {
                heart.Draw(GameContext);
            }
            foreach (Sprite bomb in Bombs)
            {
                bomb.Draw(GameContext);
            }
            GameContext.SpriteBatch.End();
        }

        public void DrawMap()
        {
            GameContext.SpriteBatch.Begin(sortMode: SpriteSortMode.Deferred, transformMatrix: GameContext.StateManager.CurrentState.Camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);

            TiledMapRenderer.Draw(GameContext.StateManager.CurrentState.Camera.GetViewMatrix());

            foreach (var component in _components)
            {
                component.Draw(GameContext);
            }

            foreach (var grunt in Grunts)
            {
                grunt.Draw(GameContext);
                //foreach (var bullet in grunt.BulletList)
                //{
                //    bullet.Draw(GameContext);
                //}
            }

            //foreach (var bullet in Player.BulletList)
            //{
            //    bullet.Draw(GameContext);
            //}

            GameContext.SpriteBatch.End();
        }


        private void UpdateBombs()
        {
            while (Player.Bombs < Bombs.Count)
            {
                Bombs.RemoveAt(Bombs.Count - 1);
            }
        }
        private void LoadBombs()
        {
            var texture = GameContext.Content.Load<Texture2D>("Sprites/Bomb");
            for (int i = 0; i < Player.Bombs; i++)
            {
                Bombs.Add(new Sprite(texture));
                Bombs[i].Position.X = (i * 40) + 40;
                Bombs[i].Position.Y = 80;
                Bombs[i].Scale = 0.8f;
            }
        }
        private void UpdateLives()
        {
            while (Hearts.Count != Player.Lives)
            {
                Hearts.RemoveAt(Hearts.Count - 1);

                // update HUD
                for (int i = 0; i < Player.Lives; i++)
                {
                    Hearts[i].Position.X = (i * 40) + 40;
                    Hearts[i].Position.Y = 40;
                    Hearts[i].Scale = 4f;
                }

                // respawn player
                Player.Position = _spawnPoint;
                Player.StartInvulnerableState();
            }
        }
        private void LoadLives()
        {
            var texture = GameContext.Content.Load<Texture2D>("Sprites/Heart");
            for (int i = 0; i < Player.Lives; i++)
            {
                Hearts.Add(new Sprite(texture));
                Hearts[i].Position.X = (i * 40) + 40;
                Hearts[i].Position.Y = 40;
                Hearts[i].Scale = 4f;
            }
        }
    }
}
