using Marooned.Sprites;

namespace Marooned.Actions
{
    public class FocusAction : IAction
    {
        private readonly Player _player;

        public FocusAction(Player player)
        {
            _player = player;
        }

        public void Execute(GameContext gameContext)
        {
            _player.IsFocused = true;
        }
    }
}
