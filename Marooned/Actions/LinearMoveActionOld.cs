using Marooned.Sprites;
using Microsoft.Xna.Framework;

namespace Marooned.Actions
{
    public class LinearMoveActionOld : IAction
    {
        private readonly Sprite _sprite;

        public LinearMoveActionOld(Sprite sprite)
        {
            _sprite = sprite;
        }

        public void Execute(GameContext gameContext)
        {
            _sprite.Position += Direction * Speed * (float)gameContext.GameTime.ElapsedGameTime.TotalSeconds;
        }

        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
    }
}
