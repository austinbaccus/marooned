using Marooned.Sprites;
using Microsoft.Xna.Framework;

namespace Marooned.Actions
{
    public class FocusAction : IAction
    {
        private readonly Player _player;

        public FocusAction(Player player)
        {
            _player = player;
        }

        public void Execute(GameTime gameTime)
        {
            _player.IsFocused = true;
        }
    }
}
