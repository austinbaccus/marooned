using System;

namespace Marooned
{
    public interface ILifeCycle : IDisposable
    {
        void Initialize();
        void LoadContent();
        void UnloadContent();
        new void Dispose();

        void Update();
        void Draw();
    }
}
