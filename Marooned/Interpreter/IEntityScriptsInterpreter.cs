using DefaultEcs;

namespace Marooned.Interpreter
{
    public interface IEntityScriptsInterpreter
    {
        Script CreateScriptFrom(string name, Entity entity);
    }
}
