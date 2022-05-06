using DefaultEcs;

namespace Marooned.Interpreter
{
    public interface ILevelsInterpreter
    {
        public Entity GetLevelEntity(string name, World world);
    }
}
