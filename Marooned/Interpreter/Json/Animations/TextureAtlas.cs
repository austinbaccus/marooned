using System.Text.Json.Serialization;

namespace Marooned.Interpreter.Json.Animations
{
    public struct TextureAtlas
    {
        [JsonPropertyName("texture")]
        public string Texture { get; set; }

        [JsonPropertyName("regionWidth")]
        public int RegionWidth { get; set; }

        [JsonPropertyName("regionHeight")]
        public int RegionHeight { get; set; }
    }
}
