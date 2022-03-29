using Microsoft.Xna.Framework;

namespace Marooned.Actions
{
    public interface IAction
    {
        public void Execute(GameTime gameTime);
    }
}
