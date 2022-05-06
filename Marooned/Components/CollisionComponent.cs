using DefaultEcs;

namespace Marooned.Components
{
    public struct CollisionComponent
    {
        public Entity? CollidedWith { get; set; }
    }
}
