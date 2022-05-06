using Marooned.Patterns;
using System;

namespace Marooned.Components
{
    public struct MoveComponent
    {
        public MovePattern Pattern { get; set; }
        public float Duration { get; set; }
        public TimeSpan Timer { get; set; }

        public bool IsFinished { get => Timer.TotalSeconds >= Duration; }
    }
}
