using Marooned.Controllers;
using Marooned.Factories;
using Marooned.Levels;
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
        private List<string> _songPaths;
        private string _playerSpritePath;
        private string _playerHitboxSpritePath;
        private InputController _inputController;
        private State _state;

        public Level(State state, GameContext gameContext, LevelInfo levelInfo, string mapPath, List<string> songPaths, string playerSpritePath, string playerHitboxSpritePath)
        {
            _state = state;
            GameContext = gameContext;

            _components = new List<ComponentOld>();
            _songPaths = songPaths;
            _playerSpritePath = playerSpritePath;
            _playerHitboxSpritePath = playerHitboxSpritePath;

            _inputController = GameContext.StateManager.CurrentState.InputController;

            LevelInfo = levelInfo;

            Script = new Script();
        }

        public TiledMap TiledMap { get; private set; }
        public Player Player { get; private set; }
        public List<GruntOld> Grunts { get; private set; } = new List<GruntOld>();
        public Stack<List<GruntOld>> Waves { get; private set; } = new Stack<List<GruntOld>>();
        public TiledMapRenderer TiledMapRenderer { get; private set; }
        public LevelInfo LevelInfo { get; set; }

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
            LoadMap();
        }

        private void LoadMusic(List<string> songPaths)
        {
            int songIdx = 0;

            // load first song
            Song song = GameContext.Content.Load<Song>(songPaths[songIdx]);
            MediaPlayer.Play(song);
        }

        private void LoadMap()
        {
            TiledMap = GameContext.Content.Load<TiledMap>(LevelInfo.MapPath);
            TiledMapRenderer = new TiledMapRenderer(GameContext.GraphicsDevice, TiledMap);
        }

        private void LoadEnemies()
        {
            //EnemyFactory.MakeGrunt(GameContext, _state.World, "skeleton", new Vector2(0, 00));
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

            PostUpdate();
        }

        public void OnDeath()
        {
            _components.Remove(Player);
            GameContext.StateManager.PushState(new GameOverState(GameContext));
        }

        public void PostUpdate()
        {
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

        }

        public void Draw()
        {
            DrawMap();
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
            }
            
            GameContext.SpriteBatch.End();
        }
    }
}
