using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;

namespace Marooned.Systems
{
    public class ScriptSystem : AComponentSystem<GameContext, ScriptComponent>
    {
        public ScriptSystem(World world) : base(world)
        {
        }

        protected override void Update(GameContext gameContext, ref ScriptComponent script)
        {
            script.TimeElapsed += gameContext.GameTime.ElapsedGameTime;
            if (script.Script.ShouldExecute(script.TimeElapsed))
            {
                script.Script.Execute(gameContext);
            }
        }
    }
}
