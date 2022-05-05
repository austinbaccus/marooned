namespace Marooned
{
    public class Interpreter
    {
        public Interpreter(GameContext gameContext)
        {
            GameContext = gameContext;
        }

        public GameContext GameContext { get; set; }
    }
}
