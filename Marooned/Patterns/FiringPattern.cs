using System.Collections.Generic;

namespace Marooned.Sprites.Enemies
{
    public class FiringPattern
    {
        public enum Pattern
        {
            straight,
            trident,
            circle
        }
        public static List<double> GetPattern(Pattern pattern)
        {
            switch(pattern)
            {
                case (Pattern.straight): { return new List<double>() { 0 }; }
                case (Pattern.trident): { return new List<double>() { 0, 20, 160 }; }
                case (Pattern.circle): { return new List<double>() { 0, 45, 90, 135, 180, 225, 270, 315 }; }
                default: { return new List<double>() { 0 }; }
            }
        }
    }
}
