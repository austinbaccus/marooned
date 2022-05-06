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

        [JsonProperty("health")]
        public void ConvertHealth(GameContext gameContext, Entity entity, JsonElement componentJson)
        {
            JsonElement valueJson = GetProperty("value", componentJson);
            entity.Set(new HealthComponent
            {
                Health = valueJson.GetInt32(),
            });
        }

        [JsonProperty("hitbox")]
        public void ConvertHitbox(GameContext gameContext, Entity entity, JsonElement componentJson)
        {
            JsonElement radiusJson = GetProperty("radius", componentJson);
            entity.Set(new HitboxComponent
            {
                HitboxRadius = radiusJson.GetInt32(),
            });
        }

        [JsonProperty("script")]
        public void ConvertScript(GameContext gameContext, Entity entity, JsonElement componentJson)
        {
            var scriptJson = componentJson.GetProperty("script");
            Script script = ((JsonEntityScriptsInterpreter)gameContext.ScriptsInterpreter).CreateScriptFromElement(scriptJson, gameContext, entity);
            entity.Set(new ScriptComponent
            {
                Script = script,
                TimeElapsed = TimeSpan.Zero,
            });
        }
    }
}
