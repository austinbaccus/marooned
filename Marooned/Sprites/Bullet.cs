using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marooned.Sprites
{
    public class Bullet : Sprite
    {
        private float _timer;
        private Vector2 _direction;
        private Vector2 _linearVelocity;
        private float _damage;
        //private Vector2 _position;

        public bool IsRemoved = false; // Bullet should be removed from list
        
        public Bullet(Texture2D texture, float lifeSpan, Vector2 direction, Vector2 linearVelocity, float damage, Vector2 origin)
          : base(texture)
        {
            _timer = lifeSpan; // Life span of bullet
            _direction = direction; // Direction of bullet
            _linearVelocity = linearVelocity; // Speed of bullet
            _damage = damage; // Amount of damage
            Position = origin; // Starting position of bullet
        }

        public override void Update(GameTime gameTime)
        {
            _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer <= 0)
                IsRemoved = true;

            Position += _direction * _linearVelocity;
        }
    }
}