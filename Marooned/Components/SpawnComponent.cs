using DefaultEcs;
using Marooned.Interpreter;
using Microsoft.Xna.Framework;
using System;

namespace Marooned.Components
{
    public struct SpawnComponent
    {
        public World World { get; set; }
        public Entity Actor { get; set; }
        public Vector2? Position { get; set; }
        public string EntityNameToSpawn { get; set; }
        public int Count { get; set; }
        public double? Rate { get; set; }
        public TimeSpan? Duration { get; set; }
        public GetScriptFunction GetScriptFunction { get; set; }
        public bool OverrideScript { get; set; }
        public TimeSpan Timer { get; set; }
        public double RateTimer { get; set; }

        public bool ShouldSpawn { get => !Duration.HasValue || (Duration.HasValue && RateTimer >= Rate); }
        public bool IsFinished { get => Duration.HasValue ? Timer >= Duration.Value : true; }
    }
}
