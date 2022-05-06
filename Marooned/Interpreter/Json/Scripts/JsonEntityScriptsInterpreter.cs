using DefaultEcs;
using Marooned.Actions;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Marooned.Interpreter.Json.Scripts
{
    public class JsonEntityScriptsInterpreter : JsonInterpreter, IEntityScriptsInterpreter
    {
        public JsonActionConverter actionConverter = new JsonActionConverter();

        Dictionary<string, JsonDocument> _cache = new Dictionary<string, JsonDocument>();

        public JsonEntityScriptsInterpreter(GameContext gameContext) : base(gameContext)
        {
        }

        public override string Path { get; set; } = "Scripts";

        public Script CreateScriptFromElement(JsonElement jsonElement, GameContext gameContext, Entity entity)
        {
            Script script;
            if (jsonElement.ValueKind == JsonValueKind.String)
            {
                script = gameContext.ScriptsInterpreter.CreateScriptFrom(jsonElement.GetString(), entity);
            }
            else if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                script = ((JsonEntityScriptsInterpreter)gameContext.ScriptsInterpreter).CreateScriptFrom(jsonElement, entity);
            }
            else
            {
                throw new Exception($"Unknown type for script");
            }
            return script;
        }

        public Script CreateScriptFrom(string name, Entity entity)
        {
            if (_cache.ContainsKey(name))
            {
                return CreateScriptFrom(_cache[name].RootElement, entity);
            }

            using (JsonDocument jsonDoc = JsonDocument.Parse(GetFileContents(name)))
            {
                try
                {
                    return CreateScriptFrom(jsonDoc.RootElement, entity);
                }
                catch (Exception ex)
                {
                    throw new Exception($"({name}.json) {ex.Message}");
                }
            }
        }

        public Script CreateScriptFrom(JsonElement jsonArray, Entity entity)
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

                    IAction action = actionConverter.Registry[actionType](GameContext, entity, actionJson);
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
