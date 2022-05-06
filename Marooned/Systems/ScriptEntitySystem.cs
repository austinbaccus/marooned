using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;

namespace Marooned.Systems
{
    [With(typeof(ScriptComponent))]
    public class ScriptEntitySystem : AEntitySetSystem<GameContext>
    {
        public ScriptEntitySystem(World world) : base(world, true)
        {
        }

        protected override void Update(GameContext gameContext, in Entity entity)
        {
            //ScriptComponent scriptComponent = entity.Get<ScriptComponent>();
            //if (scriptComponent.Script.Empty())
            //{
            //    entity.Dispose();
            //}
        }
    }
}
