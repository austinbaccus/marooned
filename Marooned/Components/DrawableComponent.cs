using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned.Components
{
    public struct DrawableComponent
    {
        public Texture2D Texture;
        public Rectangle? Source;
        public Color Color;
    }
}
