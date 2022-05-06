using DefaultEcs;
using Marooned.Actions;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Marooned.Interpreter.Json.Scripts
{
    public class JsonEntityScriptsInterpreter : JsonInterpreter, IEntityScriptsInterpreter
    {
        Dictionary<string, JsonElement> _cache = new Dictionary<string, JsonElement>();

        public JsonActionConverter actionConverter = new JsonActionConverter();

        public JsonEntityScriptsInterpreter(GameContext gameContext) : base(gameContext)
        {
        }

        public override string Path { get; set; } = "Scripts";

        public Script CreateScriptFromElement(JsonElement jsonElement, World world, Entity entity)
        {
            Script script;
            if (jsonElement.ValueKind == JsonValueKind.String)
            {
                script = CreateScriptFrom(jsonElement.GetString(), world, entity);
            }
            else if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                script = CreateScriptFrom(jsonElement, world, entity);
            }
            else
            {
                throw new Exception($"Unknown type for script");
            }
            return script;
        }

        public Script CreateScriptFrom(string name, World world, Entity entity)
        {
            if (_cache.ContainsKey(name)) return CreateScriptFrom(_cache[name], world, entity);

            using (JsonDocument jsonDoc = JsonDocument.Parse(GetFileContents(name)))
            {
                _cache.Add(name, jsonDoc.RootElement.Clone());
                try
                {
                    return CreateScriptFrom(jsonDoc.RootElement, world, entity);
                }
                catch (Exception ex)
                {
                    throw new Exception($"({name}.json) {ex.Message}");
                }
            }
        }

        public Script CreateScriptFrom(JsonElement jsonArray, World world, Entity entity)
        {
            if (jsonArray.ValueKind != JsonValueKind.Array)
            {
                throw new Exception("Json script is not an array");
            }
            Script script = new Script();
            var actions = jsonArray.EnumerateArray().GetEnumerator();
            foreach (var actionJson in actions)
            {
                var actionType = actionJson.GetProperty("action").GetString();
                if (actionConverter.Registry.ContainsKey(actionType))
                {
                    JsonElement timeJson;
                    double time = 0;
                    if (actionJson.TryGetProperty("time", out timeJson))
                    {
                        time = timeJson.GetDouble();
                    }

                    IAction action = actionConverter.Registry[actionType](GameContext, world, entity, actionJson);
                    script.Enqueue(action, time);
                }
                else
                {
                    throw new Exception($"Could not find converter for action '{actionType}' in {jsonArray}");
                }
            }
            return script;
        }
    }
}
