using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned.Sprites.Enemies
{
    public class Boss : GruntOld
    {
        public Boss(Texture2D texture, Rectangle[] animSources, FiringPattern.Pattern firingPattern, MovePatternOld.Pattern movementPattern, int health) : base(texture, animSources, firingPattern, movementPattern, health)
        {
        }
    }
}
