using System.Collections.Generic;
using System.Linq;
using Marooned.Components;

namespace Marooned.Entities
{
    public class Entity
    {
        private List<BaseComponent> _components;

        public Entity(int id)
        {
            ID = id;
            _components = new List<BaseComponent>();
        }

        public Entity(int id, List<BaseComponent> components) : this(id)
        {
            _components = components;
        }

        public int ID { get; private set; }

        public bool SetComponent<T>(T component) where T : BaseComponent
        {
            if (_components.Contains(component)) return false;
            if (GetComponent<T>() != null) return false;
            _components.Add(component);
            return true;
        }

        public T GetComponent<T>() where T : BaseComponent
        {
            BaseComponent component = _components.First(c => c.GetType().Equals(typeof(T)));
            if (component == null) return null;
            return (T)component;
        }

        public BaseComponent RemoveComponent<T>() where T : BaseComponent
        {
            BaseComponent component = GetComponent<T>();
            if (component == null) return null;
            _components.Remove(component);
            return component;
        }
    }
}
