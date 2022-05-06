using DefaultEcs;
using Marooned.Components;
using Marooned.Interpreter.Json.Scripts;
using Marooned.Levels;
using System;
using System.Text.Json;

namespace Marooned.Interpreter.Json.Level
{
    public class JsonLevelsInterpreter : JsonInterpreter, ILevelsInterpreter
    {
        public JsonLevelsInterpreter(GameContext gameContext) : base(gameContext)
        {
        }

        public override string Path { get; set; } = "Levels";

        public Entity GetLevelEntity(string name, World world)
        {
            using (JsonDocument jsonDoc = JsonDocument.Parse(GetFileContents(name)))
            {
                try
                {
                    return GetLevelEntity(jsonDoc.RootElement, world);
                }
                catch (Exception ex)
                {
                    throw new Exception($"({name}.json) {ex.Message}");
                }
            }
        }

        public Entity GetLevelEntity(JsonElement jsonElement, World world)
        {
            Entity levelEntity = world.CreateEntity();

            JsonElement titleJson = jsonElement.GetProperty("title");
            string title = titleJson.GetString();

            JsonElement mapPathJson = jsonElement.GetProperty("map");
            string mapPath = mapPathJson.GetString();

            JsonElement scriptJson = jsonElement.GetProperty("script");
            Script script = ((JsonEntityScriptsInterpreter)GameContext.ScriptsInterpreter).CreateScriptFromElement(scriptJson, world, levelEntity);

            levelEntity.Set(new LevelInfo()
            {
                Title = title,
                MapPath = mapPath,
            });

            levelEntity.Set(new ScriptComponent
            {
                Script = script,
                TimeElapsed = TimeSpan.Zero,
            });

            return levelEntity;
        }
    }
}
