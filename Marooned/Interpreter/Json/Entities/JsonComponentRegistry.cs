using DefaultEcs;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Marooned.Interpreter.Json.Entities
{
    public delegate void SetEntityComponentFromJson(GameContext gameContext, string name, Entity entity, JsonElement componentJson);

    public static class JsonComponentRegistry
    {
        public static Dictionary<string, SetEntityComponentFromJson> Registry = new Dictionary<string, SetEntityComponentFromJson>()
        {
            ["animation"] = (GameContext gameContext, string name, Entity entity, JsonElement componentJson) =>
            {
                JsonElement valueJson;
                if (componentJson.TryGetProperty("value", out valueJson))
                {
                    gameContext.AnimationsInterpreter.CreateAnimationForEntity(entity, valueJson.GetString());
                }
                else
                {
                    throw new Exception($"Could not find value for animation_group component in {name}.json");
                }
            },
        };
    }
}
