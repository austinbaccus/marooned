using DefaultEcs;
using Marooned.Components;
using System;

namespace Marooned.Actions
{
    public class ScriptAction : IAction
    {
        public ScriptAction(Script script, Entity entity)
        {
            Script = script;
            Entity = entity;
        }

        public Script Script { get; set; }
        public Entity Entity { get; set; }

        public void Execute(GameContext gameContext)
        {
            if (Entity.Has<ScriptComponent>())
            {
                Script entityScript = Entity.Get<ScriptComponent>().Script;
                while (!Script.Empty())
                {
                    IAction action;
                    double time;
                    Script.TryDequeue(out action, out time);
                    entityScript.Enqueue(action, time);
                }
            }
            else
            {
                Entity.Set(new ScriptComponent
                {
                    Script = Script,
                    TimeElapsed = TimeSpan.Zero,
                });
            }
        }
    }
}
