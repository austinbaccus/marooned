using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Marooned.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Marooned.Sprites
{
    public enum Direction
    {
        NONE,
        UP,
        LEFT,
        DOWN,
        RIGHT,
    }

    public class Player : AnimatedSprite
    {
        public static readonly HashSet<Keys> UP_KEYS = new HashSet<Keys>() { Keys.W };
        public static readonly HashSet<Keys> LEFT_KEYS = new HashSet<Keys>() { Keys.A };
        public static readonly HashSet<Keys> DOWN_KEYS = new HashSet<Keys>() { Keys.S };
        public static readonly HashSet<Keys> RIGHT_KEYS = new HashSet<Keys>() { Keys.D };
        public static readonly HashSet<Keys> SHOOT_UP_KEYS = new HashSet<Keys>() { Keys.Up };
        public static readonly HashSet<Keys> SHOOT_LEFT_KEYS = new HashSet<Keys>() { Keys.Left };
        public static readonly HashSet<Keys> SHOOT_DOWN_KEYS = new HashSet<Keys>() { Keys.Down };
        public static readonly HashSet<Keys> SHOOT_RIGHT_KEYS = new HashSet<Keys>() { Keys.Right };
        public static readonly HashSet<Keys> FOCUS_KEYS = new HashSet<Keys>() { Keys.LeftShift };

        // TODO: Maybe have a control class for handling multiple animations?
        //       (That way, other sprites like Grunt can reuse the same principle of grouped animations).
        private Dictionary<Direction, Animation> _flyAnimations;
        private Dictionary<Direction, Animation> _idleAnimations;
        private readonly double _flyAnimationSpeed = 0.07d;
        private readonly double _focusAnimationSpeedFactor = 2d;

        public float Speed;
        public float FocusSpeedFactor;
        public int Health = 5;

        public List<Bullet> BulletList = new List<Bullet>(); // List of bullets
        private double _lastBulletTimestamp = 0;
        private float _bulletLifespan = 2f;
        private float _bulletVelocity = 400f;
        private float _bulletFireRate = 100f;

        // TODO: Move these into some PlayerState struct or class
        private Direction _prevDirection = Direction.NONE;
        private Direction _currentDirection = Direction.NONE;
        private bool _prevIsMoving = false;
        private bool _isMoving = false;
        private bool _prevIsFocused = false;
        private bool _isFocused = false;

        private Sprite _hitboxSprite;
        private bool _shouldDrawHitbox = false;



        private Stopwatch timer = new Stopwatch(); // timer for damage



        public Player(Texture2D texture, Texture2D hitboxTexture) : base(texture)
        {
            // TODO: Find a better way to handle instantiating player animations
            const int SPRITE_WIDTH = 32;
            const int SPRITE_HEIGHT = 32;
            _flyAnimations = new Dictionary<Direction, Animation>()
            {
                [Direction.UP] = new Animation(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 1, 32 * 2, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 2, 32 * 2, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 3, 32 * 2, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
                [Direction.LEFT] = new Animation(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 1, 32 * 3, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 2, 32 * 3, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 3, 32 * 3, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
                [Direction.DOWN] = new Animation(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 1, 32 * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 2, 32 * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 3, 32 * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
                [Direction.RIGHT] = new Animation(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 1, 32 * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 2, 32 * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
                        new Rectangle(32 * 3, 32 * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
            };
            _idleAnimations = new Dictionary<Direction, Animation>()
            {
                [Direction.UP] = new Animation(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 0, 32 * 2, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
                [Direction.LEFT] = new Animation(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 0, 32 * 3, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
                [Direction.DOWN] = new Animation(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 0, 32 * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
                [Direction.RIGHT] = new Animation(
                    texture,
                    new Rectangle[]
                    {
                        new Rectangle(32 * 0, 32 * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
                    }
                ),
            };

            CurrentAnimation = _idleAnimations[Direction.UP];
            // TODO: Set animation speed somewhere else
            CurrentAnimation.Speed = _flyAnimationSpeed;

            _hitboxSprite = new Sprite(hitboxTexture);
        }

        public Hitbox Hitbox { get; set; }
        public bool IsFocused { get => _isFocused; }
        public bool ChangedState { get => _prevIsMoving != _isMoving
                                       || _prevDirection != _currentDirection
                                       || _prevIsFocused != _isFocused; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            if (_shouldDrawHitbox)
            {
                _hitboxSprite.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Shoot(gameTime);
            _hitboxSprite.Destination = new Rectangle(
                (int)(Position.X + Hitbox.Offset.X),
                (int)(Position.Y + Hitbox.Offset.Y),
                // Radius != Diameter
                Hitbox.Radius * 2,
                Hitbox.Radius * 2
            );
            _shouldDrawHitbox = IsFocused;

            _prevDirection = _currentDirection;
            _prevIsFocused = _isFocused;
            _prevIsMoving = _isMoving;



            IsHitTimer(gameTime); // Timer to show red damage




            base.Update(gameTime);
        }

        private void Move(GameTime gameTime)
        {
            // TODO: Maybe implement a custom KeyboardHandler class that stores things like previous keyboard states, etc...
            //       (MonoGame.Extended looks like it has something).
            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] pressedKeys = keyboardState.GetPressedKeys();
            bool upPressed = pressedKeys.Any(k => UP_KEYS.Contains(k));
            bool downPressed = pressedKeys.Any(k => DOWN_KEYS.Contains(k));
            bool leftPressed = pressedKeys.Any(k => LEFT_KEYS.Contains(k));
            bool rightPressed = pressedKeys.Any(k => RIGHT_KEYS.Contains(k));
            bool focusPressed = pressedKeys.Any(k => FOCUS_KEYS.Contains(k));

            _isFocused = focusPressed;
            // Multiply by <c>(float)gameTime.ElapsedGameTime.TotalSeconds</c> so the movement is consistent regardless of the current FPS
            float newSpeed = Speed * (_isFocused ? FocusSpeedFactor : 1f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            bool moved = false;

            if (upPressed)
            {
                Position.Y -= newSpeed;
                _currentDirection = Direction.UP;
                moved = true;
            }
            if (downPressed)
            {
                Position.Y += newSpeed;
                _currentDirection = Direction.DOWN;
                moved = true;
            }
            if (leftPressed)
            {
                Position.X -= newSpeed;
                _currentDirection = Direction.LEFT;
                moved = true;
            }
            if (rightPressed)
            {
                Position.X += newSpeed;
                _currentDirection = Direction.RIGHT;
                moved = true;
            }

            _isMoving = moved;

            if (ChangedState)
            {
                Dictionary<Direction, Animation> _sourceAnimation = _isMoving ? _flyAnimations : _idleAnimations;
                CurrentAnimation.Stop();
                CurrentAnimation = _sourceAnimation[_currentDirection];
                CurrentAnimation.Speed = _isFocused ? (_flyAnimationSpeed * _focusAnimationSpeedFactor) : _flyAnimationSpeed;
                CurrentAnimation.Play();
            }
        }

        public void Shoot(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - _lastBulletTimestamp > _bulletFireRate)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                Keys[] pressedKeys = keyboardState.GetPressedKeys();
                bool shootUpPressed = pressedKeys.Any(k => SHOOT_UP_KEYS.Contains(k));
                bool shootDownPressed = pressedKeys.Any(k => SHOOT_DOWN_KEYS.Contains(k));
                bool shootLeftPressed = pressedKeys.Any(k => SHOOT_LEFT_KEYS.Contains(k));
                bool shootRightPressed = pressedKeys.Any(k => SHOOT_RIGHT_KEYS.Contains(k));

                _lastBulletTimestamp = gameTime.TotalGameTime.TotalMilliseconds;

                if (shootRightPressed)
                { // Shoot right
                    BulletList.Add(BulletFactory.MakeBullet(_bulletLifespan, new Vector2(_bulletVelocity, 0), 2f, new Vector2(this.Position.X, this.Position.Y)));
                }
                else if (shootLeftPressed)
                { // Shoot left
                    BulletList.Add(BulletFactory.MakeBullet(_bulletLifespan, new Vector2(-_bulletVelocity, 0), 2f, new Vector2(this.Position.X, this.Position.Y)));
                }
                else if (shootDownPressed)
                { // Shoot down
                    BulletList.Add(BulletFactory.MakeBullet(_bulletLifespan, new Vector2(0, _bulletVelocity), 2f, new Vector2(this.Position.X, this.Position.Y)));
                }
                else if (shootUpPressed)
                { // Shoot up
                    BulletList.Add(BulletFactory.MakeBullet(_bulletLifespan, new Vector2(0, -_bulletVelocity), 2f, new Vector2(this.Position.X, this.Position.Y)));
                }
            }
        }




        public void IsHitTimer(GameTime gameTime)
        {
            if (isHit)
            {
                timer.Start(); // start timer
            }

            if (timer.ElapsedMilliseconds >= 50) // 2 Seconds elapsed
            {
                isHit = false;
                timer.Stop();
                timer.Reset();
            }

        }




    }
}
