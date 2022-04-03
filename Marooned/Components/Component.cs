using Marooned.Entities;
using Microsoft.Xna.Framework;

namespace Marooned.Components
{
    public abstract class Component
    {
        public Entity Entity { get; private set; }

        public abstract void Update(GameTime gameTime);
    }
}
