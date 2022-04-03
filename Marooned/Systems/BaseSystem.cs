using Marooned.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Marooned.Systems
{
    public class BaseSystem<T> where T: BaseComponent
    {
        private static List<T> _components = new List<T>();
        private static BaseSystem<T> instance = new BaseSystem<T>();
        //make the constructor private so that this class cannot be
        //instantiated
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

        //Get the only object available
        public static BaseSystem<T> GetInstance()
        {
            return instance;
        }
    }
}
