using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marooned
{
    public class Tiles
    {
        protected Texture2D texture;

        private bool canTraverse;
        public bool CanTraverse
        {
            get { return canTraverse; }
            set { canTraverse = value; }
        }

        private int width, height;
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }

        private int x, y;
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
            protected set { rectangle = value; }
        }

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        public Tiles(string textureName, Rectangle newRectangle, int width, int height, bool isTraversible)
        {
            texture = Content.Load<Texture2D>(textureName);
            this.Rectangle = newRectangle;
            this.width = width;
            this.height = height;
            this.CanTraverse = isTraversible;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(x, y), rectangle, Color.White);
        }
    }
}
