using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;

namespace Marooned.Sprites
{
    public class Bullet : ComponentOld
    {
        private float _timer;
        private Vector2 _linearVelocity;
        private float _damage;
        private MonoGame.Extended.Sprites.AnimatedSprite _sprite;
#if DEBUG
        public Sprite HitboxSprite;
#endif

        public Vector2 Position;
        public bool IsRemoved = false; // Bullet should be removed from list

        public Bullet(MonoGame.Extended.Sprites.AnimatedSprite animatedSprite, float lifeSpan, Vector2 linearVelocity, float damage, Vector2 origin)
        {
            _timer = lifeSpan; // Life span of bullet
            _linearVelocity = linearVelocity; // Speed of bullet
            _damage = damage; // Amount of damage
            Position = origin; // Starting position of bullet
            _sprite = animatedSprite;
        }

        // The origin of all sprites should be the center, rather than top-left, so that position calculation is much more simple and does
        // not have to take into account the sprite width and height.
        public virtual Vector2 Origin
        {
            get { return new Vector2(_sprite.TextureRegion.Width / 2f, _sprite.TextureRegion.Height / 2f); }
        }

        public Hitbox Hitbox { get; set; }

#if DEBUG
        public override void Draw(GameContext gameContext)
        {
            gameContext.SpriteBatch.Draw(
                _sprite,
                Position
            );
            HitboxSprite.Draw(gameContext);
        }
#endif

        public override void Update(GameContext gameContext)
        {
            Move(gameContext);
            _sprite.Update((float)gameContext.GameTime.ElapsedGameTime.TotalSeconds);
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
