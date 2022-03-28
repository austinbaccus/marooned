using Marooned.Sprites;
using Microsoft.Xna.Framework;

namespace Marooned.Actions
{
    public class UnfocusAction : IAction
    {
        private readonly Player _player;

        public UnfocusAction(Player player)
        {
            _player = player;
        }

        public void Execute(GameTime gameTime)
        {
            _player.IsFocused = false;
        }
    }
}
