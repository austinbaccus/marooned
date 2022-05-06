using DefaultEcs;
using Marooned.Actions;
using Marooned.Interpreter.Json.Patterns;
using Marooned.Patterns;
using System.Text.Json;

namespace Marooned.Interpreter.Json.Scripts
{
    public delegate IAction ConvertFunction(GameContext gameContext, Entity actor, JsonElement actionJson);

    public class JsonActionConverter : JsonConverter<ConvertFunction>
    {
        JsonPatternConverter _patternConverter = new JsonPatternConverter();

        [JsonProperty("move")]
        public IAction ConvertMove(GameContext gameContext, Entity actor, JsonElement actionJson)
        {
            JsonElement patternJson = GetProperty("pattern", actionJson);
            MovePattern pattern = _patternConverter.ConvertMove(gameContext, actor, actionJson, patternJson);
            JsonElement durationJson = GetProperty("duration", actionJson);
            float duration = (float)durationJson.GetDouble();
            MoveAction moveAction = new MoveAction(pattern, actor, duration);
            return moveAction;
        }
    }
}
