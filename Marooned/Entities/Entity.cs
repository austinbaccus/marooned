using System.Collections.Generic;
using System.Linq;
using Marooned.Components;

namespace Marooned.Entities
{
    public class Entity
    {
        private List<Component> _components;

        public Entity(int id)
        {
            ID = id;
        }

        public int ID { get; private set; }

        public bool SetComponent<T>(T component) where T : Component
        {
            if (_components.Contains(component)) return false;
            if (GetComponent<T>() != null) return false;
            _components.Add(component);
            return true;
        }

        public T GetComponent<T>() where T : Component
        {
            Component component = _components.First(c => c.GetType().Equals(typeof(T)));
            if (component == null) return null;
            return (T)component;
        }

        public Component RemoveComponent<T>() where T : Component
        {
            Component component = GetComponent<T>();
            if (component == null) return null;
            _components.Remove(component);
            return component;
        }
    }
}
