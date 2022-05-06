using Marooned.Factories;
using Microsoft.Xna.Framework;

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
            BulletFactory.MakeBullet(gameContext, gameContext.StateManager.CurrentState.World, "banana", Origin);
        }

        public float LifeSpan { get; set; }
        public Vector2 Velocity { get; set; }
        public float Damage { get; set; }
        public Vector2 Origin { get; set; }
    }
}
