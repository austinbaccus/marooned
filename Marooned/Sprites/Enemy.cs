﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;



namespace Marooned.Sprites
{
    public class Enemy : Sprite
    {
        public float Speed = 1f;
        public float SlowSpeed = 1f;
        public Rectangle[] SourceRectangle = new Rectangle[18];
        public float Timer = 0;
        public int Threshold = 250;

        private byte currentAnimationIndex;
        private byte direction = 1;
        private byte[] animationLoop = { 0, 1, 2, 1 };
        private byte[] movementLoop = { 0, 1, 2, 3 };

        public Enemy(Texture2D texture) : base(texture)
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

        }

        private void Move(GameTime gameTime)
        {
            //bool isShiftKeyPressed = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
            //if (Keyboard.GetState().IsKeyDown(Keys.I))
            //{
            //    Position.Y -= isShiftKeyPressed ? SlowSpeed : Speed;
            //    direction = 9;
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.K))
            //{
            //    Position.Y += isShiftKeyPressed ? SlowSpeed : Speed;
            //    direction = 1;
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.J))
            //{
            //    Position.X -= isShiftKeyPressed ? SlowSpeed : Speed;
            //    direction = 13;
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.L))
            //{
            //    Position.X += isShiftKeyPressed ? SlowSpeed : Speed;
            //    direction = 5;
            //}

            //// Update animation
            //if (Timer > Threshold)
            //{
            //    //  0 | 3
            //    // -1 | 2
            //    // -1.5 | 1.5
            //    currentAnimationIndex = (byte)((currentAnimationIndex + 1) % 4);
            //    byte cur = animationLoop[currentAnimationIndex];
            //    float adjusted = cur - 1f;
            //    Position.Y += 3 * adjusted;
            //    Timer = 0;
            //}
            //else
            //{
            //    Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //}
            //Random rand = new Random();

            //while (Keyboard.GetState().IsKeyDown(Keys.Delete) == false) 
            //{
            //    String rNum = rand.Next(0, 3).ToString();
            //    bool isShiftKeyPressed = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
            //    if ( rNum == "0")
            //    {
            //        Position.Y -= isShiftKeyPressed ? SlowSpeed : Speed;
            //        direction = 9;
            //    }
            //    if (rNum == "1")
            //    {
            //        Position.Y += isShiftKeyPressed ? SlowSpeed : Speed;
            //        direction = 1;
            //    }
            //    if (rNum == "2")
            //    {
            //        Position.X -= isShiftKeyPressed ? SlowSpeed : Speed;
            //        direction = 13;
            //    }
            //    if (rNum == "3")
            //    {
            //        Position.X += isShiftKeyPressed ? SlowSpeed : Speed;
            //        direction = 5;
            //    }
            //    // Update animation
            //    if (Timer > Threshold)
            //    {
            //        currentAnimationIndex = (byte)((currentAnimationIndex + 1) % 4);
            //        Timer = 0;
            //    }
            //    else
            //    {
            //        Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //    }
            //}           
            Random rand = new Random();
            String rNum = rand.Next(0, 3).ToString();
            bool isShiftKeyPressed = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
            if (rNum == "0")
            {
                Position.Y -= isShiftKeyPressed ? SlowSpeed : Speed;
                direction = 1;
            }
            if (rNum == "1")
            {
                Position.Y += isShiftKeyPressed ? SlowSpeed : Speed;
                direction = 0;
            }
            if (rNum == "2")
            {
                Position.X -= isShiftKeyPressed ? SlowSpeed : Speed;
                direction = 0;
            }
            if (rNum == "3")
            {
                Position.X += isShiftKeyPressed ? SlowSpeed : Speed;
                direction = 1;
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
    }
}