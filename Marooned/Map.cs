using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marooned
{
    public class Map
    {
        internal Tiles[,] tilemap;
        public Tiles[,] MapTiles
        {
            get { return tilemap; }
            set { tilemap = value; }
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

        public Map() { }

        //public void Generate(Tiles[,] map, int size)
        public virtual void Generate()
        {
            //for (int x = 0; x < map.GetLength(1); x++)
            //{
            //    for (int y = 0; y < map.GetLength(0); y++)
            //    {
            //        //string textureName = tilemap[y, x];
            //        //tiles.Add(new Tiles(textureName, new Rectangle(x * size, y * size, size, size)));

            //        width = (x + 1) * size;
            //        height = (y + 1) * size;
            //    }
            //}
        }
        public void Draw(SpriteBatch sb)
        {
            for (int x = 0; x < tilemap.GetLength(1); x++)
            {
                for (int y = 0; y < tilemap.GetLength(0); y++)
                {
                    tilemap[y, x].X = x * 16;
                    tilemap[y, x].Y = y * 16;
                    tilemap[y, x].Draw(sb);
                }
            }
        }
    }
}
