using DefaultEcs;
using Marooned.Components;
using Marooned.Interpreter.Json.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Text.Json;

namespace Marooned.Interpreter.Json.Component
{
    public delegate void ConvertFunction(GameContext gameContext, World world, Entity entity, JsonElement componentJson);

    public class JsonComponentConverter : JsonConverter<ConvertFunction>
    {
        [JsonProperty("animation")]
        public void ConvertAnimation(GameContext gameContext, World world, Entity entity, JsonElement componentJson)
        {
            JsonElement valueJson = GetProperty("value", componentJson);
            gameContext.AnimationsInterpreter.CreateAnimationForEntity(entity, valueJson.GetString());
        }

        [JsonProperty("health")]
        public void ConvertHealth(GameContext gameContext, World world, Entity entity, JsonElement componentJson)
        {
            JsonElement valueJson = GetProperty("value", componentJson);
            entity.Set(new HealthComponent
            {
                Health = valueJson.GetInt32(),
            });
        }

        [JsonProperty("hitbox")]
        public void ConvertHitbox(GameContext gameContext, World world, Entity entity, JsonElement componentJson)
        {
            JsonElement radiusJson = GetProperty("radius", componentJson);
            entity.Set(new HitboxComponent
            {
                HitboxRadius = radiusJson.GetInt32(),
            });
        }

        [JsonProperty("script")]
        public void ConvertScript(GameContext gameContext, World world, Entity entity, JsonElement componentJson)
        {
            JsonElement scriptJson = componentJson.GetProperty("script");
            Script script = ((JsonEntityScriptsInterpreter)gameContext.ScriptsInterpreter).CreateScriptFromElement(scriptJson, world, entity);
            entity.Set(new ScriptComponent
            {
                Script = script,
                TimeElapsed = TimeSpan.Zero,
            });
        }

        [JsonProperty("transform")]
        public void ConvertTransform(GameContext gameContext, World world, Entity entity, JsonElement componentJson)
        {
            JsonElement positionJson;
            Vector2 position = Vector2.Zero;
            if (componentJson.TryGetProperty("position", out positionJson))
            {
                position = DeserializeVector2(positionJson);
            }
            entity.Set(new TransformComponent
            {
                Position = position
            });
        }
    }
}
