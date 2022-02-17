using System;
using System.Collections.Generic;
using System.Text;
using Marooned.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Marooned.Sprites
{
    public class Player : Sprite
    {
        public float Speed = 2f;
        public float SlowSpeed = 1f;
        public Rectangle[] SourceRectangle = new Rectangle[18];
        public float Timer = 0;
        public int Threshold = 250;

        public List<Bullet> BulletList = new List<Bullet>(); // List of bullets
        private double _lastBulletTimestamp = 0;
        private float _bulletLifespan = 2f;
        private float _bulletVelocity = 7f;
        private float _bulletFireRate = 100f;
		
        private byte currentAnimationIndex;
        private byte direction = 1;
        private byte[] animationLoop = { 0, 1, 2, 1 };

        public Player(Texture2D texture) : base(texture)
        {
            SourceRectangle[0] = new Rectangle(0, 0, 32, 32);     // 1  - idle down
            SourceRectangle[1] = new Rectangle(32, 0, 32, 32);    // 2  - fly down 1
            SourceRectangle[2] = new Rectangle(64, 0, 32, 32);    // 3  - fly down 2
            SourceRectangle[3] = new Rectangle(96, 0, 32, 32);    // 4  - fly down 3

            SourceRectangle[4] = new Rectangle(0, 32, 32, 32);    // 5  - idle right
            SourceRectangle[5] = new Rectangle(32, 32, 32, 32);   // 6  - fly right 1
            SourceRectangle[6] = new Rectangle(64, 32, 32, 32);   // 7  - fly right 2
            SourceRectangle[7] = new Rectangle(96, 32, 32, 32);   // 8  - fly right 3

            SourceRectangle[8] = new Rectangle(0, 64, 32, 32);    // 9  - idle up
            SourceRectangle[9] = new Rectangle(32, 64, 32, 32);   // 10 - fly up 1
            SourceRectangle[10] = new Rectangle(64, 64, 32, 32);  // 11 - fly up 2
            SourceRectangle[11] = new Rectangle(96, 64, 32, 32);  // 12 - fly up 3

            SourceRectangle[12] = new Rectangle(0, 96, 32, 32);   // 13 - idle left
            SourceRectangle[13] = new Rectangle(32, 96, 32, 32);  // 14 - fly left 1
            SourceRectangle[14] = new Rectangle(64, 96, 32, 32);  // 15 - fly left 2
            SourceRectangle[15] = new Rectangle(96, 96, 32, 32);  // 16 - fly left 3

            SourceRectangle[16] = new Rectangle(0, 128, 32, 32);  // 17 - idle flap
            SourceRectangle[17] = new Rectangle(32, 128, 32, 32); // 18 - idle monch

            currentAnimationIndex = 0;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, SourceRectangle[animationLoop[currentAnimationIndex] + direction], Color.White);
        }
		
		public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            Shoot(gameTime);
		}

        public void Shoot()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            { // Shoot right
                Bullet bullet = SpriteFactory.GenerateBullet(2f, new Vector2 (1, 0), new Vector2(2, 1), 5f, new Vector2(this.Position.X, this.Position.Y));
                BulletList.Add(bullet);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            { // Shoot right
                Bullet bullet = SpriteFactory.GenerateBullet(2f, new Vector2(-1, 0), new Vector2(2, 1), 5f, new Vector2(this.Position.X, this.Position.Y));
                BulletList.Add(bullet);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            { // Shoot right
                Bullet bullet = SpriteFactory.GenerateBullet(2f, new Vector2(0, 1), new Vector2(1, 2), 5f, new Vector2(this.Position.X, this.Position.Y));
                BulletList.Add(bullet);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            { // Shoot right
                Bullet bullet = SpriteFactory.GenerateBullet(2f, new Vector2(0, -1), new Vector2(1, 2), 5f, new Vector2(this.Position.X, this.Position.Y));
                BulletList.Add(bullet);
            }
        }

        public void Move(GameTime gameTime)
        {
            Move(gameTime);
            Shoot(gameTime);
        }

        private void Move(GameTime gameTime)
        {
            bool isShiftKeyPressed = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Position.Y -= isShiftKeyPressed ? SlowSpeed : Speed;
                direction = 9;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Position.Y += isShiftKeyPressed ? SlowSpeed : Speed;
                direction = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Position.X -= isShiftKeyPressed ? SlowSpeed : Speed;
                direction = 13;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Position.X += isShiftKeyPressed ? SlowSpeed : Speed;
                direction = 5;
            }

            // Update animation
            if (Timer > Threshold)
            {
                currentAnimationIndex = (byte)((currentAnimationIndex + 1) % 4);
                Timer = 0;
            }
            else
            {
                Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        public void Shoot(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - _lastBulletTimestamp > _bulletFireRate)
            {
                _lastBulletTimestamp = gameTime.TotalGameTime.TotalMilliseconds;

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                { // Shoot right
                    BulletList.Add(BulletFactory.MakeBullet(_bulletLifespan, new Vector2(1, 0), new Vector2(_bulletVelocity, 1), 2f, new Vector2(this.Position.X, this.Position.Y)));
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                { // Shoot left
                    BulletList.Add(BulletFactory.MakeBullet(_bulletLifespan, new Vector2(-1, 0), new Vector2(_bulletVelocity, 1), 2f, new Vector2(this.Position.X, this.Position.Y)));
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                { // Shoot down
                    BulletList.Add(BulletFactory.MakeBullet(_bulletLifespan, new Vector2(0, 1), new Vector2(1, _bulletVelocity), 2f, new Vector2(this.Position.X, this.Position.Y)));
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                { // Shoot up
                    BulletList.Add(BulletFactory.MakeBullet(_bulletLifespan, new Vector2(0, -1), new Vector2(1, _bulletVelocity), 2f, new Vector2(this.Position.X, this.Position.Y)));
                }
            }
        }
    }
}
