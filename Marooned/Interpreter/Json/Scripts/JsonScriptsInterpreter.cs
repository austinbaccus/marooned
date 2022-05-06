using System;
using System.Text.Json;

namespace Marooned.Interpreter.Json.Scripts
{
    public class JsonScriptsInterpreter : JsonInterpreter, IScriptsInterpreter
    {
        public JsonScriptsInterpreter(GameContext gameContext) : base(gameContext)
        {
        }

        public override string Path { get; set; } = "Scripts";

        public Script CreateScriptFrom(string name)
        {
            //using (JsonDocument jsonDoc = JsonDocument.Parse(GetFileContents(name)))
            //{
            //    var components = jsonDoc.RootElement.GetProperty("components").EnumerateArray();
            //    while (components.MoveNext())
            //    {
            //        var component = components.Current;
            //        var type = component.GetProperty("type").GetString();
            //        if (JsonComponentRegistry.Registry.ContainsKey(type))
            //        {
            //            // Call on the json component registry to create
            //            // and set components to the entities
            //            JsonComponentRegistry.Registry[type](GameContext, name, entity, component);
            //        }
            //    }
            //    // Script code
            //    //var scriptJson = jsonDoc.RootElement.GetProperty("script");
            //    //if (scriptJson.ValueKind == JsonValueKind.String)
            //    //{
            //    //    string scriptName = scriptJson.GetString();
            //    //    Script script = GameContext.ScriptsInterpreter.CreateScriptFrom(scriptName);

            //    //}
            //}
            throw new NotImplementedException();
        }
    }
}
