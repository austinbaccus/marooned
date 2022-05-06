using DefaultEcs;
using Marooned.Actions;
using Marooned.Interpreter.Json.Patterns;
using Marooned.Patterns;
using Microsoft.Xna.Framework;
using System;
using System.Text.Json;

namespace Marooned.Interpreter.Json.Scripts
{
    public delegate IAction ConvertFunction(GameContext gameContext, World world, Entity actor, JsonElement actionJson);

    public class JsonActionConverter : JsonConverter<ConvertFunction>
    {
        public JsonPatternConverter _patternConverter = new JsonPatternConverter();

        [JsonProperty("move")]
        public IAction ConvertMove(GameContext gameContext, World world, Entity actor, JsonElement actionJson)
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
        public IAction ConvertScript(GameContext gameContext, World world, Entity actor, JsonElement actionJson)
        {
            JsonElement scriptJson = GetProperty("script", actionJson);
            Script script = ((JsonEntityScriptsInterpreter)gameContext.ScriptsInterpreter).CreateScriptFromElement(scriptJson, world, actor);
            return new ScriptAction(script, actor);
        }

        [JsonProperty("clear_enemy_bullets")]
        public IAction ConvertClearEnemyBullets(GameContext gameContext, World world, Entity actor, JsonElement actionJson)
        {
            return new ClearEnemyBulletsAction(gameContext.StateManager.CurrentState.World);
        }

        [JsonProperty("clear_player_bullets")]
        public IAction ConvertClearPlayerBullets(GameContext gameContext, World world, Entity actor, JsonElement actionJson)
        {
            return new ClearPlayerBulletsAction(gameContext.StateManager.CurrentState.World);
        }

        [JsonProperty("die")]
        public IAction ConvertDie(GameContext gameContext, World world, Entity actor, JsonElement actionJson)
        {
            return new DieAction(actor);
        }

        [JsonProperty("play_animation")]
        public IAction ConvertPlayAnimation(GameContext gameContext, World world, Entity actor, JsonElement actionJson)
        {
            JsonElement animationNameJson = GetProperty("animation", actionJson);
            string animationName = animationNameJson.GetString();
            if (animationName == null)
            {
                throw new Exception($"Invalid animation name: {animationNameJson}");
            }
            return new PlayAnimationAction(actor, animationName);
        }

        [JsonProperty("spawn")]
        public IAction ConvertSpawn(GameContext gameContext, World world, Entity actor, JsonElement actionJson)
        {
            JsonElement entityNameJson = GetProperty("entity", actionJson);
            string entityName = entityNameJson.GetString();

            JsonElement? positionJson;
            Vector2? position = null;
            if (TryGetProperty("position", actionJson, out positionJson))
            {
                position = DeserializeVector2(positionJson.Value);
            }

            JsonElement? countJson;
            int count = 1;
            if (TryGetProperty("count", actionJson, out countJson))
            {
                count = countJson.Value.GetInt32();
            }

            JsonElement? scriptJson;
            GetScriptFunction getScriptFunction = null;
            if (TryGetProperty("script", actionJson, out scriptJson))
            {
                JsonElement clonedScriptJson = scriptJson.Value.Clone();
                getScriptFunction = (thisWorld, thisEntity) =>
                {
                    return ((JsonEntityScriptsInterpreter)gameContext.ScriptsInterpreter).CreateScriptFromElement(clonedScriptJson, thisWorld, thisEntity);
                };
            }

            JsonElement? overrideScriptJson;
            bool overrideScript = true;
            if (TryGetProperty("override", actionJson, out overrideScriptJson))
            {
                overrideScript = overrideScriptJson.Value.GetBoolean();
            }

            JsonElement? rateJson;
            double rate = 0;
            if (TryGetProperty("rate", actionJson, out rateJson))
            {
                rate = rateJson.Value.GetDouble();
            }

            JsonElement? durationJson;
            double duration = 0;
            if (TryGetProperty("duration", actionJson, out durationJson))
            {
                duration = durationJson.Value.GetDouble();
            }

            return new SpawnAction(world, actor, entityName, position, count, rate, duration, getScriptFunction, overrideScript);
        }
    }
}
