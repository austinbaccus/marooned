using Marooned.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Marooned.Sprites.Enemies
{
    public class Boss : Grunt
    {
        public Boss(Texture2D texture, Rectangle[] animSources, MovePattern.Pattern firingPattern, MovePattern.Pattern movementPattern, int health) : base(texture, animSources, firingPattern, movementPattern, health)
        {
        }
    }
}
