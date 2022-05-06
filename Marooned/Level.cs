namespace Marooned
{
    public class Level : ILifeCycle
    {
        private ScriptOld _script;

        public Level(GameContext gameContext)
        {
            GameContext = gameContext;
        }

        public GameContext GameContext { get; set; }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            // TODO: Load script from JSON using GameContext.Interpreter here!!
            _script = new ScriptOld();
        }

        public void UnloadContent()
        {
        }

        public void Dispose()
        {
        }

        public void Update()
        {   
        }

        public void Draw()
        {
        }
    }
}
