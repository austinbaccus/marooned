using DefaultEcs;
using Marooned.Components;
using Marooned.Factories;
using Marooned.Patterns;
using Microsoft.Xna.Framework;
using System;

namespace Marooned.Actions
{
    public class ShootAction : IAction
    {
        //private List<Bullet> _bulletList;

        public ShootAction()
        {
            //_bulletList = bulletList;
        }

        public void Execute(GameContext gameContext)
        {
            //_bulletList.Add(BulletFactory.MakeBullet(gameContext, LifeSpan, Velocity, Damage, Origin));
            Entity entity = BulletFactory.MakeBullet(gameContext, gameContext.StateManager.CurrentState.World, "banana", Origin);
            entity.Set(new TransformComponent
            {
                Position = Origin,
            });
            entity.Set(new VelocityComponent
            {
                Value = Velocity,
            });
            if (!entity.Has<ScriptComponent>())
            {
                entity.Set(new ScriptComponent
                {
                    Script = new Script(),
                    TimeElapsed = TimeSpan.Zero,
                });
            }
            entity.Get<ScriptComponent>().Script.Enqueue(
                new MoveAction(new LinearMovePattern(), entity, TimeSpan.FromSeconds(LifeSpan)), 0
            );
        }

        public float LifeSpan { get; set; }
        public Vector2 Velocity { get; set; }
        public float Damage { get; set; }
        public Vector2 Origin { get; set; }
    }
}
