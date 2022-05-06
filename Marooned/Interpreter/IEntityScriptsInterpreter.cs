using DefaultEcs;

namespace Marooned.Interpreter
{
    public delegate Script GetScriptFunction(World world, Entity entity);

    public interface IEntityScriptsInterpreter
    {
        Script CreateScriptFrom(string name, World world, Entity entity);
    }
}
