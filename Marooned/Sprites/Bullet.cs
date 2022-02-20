using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned.Sprites
{
    public class Bullet : AnimatedSprite
    {
        private float _timer;
        private Vector2 _direction;
        private Vector2 _linearVelocity;
        private float _damage;
#if DEBUG
        public Sprite HitboxSprite;
#endif

        public bool IsRemoved = false; // Bullet should be removed from list
        public Bullet(Texture2D texture, Rectangle[] animSources, float lifeSpan, Vector2 direction, Vector2 linearVelocity, float damage, Vector2 origin) : base(texture, animSources)
        {
            _timer = lifeSpan; // Life span of bullet
            _direction = direction; // Direction of bullet
            _linearVelocity = linearVelocity; // Speed of bullet
            _damage = damage; // Amount of damage
            Position = origin; // Starting position of bullet

            CurrentAnimation.Play();
        }

        public Hitbox Hitbox { get; set; }

#if DEBUG
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            HitboxSprite.Draw(gameTime, spriteBatch);
        }
#endif

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            CurrentAnimation.Update(gameTime);
#if DEBUG
            HitboxSprite.Destination = new Rectangle(
                (int)(Position.X + Hitbox.Offset.X),
                (int)(Position.Y + Hitbox.Offset.Y),
                Hitbox.Radius * 2,
                Hitbox.Radius * 2
            );
#endif
        }

        public void Move(GameTime gameTime)
        {
            _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer <= 0)
            {
                IsRemoved = true;
            }

            Position += _direction * _linearVelocity;
        }
    }
}
