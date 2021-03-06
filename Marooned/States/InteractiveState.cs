using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Marooned.Sprites;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using Marooned.Sprites.Enemies;
using Marooned.Controllers;
using DefaultEcs.System;
using Marooned.Systems;
using DefaultEcs;
using Marooned.Levels;

namespace Marooned.States
{
    public class InteractiveState : State
    {
        private List<ComponentOld> _components;
        private Vector2 _spawnPoint = new Vector2(300, 300);

        private string _playerSpritePath;
        private string _playerHitboxSpritePath;

        public InteractiveState(GameContext gameContext, string levelName, List<string> songPaths, string playerSpritePath, string playerHitboxSpritePath, bool isHard = false) : base(gameContext)
        {
            _components = new List<ComponentOld>();

            _playerSpritePath = playerSpritePath;
            _playerHitboxSpritePath = playerHitboxSpritePath;

            InputController = new InputController(this);

            Entity levelEntity = gameContext.LevelsInterpreter.GetLevelEntity(levelName, World);
            LevelInfo levelInfo = levelEntity.Get<LevelInfo>();

            CurrentLevel = new Level(this, gameContext, levelInfo, songPaths, playerSpritePath, playerHitboxSpritePath, isHard);

            Systems = new SequentialSystem<GameContext>(
                new PlayerBulletCollisionSystem(World),
                new EnemyBulletCollisionSystem(World),
                new PlayerCollisionSystem(World),
                new EnemyCollisionSystem(World),
                //new DamageSystem(World),
                //new RemoveEntitySystem(World),
                new BulletBoundarySystem(World),
                new EnemyBoundarySystem(World),
                new BulletCollisionRemovalSystem(World),
                new SpawnActionSystem(World),
                new SpawnSystem(World),
                new MoveEntitySystem(World),
                new MoveSystem(World),
                new ScriptSystem(World),
                new ScriptEntitySystem(World),
                new AnimationSystem(World),
                new DrawSystem(World),
                new DrawAnimationSystem(World)
            );
        }

        public Player Player { get; private set; }
        public OrthographicCamera _camera { get; private set; }
        public List<GruntOld> Grunts { get; private set; } = new List<GruntOld>();

        public List<Sprite> Hearts = new List<Sprite>();
        public List<Sprite> Bombs = new List<Sprite>();

        public Level CurrentLevel { get; private set; }

        public override void Dispose()
        {
            Systems.Dispose();

            base.Dispose();
        }

        public override void Update()
        {
            CurrentLevel.Update();

            // update lives
            //UpdateLives();
            //UpdateBombs();

            //PostUpdate();
        }

        public override void LoadContent()
        {
            GraphicsDevice graphicsDevice = GameContext.GraphicsDevice;
            var viewportadapter = new BoxingViewportAdapter(
                GameContext.Game.Window,
                graphicsDevice,
                graphicsDevice.Viewport.Width,
                graphicsDevice.Viewport.Height
            );
            Camera = new OrthographicCamera(viewportadapter)
            {
                Zoom = 2f,
            };

            LoadSprites(_playerSpritePath, _playerHitboxSpritePath);
            //LoadEnemies();
            //LoadMusic(_songPaths);
            //LoadMap(_mapPath);

            CurrentLevel.LoadContent();

            //LoadLives();
            //LoadBombs();
        }

