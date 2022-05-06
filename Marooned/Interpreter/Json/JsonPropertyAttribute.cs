using System;

namespace Marooned.Interpreter.Json
{
    public class JsonPropertyAttribute : Attribute
    {
        public JsonPropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }
    }
}
