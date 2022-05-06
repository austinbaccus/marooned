using DefaultEcs;
using Marooned.Components;

namespace Marooned.Interpreter
{
    public interface IAnimationsInterpreter
    {
        void CreateAnimationForEntity(Entity entity, string name, string currentAnimation = null);
        AnimationComponent CreateAnimationComponentFrom(string name);
    }
}