        private void LoadSprites(string playerSpritePath, string playerHitboxSpritePath)
        {
            var texture = GameContext.Content.Load<Texture2D>(playerSpritePath);
            var hitboxTexture = GameContext.Content.Load<Texture2D>(playerHitboxSpritePath);

            Player = new Player(GameContext, texture, hitboxTexture, InputController)
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

        //private void LoadEnemies()
        //{
        //    List<Grunt> wave1 = new List<Grunt>
        //    {
        //        EnemyFactory.MakeGrunt(GameContext, "skeleton", new Vector2(225, 100), 5),
        //    };

        //    List<Grunt> wave2 = new List<Grunt>
        //    {
        //        EnemyFactory.MakeGrunt(GameContext, "skeleton", new Vector2(225, 100), 5),
        //        EnemyFactory.MakeGrunt(GameContext, "skeleton", new Vector2(275, 100), 5),
        //        EnemyFactory.MakeGrunt(GameContext, "skeleton_dangerous", new Vector2(250, 100), 5)
        //    };

        //    List<Grunt> wave3 = new List<Grunt>
        //    {
        //        EnemyFactory.MakeGrunt(GameContext, "skeleton_mage", new Vector2(250, 100), 5)
        //    };

        //    // Boss goes here, replace skeleton_mage with boss assets/props
        //    List<Grunt> wave4 = new List<Grunt>
        //    {
        //        EnemyFactory.MakeGrunt(GameContext, "miniboss1", new Vector2(250, 300), 10)
        //    };

        //    List<Grunt> wave5 = new List<Grunt>
        //    {
        //        EnemyFactory.MakeGrunt(GameContext, "boss1", new Vector2(250, 300), 30)
        //    };

        //    //Waves.Push(wave5);
        //    //Waves.Push(wave4);
        //    //Waves.Push(wave3);
        //    //Waves.Push(wave2);
        //    Waves.Push(wave1);
        //}

        //private void LoadMusic(List<string> songPaths)
        //{
        //    int songIdx = 0;

        //    // load first song
        //    Song song = GameContext.Content.Load<Song>(songPaths[songIdx]);
        //    MediaPlayer.Play(song);

        //    // TODO: Find a better way to handle multiple songs, since it
        //    // doesn't work with the new handling of content loading/unloading.
        //    // (Maybe a PlaylistManager or SongManager class?).
        //    // Note: with the current handling of content loading/unloading, you
        //    // do NOT need to manually call `Dispose()`; that will automatically be
        //    // called when a state is unloaded. That might be why this doesn't work.
        //    //MediaPlayer.ActiveSongChanged += (s, e) =>
        //    //{
        //    //    // dispose of current song
        //    //    song.Dispose();
        //    //    songIdx = (songIdx + 1) % songPaths.Count;
        //    //    System.Diagnostics.Debug.WriteLine("Song ended and disposed");

        //    //    // play next song
        //    //    song = GameContext.Content.Load<Song>(songPaths[songIdx]);
        //    //    MediaPlayer.Play(song);
        //    //};
        //}

        //private void LoadMap(string mapPath)
        //{
        //    TiledMap = GameContext.Content.Load<TiledMap>(mapPath);
        //    TiledMapRenderer = new TiledMapRenderer(GameContext.GraphicsDevice, TiledMap);
        //}

        private void LoadLives()
        {
            var texture = GameContext.Content.Load<Texture2D>("Sprites/Heart");
            for (int i = 0; i < 5; i++)
            {
                Hearts.Add(new Sprite(texture));
                Hearts[i].Position.X = (i * 40) + 40;
                Hearts[i].Position.Y = 40;
                Hearts[i].Scale = 4f;
            }
        }
        private void LoadBombs()
        {
            var texture = GameContext.Content.Load<Texture2D>("Sprites/Bomb");
            for (int i = 0; i < 3; i++)
            {
                Bombs.Add(new Sprite(texture));
                Bombs[i].Position.X = (i * 40) + 40;
                Bombs[i].Position.Y = 80;
                Bombs[i].Scale = 1f;
            }
        }

        //private void LoadNextWave()
        //{
        //    if (Waves.Count > 0)
        //    {
        //        Grunts.AddRange(Waves.Peek());
        //        Waves.Pop();
        //    }
        //    else
        //    {
        //        // you won! game over
        //        GameContext.StateManager.SwapState(new MenuState(GameContext));
        //    }
        //}

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
        private void UpdateBombs()
        {
            while (Player.Bombs < Bombs.Count)
            {
                Bombs.RemoveAt(Bombs.Count - 1);
            }
        }

        //public void OnDeath()
        //{
        //    _components.Remove(Player);
        //    UpdateEnabled = false;
        //    GameContext.StateManager.PushState(new GameOverState(GameContext));
        //}

        // TODO: Use a creational pattern to create an InteractiveState (or states in general?)
        public static InteractiveState CreateDefaultState(GameContext gameContext)
        {
            // TODO: Right now we are manually passing in map parameters when changing state. Later on this will be delegated to the Level Interpreter.
            return new InteractiveState(
                gameContext,
                "level1",
                new List<string>()
                {
                    "Sounds/Music/ConcernedApe - Stardew Valley 1.5 Original Soundtrack - 03 Volcano Mines (Molten Jelly)",
                    "Sounds/Music/ConcernedApe - Stardew Valley 1.5 Original Soundtrack - 01 Ginger Island"
                },
                "Sprites/IslandParrot",
                "Sprites/PlayerHitbox"
            );
        }

        public static InteractiveState CreateHardModeState(GameContext gameContext)
        {
            // TODO: Right now we are manually passing in map parameters when changing state. Later on this will be delegated to the Level Interpreter.
            return new InteractiveState(
                gameContext,
                "level1",
                new List<string>()
                {
                    "Sounds/Music/ConcernedApe - Stardew Valley 1.5 Original Soundtrack - 03 Volcano Mines (Molten Jelly)",
                    "Sounds/Music/ConcernedApe - Stardew Valley 1.5 Original Soundtrack - 01 Ginger Island"
                },
                "Sprites/IslandParrot",
                "Sprites/PlayerHitbox",
                isHard: true
            );
        }

        //public void DrawMap()
        //{
        //    GameContext.SpriteBatch.Begin(sortMode: SpriteSortMode.Deferred, transformMatrix: Camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);

        //    TiledMapRenderer.Draw(Camera.GetViewMatrix());

        //    foreach (var component in _components)
        //    {
        //        component.Draw(GameContext);
        //    }

        //    foreach (var grunt in Grunts)
        //    {
        //        grunt.Draw(GameContext);
        //        foreach (var bullet in grunt.BulletList)
        //        {
        //            bullet.Draw(GameContext);
        //        }
        //    }

        //    foreach (var bullet in Player.BulletList)
        //    {
        //        bullet.Draw(GameContext);
        //    }

        //    GameContext.SpriteBatch.End();
        //}

        public void DrawHUD()
        {
            GameContext.SpriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);

            foreach (Sprite heart in Hearts)
            {
                heart.Draw(GameContext);
            }
            foreach (Sprite heart in Bombs)
            {
                heart.Draw(GameContext);
            }

            GameContext.SpriteBatch.End();
        }

        public override void Draw()
        {
            GameContext.GraphicsDevice.Clear(Color.CornflowerBlue);
            //DrawMap();

            CurrentLevel.Draw();

            DrawHUD();
            Systems.Update(GameContext);

        }
    }
}
