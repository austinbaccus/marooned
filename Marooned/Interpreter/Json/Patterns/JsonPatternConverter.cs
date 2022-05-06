using DefaultEcs;
using Marooned.Patterns;
using Microsoft.Xna.Framework;
using System;
using System.Text.Json;

namespace Marooned.Interpreter.Json.Patterns
{
    public delegate MovePattern ConvertFunction(GameContext gameContext, Entity actor, JsonElement actionJson, JsonElement patternJson);

    public class JsonPatternConverter : JsonConverter<ConvertFunction>
    {
        [JsonProperty("move")]
        public MovePattern ConvertMove(GameContext gameContext, Entity actor, JsonElement actionJson, JsonElement patternJson)
        {
            string patternName = patternJson.GetString();
            if (patternName != null)
            {
                switch (patternName)
                {
                    case "linear":
                        return new LinearMovePattern();
                    case "circle":                        
                        int degrees = actionJson.GetProperty("degrees").GetInt32();
                        Vector2 centerOffset = DeserializeVector2(actionJson.GetProperty("center_offset"));
                        return new CircleMovePattern(degrees, centerOffset);
                    default:
                        throw new Exception($"Pattern {patternName} does not exist");
                }
            }
            else
            {
                throw new Exception($"Could not find pattern in {patternJson}");
            }
        }
    }
}
