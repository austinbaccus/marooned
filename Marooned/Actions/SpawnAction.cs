using DefaultEcs;
using Marooned.Components;
using Marooned.Interpreter;
using Microsoft.Xna.Framework;
using System;

namespace Marooned.Actions
{
    public class SpawnAction : IAction
    {
        public SpawnAction(World world,
                           Entity actor,
                           string entityNameToSpawn,
                           Vector2? position,
                           int count,
                           double rate = 0,
                           double duration = 0,
                           GetScriptFunction getScriptFunction = null,
                           bool overrideScript = true)
        {
            World = world;
            Actor = actor;
            EntityNameToSpawn = entityNameToSpawn;
            Position = position;
            Count = count;
            Rate = rate;
            Duration = duration;
            GetScriptFunction = getScriptFunction;
            OverrideScript = overrideScript;
        }

        public World World { get; set; }
        public Entity Actor { get; set; }
        public Vector2? Position { get; set; }
        public string EntityNameToSpawn { get; set; }
        public int Count { get; set; }
        public double Rate { get; set; }
        public double Duration { get; set; }
        public GetScriptFunction GetScriptFunction { get; set; }
        public bool OverrideScript { get; set; }

        public void Execute(GameContext gameContext)
        {
            Actor.Set(new SpawnComponent
            {
                World = World,
                Actor = Actor,
                Position = Position,
                EntityNameToSpawn = EntityNameToSpawn,
                Count = Count,
                Rate = Rate,
                Duration = Duration == 0 ? null : TimeSpan.FromSeconds(Duration),
                GetScriptFunction = GetScriptFunction,
            });
        }
    }
}
