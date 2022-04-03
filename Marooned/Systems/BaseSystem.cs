using Marooned.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Marooned.Systems
{
    public class BaseSystem<T> where T: BaseComponent
    {
        private List<T> _components = new List<T>();
        public void Update(GameTime gameTime)
        {
            foreach(T component in _components)
            {
                component.Update(gameTime);
            }
        }

        // add components

        public void AddComponent(T component)
        {
            _components.Add(component);
        }
    }
}
