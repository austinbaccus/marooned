using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marooned.Sprites.Enemies
{
    public class MovementPattern
    {
        static List<Tuple<Vector2, int>> down_left = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(new Vector2(0,1),1000),
            new Tuple<Vector2, int>(new Vector2(-1,0),1000)
        };
        static List<Tuple<Vector2, int>> down_right = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(new Vector2(0,1),1000),
            new Tuple<Vector2, int>(new Vector2(1,0),1000)
        };

        public enum Pattern
        {
            down_left,
            down_right
        }

        public static List<Tuple<Vector2, int>> GetPattern(Pattern pattern)
        {
            switch (pattern)
            {
                case (Pattern.down_left): { return down_left; }
                case (Pattern.down_right): { return down_right; }
                default: return down_left;
            }
        }
    }
}
