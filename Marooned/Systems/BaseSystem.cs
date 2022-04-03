using Marooned.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Marooned.Systems
{
    public class BaseSystem<T> where T: BaseComponent
    {
        private static List<T> _components = new List<T>();
        private static T instance;
        private BaseSystem() { }

        // Update each component
        public static void Update(GameTime gameTime)
        {
            foreach(T component in _components)
            {
                component.Update(gameTime);
            }
        }

        // Add components

        public static void AddComponent(T component)
        {
            _components.Add(component);
        }

        public static T GetInstance()
        {
            return instance;
        }
    }
}
