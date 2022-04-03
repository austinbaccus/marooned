using Marooned.Entities;
using Microsoft.Xna.Framework;

namespace Marooned.Components
{
    public abstract class BaseComponent
    {
        protected Entity _entity;

        public BaseComponent(Entity entity)
        {
            _entity = entity;
        }

        public abstract void Update(GameTime gameTime);
    }
}
