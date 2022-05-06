using DefaultEcs;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Marooned.Interpreter.Json.Scripts
{
    public class JsonActionRegistry
    {
        public delegate void AddActionToScript(GameContext gameContext, string name, Script script);

        //public static class JsonActionRegistry
        //{
        //    public static Dictionary<string, AddActionToScript> Registry = new Dictionary<string, AddActionToScript>()
        //    {
        //        ["move"] = (GameContext gameContext, string name, Entity entity, JsonElement componentJson) =>
        //        {
        //            JsonElement valueJson;
        //            if (componentJson.TryGetProperty("value", out valueJson))
        //            {
        //                gameContext.AnimationsInterpreter.CreateAnimationForEntity(entity, valueJson.GetString());
        //            }
        //            else
        //            {
        //                throw new Exception($"Could not find value for animation_group component in {name}.json");
        //            }
        //        },
        //    };
        //}
    }
}
