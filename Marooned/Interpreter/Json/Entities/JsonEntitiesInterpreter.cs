using DefaultEcs;
using Marooned.Interpreter.Json.Component;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Marooned.Interpreter.Json.Entities
{
    public class JsonEntitiesInterpreter : JsonInterpreter, IEntitiesInterpreter
    {
        Dictionary<string, JsonElement> _cache = new Dictionary<string, JsonElement>();

        public JsonComponentConverter _componentConverter = new JsonComponentConverter();

        public JsonEntitiesInterpreter(GameContext gameContext) : base(gameContext)
        {
        }

        public override string Path { get; set; } = "Entities";

        public Entity CreateEntityFrom(World world, string name)
        {
            if (_cache.ContainsKey(name)) return CreateEntityFrom(world, _cache[name]);

            using (JsonDocument jsonDoc = JsonDocument.Parse(GetFileContents(name)))
            {
                _cache.Add(name, jsonDoc.RootElement.Clone());
                try
                {
                    return CreateEntityFrom(world, jsonDoc.RootElement);
                }
                catch (Exception ex)
                {
                    throw new Exception($"({name}.json) {ex.Message}\n{ex.StackTrace}");
                }
            }
        }

        public Entity CreateEntityFrom(World world, JsonElement rootElement)
        {
            Entity entity = world.CreateEntity();
            
            var components = rootElement.GetProperty("components").EnumerateArray();
            while (components.MoveNext())
            {
                var component = components.Current;
                var type = component.GetProperty("type").GetString();
                if (_componentConverter.Registry.ContainsKey(type))
                {
                    // Call on the json component registry to create
                    // and set components to the entities
                    _componentConverter.Registry[type](GameContext, world, entity, component);
                }
            }

            _componentConverter.Registry["script"](GameContext, world, entity, rootElement);

            return entity;
        }
    }
}
