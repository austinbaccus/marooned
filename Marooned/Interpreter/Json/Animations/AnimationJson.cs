using MonoGame.Extended.Serialization;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Marooned.Interpreter.Json.Animations
{
    public class AnimationJsonReader : JsonContentTypeReader<AnimationJson> { }

    public class AnimationJson
    {
        [JsonPropertyName("textureAtlas")]
        public TextureAtlas TextureAtlas { get; set; }
        [JsonPropertyName("cycles")]
        public Dictionary<string, AnimationInfoJson> Cycles { get; set; }
    }
}
