using System;

namespace Marooned.Components
{
    public struct ScriptComponent
    {
        public Script Script { get; set; }
        public TimeSpan TimeElapsed { get; set; }
    }
}
