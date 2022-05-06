using DefaultEcs;

namespace Marooned.Components
{
    public struct CollisionComponent
    {
        public CollisionComponent()
        {
            HasCollided = false;
            CollidedWith = null;
        }

        public bool HasCollided { get; set; }
        public Entity? CollidedWith { get; set; }
    }
}
