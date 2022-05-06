using System;
using System.Collections.Generic;
using System.Diagnostics;
using Marooned.Actions;
using Marooned.Animation;
using Marooned.Controllers;
using Marooned.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned.Sprites
{
    public class Player : AnimatedSprite
    {
        // TODO: Maybe have a control class for handling multiple animations?
        //       (That way, other sprites like Grunt can reuse the same principle of grouped animations).
        private Dictionary<Direction, AnimationOld> _flyAnimations;
        private Dictionary<Direction, AnimationOld> _idleAnimations;
        private readonly double _flyAnimationSpeed = 0.07d;
        private readonly double _focusAnimationSpeedFactor = 2d;

        public float Speed;
        public float FocusSpeedFactor;
        public int Lives = 5;
        public int Bombs = 3;

        private double _lastBulletTimestamp = 0;
        private float _bulletLifespan = 2f;
        private float _bulletVelocity = 400f;
        private float _bulletFireRate = 100f;

        private bool _prevIsMoving = false;
        private bool _isMoving = false;
        private bool _prevIsFocused = false;
        private bool _isFocused = false;

        private Sprite _hitboxSprite;
        private bool _shouldDrawHitbox = false;

        // TODO: Move these into some PlayerState struct or class
        private Direction _prevDirection = Direction.NONE;
        private Direction _currentDirection = Direction.NONE;

        public bool isHit = false;
        private Stopwatch _damageTimer = new Stopwatch();
        private Stopwatch _invulnerabilityTimer = new Stopwatch();

        private InputController _inputController;

        public Player(GameContext gameContext, Texture2D texture, Texture2D hitboxTexture, InputController inputController) : base(texture)
        {
            // TODO: Find a better way to handle instantiating player animations
            const int SPRITE_WIDTH = 32;
            const int SPRITE_HEIGHT = 32;
            _flyAnimations = new Dictionary<Direction, AnimationOld>()
            {
                [Direction.UP] = new AnimationOld(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 1, 32 * 2, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 2, 32 * 2, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 3, 32 * 2, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
                [Direction.LEFT] = new AnimationOld(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 1, 32 * 3, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 2, 32 * 3, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 3, 32 * 3, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
                [Direction.DOWN] = new AnimationOld(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 1, 32 * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 2, 32 * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 3, 32 * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
                [Direction.RIGHT] = new AnimationOld(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 1, 32 * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 2, 32 * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 3, 32 * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
            };
            _idleAnimations = new Dictionary<Direction, AnimationOld>()
            {
                [Direction.UP] = new AnimationOld(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 0, 32 * 2, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
                [Direction.LEFT] = new AnimationOld(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 0, 32 * 3, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
                [Direction.DOWN] = new AnimationOld(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 0, 32 * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
                [Direction.RIGHT] = new AnimationOld(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 0, 32 * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
            };

            // TODO: Find a better way to initialize InputController
            _inputController = inputController;
            _inputController.OnKeyDownEvent += HandleKeyDown;
            _inputController.OnKeyUpEvent += HandleKeyUp;

            CurrentAnimation = _idleAnimations[Direction.UP];
            // TODO: Set animation speed somewhere else
            CurrentAnimation.Speed = _flyAnimationSpeed;

            _hitboxSprite = new Sprite(hitboxTexture);

            GameContext = gameContext;
        }

        public GameContext GameContext { get; set; }

        public Hitbox Hitbox { get; set; }
        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                _prevIsFocused = _isFocused;
                _isFocused = value;
            }
        }
        public bool IsMoving
        {
            get => _isMoving;
            set
            {
                _prevIsMoving = _isMoving;
                _isMoving = value;
            }
        }
        public Direction CurrentDirection
        {
            get => _currentDirection;
            set
            {
                _prevDirection = CurrentDirection;
                _currentDirection = value;
            }
        }
        public bool ChangedState { get => _prevIsMoving != _isMoving
                                       || _prevDirection != _currentDirection
                                       || _prevIsFocused != _isFocused; }
        public bool IsInvulnerable { get; set; } = false;
        public ScriptOld Script { get; private set; } = new ScriptOld();

        public void HandleKeyDown(object sender, KeyEventArgs e)
        {
            bool focusPressed = InputController.FOCUS_KEYS.Contains(e.Key);

            if (focusPressed)
            {
                IsFocused = true;
                Script.AddAction(new FocusAction(this));
            }

            HandleInputMove(e);
            HandleInputShoot(e);
        }

        public void HandleKeyUp(object sender, KeyEventArgs e)
        {
            bool upReleased = InputController.UP_KEYS.Contains(e.Key);
            bool downReleased = InputController.DOWN_KEYS.Contains(e.Key);
            bool leftReleased = InputController.LEFT_KEYS.Contains(e.Key);
            bool rightReleased = InputController.RIGHT_KEYS.Contains(e.Key);
            bool bombReleased = InputController.BOMB_KEYS.Contains(e.Key);
            if (upReleased || downReleased || leftReleased || rightReleased)
            {
                IsMoving = false;
            }

            bool focusReleased = InputController.FOCUS_KEYS.Contains(e.Key);
            if (focusReleased)
            {
                IsFocused = false;
                Script.AddAction(new UnfocusAction(this));
            }

            // cheat mode
            if (InputController.CHEAT_KEYS.Contains(e.Key))
            {
                IsInvulnerable = !IsInvulnerable;
            }

            // bomb
            if (InputController.BOMB_KEYS.Contains(e.Key) && Bombs > 0)
            {
                // explode the bomb
                // remove all enemies
                Script.AddAction(new ClearEnemiesAction(GameContext.StateManager.CurrentState.World));
                Bombs--;
            }
        }

        public override void Draw(GameContext gameContext)
        {
            base.Draw(gameContext);
            if (_shouldDrawHitbox)
            {
                _hitboxSprite.Draw(gameContext);
            }
        }

        public override void Update(GameContext gameContext)
        {
            Script.ExecuteAll(gameContext);
            UpdateAnimation();
            _hitboxSprite.Destination = new Rectangle(
                (int)(Position.X + Hitbox.Offset.X),
                (int)(Position.Y + Hitbox.Offset.Y),
                // Radius != Diameter
                Hitbox.Radius * 2,
                Hitbox.Radius * 2
            );
            _shouldDrawHitbox = IsFocused;

            UpdateDamageTimer(gameContext);
            UpdateInvulnerabilityTimer(gameContext);

            base.Update(gameContext);
        }

        private AnimationOld GetRelevantAnimation()
        {
            Dictionary<Direction, AnimationOld> _sourceAnimation = IsMoving ? _flyAnimations : _idleAnimations;
            return _sourceAnimation[CurrentDirection];
        }

        private void UpdateAnimation()
        {
            if (ChangedState)
            {
                AnimationOld animation = GetRelevantAnimation();
                CurrentAnimation.Stop();
                CurrentAnimation = animation;
                CurrentAnimation.Speed = IsFocused ? (_flyAnimationSpeed * _focusAnimationSpeedFactor) : _flyAnimationSpeed;
                CurrentAnimation.Play();
            }
        }

        private void HandleInputMove(KeyEventArgs e)
        {
            bool upPressed = InputController.UP_KEYS.Contains(e.Key);
            bool downPressed = InputController.DOWN_KEYS.Contains(e.Key);
            bool leftPressed = InputController.LEFT_KEYS.Contains(e.Key);
            bool rightPressed = InputController.RIGHT_KEYS.Contains(e.Key);

            float newSpeed = Speed * (IsFocused ? FocusSpeedFactor : 1f);

            if (upPressed)
            {
                Script.AddAction(new LinearMoveActionOld(this)
                {
                    Direction = new Vector2(0, -1),
                    Speed = newSpeed,
                });
                CurrentDirection = Direction.UP;
                IsMoving = true;
            }
            if (downPressed)
            {
                Script.AddAction(new LinearMoveActionOld(this)
                {
                    Direction = new Vector2(0, 1),
                    Speed = newSpeed,
                });
                CurrentDirection = Direction.DOWN;
                IsMoving = true;
            }
            if (leftPressed)
            {
                Script.AddAction(new LinearMoveActionOld(this)
                {
                    Direction = new Vector2(-1, 0),
                    Speed = newSpeed,
                });
                CurrentDirection = Direction.LEFT;
                IsMoving = true;
            }
            if (rightPressed)
            {
                Script.AddAction(new LinearMoveActionOld(this)
                {
                    Direction = new Vector2(1, 0),
                    Speed = newSpeed,
                });
                CurrentDirection = Direction.RIGHT;
                IsMoving = true;
            }
        }

        public void HandleInputShoot(KeyEventArgs e)
        {
            if (e.GameContext.GameTime.TotalGameTime.TotalMilliseconds - _lastBulletTimestamp > _bulletFireRate)
            {
                _lastBulletTimestamp = e.GameContext.GameTime.TotalGameTime.TotalMilliseconds;

                bool shootUpPressed = InputController.SHOOT_UP_KEYS.Contains(e.Key);
                bool shootDownPressed = InputController.SHOOT_DOWN_KEYS.Contains(e.Key);
                bool shootLeftPressed = InputController.SHOOT_LEFT_KEYS.Contains(e.Key);
                bool shootRightPressed = InputController.SHOOT_RIGHT_KEYS.Contains(e.Key);

                if (shootRightPressed)
                {
                    Script.AddAction(new ShootAction()
                    {
                        LifeSpan = _bulletLifespan,
                        Velocity = new Vector2(_bulletVelocity, 0),
                        Damage = 2f,
                        Origin = new Vector2(this.Position.X, this.Position.Y),
                    });
                }
                else if (shootLeftPressed)
                {
                    Script.AddAction(new ShootAction()
                    {
                        LifeSpan = _bulletLifespan,
                        Velocity = new Vector2(-_bulletVelocity, 0),
                        Damage = 2f,
                        Origin = new Vector2(this.Position.X, this.Position.Y),
                    });
                }
                else if (shootDownPressed)
                {
                    Script.AddAction(new ShootAction()
                    {
                        LifeSpan = _bulletLifespan,
                        Velocity = new Vector2(0, _bulletVelocity),
                        Damage = 2f,
                        Origin = new Vector2(this.Position.X, this.Position.Y),
                    });
                }
                else if (shootUpPressed)
                {
                    Script.AddAction(new ShootAction()
                    {
                        LifeSpan = _bulletLifespan,
                        Velocity = new Vector2(0, -_bulletVelocity),
                        Damage = 2f,
                        Origin = new Vector2(this.Position.X, this.Position.Y),
                    });
                }
            }
        }

        public void UpdateDamageTimer(GameContext gameContext)
        {
            if (isHit)
            {
                _damageTimer.Start(); // start timer
                Color = Color.Red;
            }

            if (_damageTimer.ElapsedMilliseconds >= 50)
            {
                isHit = false;
                Color = Color.White;
                _damageTimer.Stop();
                _damageTimer.Reset();
            }
        }

        public void StartInvulnerableState()
        {
            IsInvulnerable = true;
            _invulnerabilityTimer.Start();
        }

        public void UpdateInvulnerabilityTimer(GameContext gameContext)
        {
            if (IsInvulnerable)
            {
                Color = Color.White * (float)((Math.Sin(_invulnerabilityTimer.ElapsedMilliseconds * gameContext.GameTime.ElapsedGameTime.TotalSeconds * 5) + 1) / 2);
            }
            else
            {
                Color = Color.White;
            }

            if (_invulnerabilityTimer.Elapsed.TotalSeconds >= 2)
            {
                IsInvulnerable = false;
                _invulnerabilityTimer.Stop();
                _invulnerabilityTimer.Reset();
                Color = Color.White;
            }
        }
    }
}
