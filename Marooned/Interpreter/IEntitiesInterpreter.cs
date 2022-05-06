using DefaultEcs;

namespace Marooned.Interpreter
{
    public interface IEntitiesInterpreter
    {
        Entity CreateEntityFrom(World world, string name);
    }
}
