using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace Marooned.Interpreter.Json
{
    public class Vector2JsonConverter : System.Text.Json.Serialization.JsonConverter<Vector2>
    {
        public override Vector2 Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            string jsonString = reader.GetString();
            string[] components = jsonString.Split(" ");
            return new Vector2(float.Parse(components[0]), float.Parse(components[1]));
        }

        public override void Write(
            Utf8JsonWriter writer,
            Vector2 vectorValue,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue($"{vectorValue.X} {vectorValue.Y}");
        }
    }

    public abstract class JsonConverter<T> where T : Delegate
    {
        public Dictionary<string, T> Registry;

        public JsonConverter()
        {
            Registry = new Dictionary<string, T>();
            foreach (var method in this.GetType().GetTypeInfo().GetMethods())
            {
                var attr = method.GetCustomAttribute<JsonPropertyAttribute>();
                if (attr != null)
                {
                    Type t = typeof(T);
                    AddConverter(attr.PropertyName, (T)Delegate.CreateDelegate(t, this, method));
                }
            }
        }

        public void AddConverter(string propertyName, T converterFunction)
        {
            Registry.Add(propertyName, converterFunction);
        }

        public JsonElement GetProperty(string propertyName, JsonElement jsonElement)
        {
            JsonElement propertyJson;
            if (jsonElement.TryGetProperty(propertyName, out propertyJson))
            {
                return propertyJson;
            }
            else
            {
                throw new Exception($"Could not find property '{propertyName}' inside: {jsonElement}");
            }
        }

        public bool TryGetProperty(string propertyName, JsonElement jsonElement, out JsonElement? resultJson)
        {
            JsonElement propertyJson;
            if (jsonElement.TryGetProperty(propertyName, out propertyJson))
            {
                resultJson = propertyJson;
                return true;
            }
            else
            {
                resultJson = null;
                return false;
            }
        }

        public Vector2 DeserializeVector2(JsonElement jsonElement)
        {
            var deserializeOptions = new JsonSerializerOptions();
            deserializeOptions.Converters.Add(new Vector2JsonConverter());
            return JsonSerializer.Deserialize<Vector2>(jsonElement, deserializeOptions);
        }

        public delegate T GetConvertFunction(string attributeName);
    }
}
