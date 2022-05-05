using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned.Sprites
{
    public class Bullet : AnimatedSprite
    {
        private float _timer;
        private Vector2 _linearVelocity;
        private float _damage;
#if DEBUG
        public Sprite HitboxSprite;
#endif

        public bool IsRemoved = false; // Bullet should be removed from list
        public Bullet(Texture2D texture, Rectangle[] animSources, float lifeSpan, Vector2 linearVelocity, float damage, Vector2 origin) : base(texture, animSources)
        {
            _timer = lifeSpan; // Life span of bullet
            _linearVelocity = linearVelocity; // Speed of bullet
            _damage = damage; // Amount of damage
            Position = origin; // Starting position of bullet

            CurrentAnimation.Play();
        }

        public Hitbox Hitbox { get; set; }

#if DEBUG
        public override void Draw(GameContext gameContext)
        {
            base.Draw(gameContext);
            HitboxSprite.Draw(gameContext);
        }
#endif

        public override void Update(GameContext gameContext)
        {
            Move(gameContext);
            CurrentAnimation.Update(gameContext);
#if DEBUG
            HitboxSprite.Destination = new Rectangle(
                (int)(Position.X + Hitbox.Offset.X),
                (int)(Position.Y + Hitbox.Offset.Y),
                Hitbox.Radius * 2,
                Hitbox.Radius * 2
            );
#endif
        }

        public void Move(GameContext gameContext)
        {
            _timer -= (float)gameContext.GameTime.ElapsedGameTime.TotalSeconds;

            if (_timer <= 0)
            {
                IsRemoved = true;
            }

            Position += (_linearVelocity * (float)gameContext.GameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
