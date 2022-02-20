using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Marooned.Factories;

namespace Marooned.Sprites.Enemies
{
    public class Grunt : Sprite
    {
        public Rectangle SourceRectangle = new Rectangle(0, 0, 16, 32);
        private List<double> _firePattern;
        private List<Tuple<Vector2, int>> _movePattern;
        private int _currentMovePattern = 0;
        private int _currentMovePatternTimeRemaining = 0;

        public List<Bullet> BulletList = new List<Bullet>(); // List of bullets
        private double _lastBulletTimestamp = 0;
        private float _bulletLifespan = 2f;
        private float _bulletVelocity = 3f;
        private float _bulletFireRate = 1000f;

        public int HitboxRadius = 10;

        public bool IsRemoved = false; // Grunt should be removed from list

        public Grunt(Texture2D texture, FiringPattern.Pattern firingPattern, MovementPattern.Pattern movementPattern) : base(texture)
        {
            _firePattern = FiringPattern.GetPattern(firingPattern);
            _movePattern = MovementPattern.GetPattern(movementPattern);
            _currentMovePatternTimeRemaining = _movePattern[0].Item2;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, SourceRectangle, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            //Move(gameTime);
            Shoot(gameTime);
        }

        private void Move(GameTime gameTime)
        {
            if (!IsRemoved)
            {
                // move the grunt
                Position += _movePattern[_currentMovePattern].Item1 * new Vector2(0.1f, 0.1f);

                // decrement time left for that instruction
                _currentMovePatternTimeRemaining--;

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

        public void Shoot(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - _lastBulletTimestamp > _bulletFireRate)
            {
                _lastBulletTimestamp = gameTime.TotalGameTime.TotalMilliseconds;
                BulletList.Add(BulletFactory.MakeBullet(_bulletLifespan, new Vector2(0, 1), new Vector2(1, _bulletVelocity), 2f, new Vector2(this.Position.X, this.Position.Y)));
            }
        }
    }
}
