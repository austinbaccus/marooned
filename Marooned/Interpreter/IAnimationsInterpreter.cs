using DefaultEcs;
using Marooned.Components;

namespace Marooned.Interpreter
{
    public interface IAnimationsInterpreter
    {
        void CreateAnimationForEntity(Entity entity, string name);
        AnimationComponent CreateAnimationComponentFrom(string name);
    }
}
