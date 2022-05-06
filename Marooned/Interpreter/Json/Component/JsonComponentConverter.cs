using DefaultEcs;
using Marooned.Components;
using Marooned.Interpreter.Json.Scripts;
using System;
using System.Text.Json;

namespace Marooned.Interpreter.Json.Component
{
    public delegate void ConvertFunction(GameContext gameContext, Entity entity, JsonElement componentJson);

    public class JsonComponentConverter : JsonConverter<ConvertFunction>
    {
        [JsonProperty("animation")]
        public void ConvertAnimation(GameContext gameContext, Entity entity, JsonElement componentJson)
        {
            JsonElement valueJson = GetProperty("value", componentJson);
            gameContext.AnimationsInterpreter.CreateAnimationForEntity(entity, valueJson.GetString());
        }

        [JsonProperty("script")]
        public void ConvertScript(GameContext gameContext, Entity entity, JsonElement componentJson)
        {
            Script script;
            var scriptJson = componentJson.GetProperty("script");
            if (scriptJson.ValueKind == JsonValueKind.String)
            {
                script = gameContext.ScriptsInterpreter.CreateScriptFrom(scriptJson.GetString(), entity);
            }
            else if (scriptJson.ValueKind == JsonValueKind.Array)
            {
                script = ((JsonEntityScriptsInterpreter)gameContext.ScriptsInterpreter).CreateScriptFrom(scriptJson, entity);
            }
            else
            {
                throw new Exception($"Unknown type for script");
            }

            entity.Set(new ScriptComponent
            {
                Script = script,
                TimeElapsed = TimeSpan.Zero,
            });
        }
    }
}
