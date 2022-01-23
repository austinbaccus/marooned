using Marooned.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marooned
{
    public static class SpriteFactory
    {
        public static ContentManager content;

        public static T GenerateSprite<T>()
        {
            if (typeof(T) == typeof(Bullet))
            {
                var texture = content.Load<Texture2D>("Sprites/Banana");

                return new Bullet(texture, 2f, );
            } else
            {
                return default;
            }
        }
    }
}
