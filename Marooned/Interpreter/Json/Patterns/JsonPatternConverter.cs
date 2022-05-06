using DefaultEcs;
using Marooned.Components;
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

                        Vector2 initialVelocity = DeserializeVector2(actionJson.GetProperty("velocity"));
                        actor.Set(new VelocityComponent
                        {
                            Value = initialVelocity,
                        });

                        return new LinearMovePattern();
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
