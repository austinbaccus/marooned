using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Marooned.Interpreter.Json.Animations
{
    public class AnimationInfoJson
    {
        [JsonPropertyName("frames")]
        public int[] Frames { get; set; }

        [JsonPropertyName("isLooping")]
        public bool IsLooping { get; set; } = false;

        [JsonPropertyName("frameDuration")]
        public float FrameDuration { get; set; }

        public Vector2 GetFrameLocation(int frameIndex, int regionWidth, int regionHeight, int textureWidth)
        {
            int rawX = regionWidth * Frames[frameIndex];
            int x = rawX % textureWidth;
            int y = regionHeight * (rawX / textureWidth);
            return new Vector2(x, y);
        }
    }
}
