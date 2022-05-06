using DefaultEcs;
using Marooned.Interpreter.Json.Component;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Marooned.Interpreter.Json.Entities
{
    public class JsonEntitiesInterpreter : JsonInterpreter, IEntitiesInterpreter
    {
        JsonComponentConverter _componentConverter = new JsonComponentConverter();
        Dictionary<string, JsonDocument> _cache = new Dictionary<string, JsonDocument>();

        public JsonEntitiesInterpreter(GameContext gameContext) : base(gameContext)
        {
        }

        public override string Path { get; set; } = "Entities";

        public Entity CreateEntityFrom(World world, string name)
        {
            if (_cache.ContainsKey(name))
            {
                return CreateEntityFrom(world, _cache[name].RootElement);
            }

            using (JsonDocument jsonDoc = JsonDocument.Parse(GetFileContents(name)))
            {
                try
                {
                    return CreateEntityFrom(world, jsonDoc.RootElement);
                }
                catch (Exception ex)
                {
                    throw new Exception($"({name}.json) {ex.Message}");
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
                    _componentConverter.Registry[type](GameContext, entity, component);
                }
            }

            _componentConverter.Registry["script"](GameContext, entity, rootElement);

            return entity;
        }
    }
}
