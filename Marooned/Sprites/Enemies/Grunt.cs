using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Marooned.Factories;
using System.Diagnostics;

namespace Marooned.Sprites.Enemies
{
    public class Grunt : AnimatedSprite
    {
        protected List<double> _firePattern;
        protected List<Tuple<Vector2, int>> _movePattern;
        protected int _currentMovePattern = 0;
        protected int _currentMovePatternTimeRemaining = 0;

        public int Health = 0;

        public List<Bullet> BulletList = new List<Bullet>(); // List of bullets
        protected double _lastBulletTimestamp = 0;
        protected float _bulletLifespan = 2f;
        protected float _bulletVelocity = 250f;
        protected float _bulletFireRate = 1000;
        protected Vector2 _referenceVector = new Vector2(0f, 1f);

        public bool IsRemoved = false; // Grunt should be removed from list

#if DEBUG
        public Sprite HitboxSprite;
#endif

        public bool isHit; // Red Damage
        private Stopwatch _damageTimer = new Stopwatch(); // timer for damage

        public Grunt(Texture2D texture, Rectangle[] animSources, FiringPattern.Pattern firingPattern, MovePatternOld.Pattern movementPattern, int health) : base(texture, animSources)
        {
            Health = health;
            _firePattern = FiringPattern.GetPattern(firingPattern);
            _movePattern = MovePatternOld.GetPattern(movementPattern);
            _currentMovePatternTimeRemaining = _movePattern[0].Item2;

            CurrentAnimation.Play();
        }
        
        public Hitbox Hitbox { get; set; }

        public override void Update(GameContext gameContext)
        {
            Move(gameContext);
            Shoot(gameContext);
            CurrentAnimation.Update(gameContext);

            UpdateDamageTimer(gameContext);

#if DEBUG
            HitboxSprite.Destination = new Rectangle(
                (int)(Position.X + Hitbox.Offset.X),
                (int)(Position.Y + Hitbox.Offset.Y),
                Hitbox.Radius * 2,
                Hitbox.Radius * 2
            );
#endif
        }

#if DEBUG
        public override void Draw(GameContext gameContext)
        {
            base.Draw(gameContext);
            HitboxSprite.Draw(gameContext);
        }
#endif
        float test = 100;
        protected virtual void Move(GameContext gameContext)
        {
            
            if (!IsRemoved)
            {
                float dt = (float)gameContext.GameTime.ElapsedGameTime.TotalSeconds;
                // move the grunt
                Position = new Vector2(
                        Position.X + (float)Math.Cos(_currentMovePatternTimeRemaining * dt) * test * (float).01,
                        Position.Y + (float)Math.Sin(_currentMovePatternTimeRemaining * dt) * test * (float).01
                    ) ;

                // decrement time left for that instruction
                _currentMovePatternTimeRemaining--;
                test -= .05f;
                // if time <= 0, increment curInstruction index
                if (_currentMovePatternTimeRemaining <= 0)
                {
                    _currentMovePattern++;
                    if (_currentMovePattern >= _movePattern.Count)
                    {
                        // despawn the grunt because the grunt has finished moving
                        IsRemoved = true;
                    }
                    else
                    {
                        _currentMovePatternTimeRemaining = _movePattern[_currentMovePattern].Item2;
                    }
                }
            }
        }

        //public override Vector2 Transform(GameContext gameContext, Vector2 currentPosition, Vector2 currentVelocity)
        //{
        //    float timer = 0;
        //    float dt = (float)gameContext.GameTime.ElapsedGameTime.TotalSeconds;
        //    return new Vector2(
        //        currentPosition.X + (float)Math.Cos(timer * dt),
        //        currentPosition.Y + (float)Math.Sin(timer * dt)
        //    );
        //}

        protected virtual void Shoot(GameContext gameContext)
        {
            if (gameContext.GameTime.TotalGameTime.TotalMilliseconds - _lastBulletTimestamp > _bulletFireRate)
            {
                _lastBulletTimestamp = gameContext.GameTime.TotalGameTime.TotalMilliseconds;

                foreach (double angle in _firePattern)
                {
                    float dX = (float)Math.Cos(angle + Math.PI / 2);
                    float dY = (float)Math.Sin(angle + Math.PI / 2);
                    Vector2 angleVector = new Vector2(dX, dY);
                    //BulletList.Add(BulletFactory.MakeBullet(gameContext, _bulletLifespan, angleVector * _bulletVelocity, 2f, new Vector2(this.Position.X, this.Position.Y)));
                    BulletFactory.MakeBullet(gameContext, gameContext.StateManager.CurrentState.World, "banana", Position);
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
    }
}
