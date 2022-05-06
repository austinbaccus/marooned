using DefaultEcs;
using Marooned.Actions;
using Marooned.Interpreter.Json.Patterns;
using Marooned.Patterns;
using Microsoft.Xna.Framework;
using System;
using System.Text.Json;

namespace Marooned.Interpreter.Json.Scripts
{
    public delegate IAction ConvertFunction(GameContext gameContext, Entity actor, JsonElement actionJson);

    public class JsonActionConverter : JsonConverter<ConvertFunction>
    {
        public JsonPatternConverter _patternConverter = new JsonPatternConverter();

        [JsonProperty("move")]
        public IAction ConvertMove(GameContext gameContext, Entity actor, JsonElement actionJson)
        {
            JsonElement patternJson = GetProperty("pattern", actionJson);
            MovePattern pattern = _patternConverter.ConvertMove(gameContext, actor, actionJson, patternJson);
            JsonElement durationJson = GetProperty("duration", actionJson);
            TimeSpan duration = TimeSpan.FromSeconds(durationJson.GetDouble());

            MoveAction moveAction;
            switch (pattern)
            {
                case LinearMovePattern linearMovePattern:
                    Vector2 velocity = DeserializeVector2(actionJson.GetProperty("velocity"));
                    moveAction = new LinearMoveAction(linearMovePattern, actor, duration, velocity);
                    break;
                default:
                    moveAction = new MoveAction(pattern, actor, duration);
                    break;
            }

            return moveAction;
        }

        [JsonProperty("script")]
        public IAction ConvertScript(GameContext gameContext, Entity actor, JsonElement actionJson)
        {
            JsonElement scriptJson = GetProperty("script", actionJson);
            Script script = ((JsonEntityScriptsInterpreter)gameContext.ScriptsInterpreter).CreateScriptFromElement(scriptJson, gameContext, actor);
            return new ScriptAction(script, actor);
        }

        [JsonProperty("clear_enemy_bullets")]
        public IAction ConvertClearEnemyBullets(GameContext gameContext, Entity actor, JsonElement actionJson)
        {
            return new ClearEnemyBulletsAction(gameContext.StateManager.CurrentState.World);
        }

        [JsonProperty("clear_player_bullets")]
        public IAction ConvertClearPlayerBullets(GameContext gameContext, Entity actor, JsonElement actionJson)
        {
            return new ClearPlayerBulletsAction(gameContext.StateManager.CurrentState.World);
        }

        [JsonProperty("play_animation")]
        public IAction ConvertPlayAnimation(GameContext gameContext, Entity actor, JsonElement actionJson)
        {
            JsonElement animationNameJson = GetProperty("animation", actionJson);
            string animationName = animationNameJson.GetString();
            if (animationName == null)
            {
                throw new Exception($"Invalid animation name: {animationNameJson}");
            }
            return new PlayAnimationAction(actor, animationName);
        }
    }
}
