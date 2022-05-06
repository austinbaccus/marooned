using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;
using Microsoft.Xna.Framework;
using System;

namespace Marooned.Systems
{
    [With(typeof(SpawnComponent))]
    public class SpawnActionSystem : AEntitySetSystem<GameContext>
    {
        public SpawnActionSystem(World world, bool useBuffer = true) : base(world, useBuffer)
        {
        }

        protected override void Update(GameContext gameContext, in Entity entity)
        {
            SpawnComponent spawnComponent = entity.Get<SpawnComponent>();

            if (spawnComponent.ShouldSpawn)
            {
                spawnComponent.RateTimer = 0;
                for (int i = 0; i < spawnComponent.Count; i++)
                {
                    // TODO: Change to world spawn point?
                    Vector2 newEntityPosition = Vector2.Zero;
                    Entity newEntity = gameContext.EntitiesInterpreter.CreateEntityFrom(World, spawnComponent.EntityNameToSpawn);
                    if (spawnComponent.Position.HasValue)
                    {
                        newEntityPosition = spawnComponent.Position.Value;
                    }
                    else
                    {
                        if (entity.Has<TransformComponent>())
                        {
                            newEntityPosition = entity.Get<TransformComponent>().Position;
                        }
                    }
                    newEntity.Set(new TransformComponent
                    {
                        Position = newEntityPosition,
                    });

                    if (spawnComponent.GetScriptFunction != null)
                    {
                        Script? newEntityScript = null;
                        if (!spawnComponent.OverrideScript && newEntity.Has<ScriptComponent>())
                        {
                            ScriptComponent scriptComponent = newEntity.Get<ScriptComponent>();
                            newEntityScript = spawnComponent.GetScriptFunction(World, newEntity);
                            newEntityScript.Value.Enqueue(scriptComponent.Script);
                        }
                        else
                        {
                            newEntityScript = spawnComponent.GetScriptFunction(World, newEntity);
                        }

                        newEntity.Set(new ScriptComponent
                        {
                            Script = newEntityScript.Value,
                            TimeElapsed = TimeSpan.Zero,
                        });
                    }
                }
            }

            if (spawnComponent.IsFinished)
            {
                entity.Remove<SpawnComponent>();
            }
        }
    }
}
