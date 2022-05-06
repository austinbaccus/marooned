using Microsoft.Xna.Framework;

namespace Marooned.Animation
{
    public struct AnimationInfo
    {
        public string Name { get; set; }
        public Vector2[] FrameLocations { get; set; }
        public int CurrentFrameIndex { get; set; }
        public int FrameCount { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public bool IsLooping { get; set; }
        public float FrameDuration { get; set; }

        public Vector2 CurrentFrameLocation { get => FrameLocations[CurrentFrameIndex]; }
        public Vector2 FrameSize { get => new Vector2(FrameWidth, FrameHeight); }
    }
}
