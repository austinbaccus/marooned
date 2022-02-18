using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned.Sprites
{
    public class Bullet : Sprite
    {
        private float _timer;
        private Vector2 _direction;
        private Vector2 _linearVelocity;
        private float _damage;

        public Rectangle[] SourceRectangle = new Rectangle[4];
        private byte[] animationLoop = { 0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3 };
        //private int[] animationLoop = { 0, 1, 2, 3 };
        private int currentAnimationIndex;

        public bool IsRemoved = false; // Bullet should be removed from list

        public Bullet(Texture2D texture, float lifeSpan, Vector2 direction, Vector2 linearVelocity, float damage, Vector2 origin) : base(texture)
        {
            _timer = lifeSpan; // Life span of bullet
            _direction = direction; // Direction of bullet
            _linearVelocity = linearVelocity; // Speed of bullet
            _damage = damage; // Amount of damage
            Position = origin; // Starting position of bullet

            SourceRectangle[0] = new Rectangle(18, 18, 16, 20);
            SourceRectangle[1] = new Rectangle(33, 19, 20, 18);
            SourceRectangle[2] = new Rectangle(54, 19, 16, 19);
            SourceRectangle[3] = new Rectangle(70, 20, 19, 17);

            currentAnimationIndex = 0;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, SourceRectangle[animationLoop[currentAnimationIndex]], Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentAnimationIndex = (byte)((currentAnimationIndex + 1) % 12);
            //currentAnimationIndex = (((currentAnimationIndex + 1) % 12) /4);

            if (_timer <= 0)
            {
                IsRemoved = true;
            }

            Position += _direction * _linearVelocity;
        }
    }
}