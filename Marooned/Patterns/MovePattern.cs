using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Marooned.Sprites.Enemies
{
    public class MovePattern
    {
        static float ENEMY_MOVEMENT_SPEED_FACTOR = 30f;
        static float MINIBOSS_MOVEMENT_SPEED_FACTOR = 50f;
        static float BOSS_MOVEMENT_SPEED_FACTOR = 50f;

        //`````````````````````````````````````````````````````````````````````````````````
        static Vector2 up = new Vector2(0, -1);
        static Vector2 down = new Vector2(0, 1);
        static Vector2 left = new Vector2(-1, 0);
        static Vector2 right = new Vector2(1, 0);
        static Vector2 right_up = new Vector2(1, -1);
        static Vector2 left_up = new Vector2(-1, -1);
        static Vector2 left_down = new Vector2(-1, 1);
        static Vector2 right_down = new Vector2(1, 1);
        static Vector2 hover = new Vector2(0, 0);
        //`````````````````````````````````````````````````````````````````````````````````
        static Tuple<Vector2, int> flyoff = new Tuple<Vector2, int>(up * ENEMY_MOVEMENT_SPEED_FACTOR * 2, 400);
        static Tuple<Vector2, int> flyoff_left = new Tuple<Vector2, int>(left_up * ENEMY_MOVEMENT_SPEED_FACTOR * 2, 400);
        static Tuple<Vector2, int> flyoff_right = new Tuple<Vector2, int>(right_up * ENEMY_MOVEMENT_SPEED_FACTOR * 2, 400);
        static Tuple<Vector2, int> dangerous_flyoff = new Tuple<Vector2, int>(up * ENEMY_MOVEMENT_SPEED_FACTOR * 2, 300);
        static Tuple<Vector2, int> mage_flyoff = new Tuple<Vector2, int>(up * ENEMY_MOVEMENT_SPEED_FACTOR * 2, 400);
        static Tuple<Vector2, int> mini_flyoff = new Tuple<Vector2, int>(up * ENEMY_MOVEMENT_SPEED_FACTOR * 2, 200);
        static Tuple<Vector2, int> boss_flyoff = new Tuple<Vector2, int>(up * ENEMY_MOVEMENT_SPEED_FACTOR * 2, 100);
        //`````````````````````````````````````````````````````````````````````````````````

        static List<Tuple<Vector2, int>> left_spawn = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(right_down * ENEMY_MOVEMENT_SPEED_FACTOR * 3, 200),
            new Tuple<Vector2, int>(hover, 600),
            flyoff
        };
        static List<Tuple<Vector2, int>> left_spawn_across = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(right_down * ENEMY_MOVEMENT_SPEED_FACTOR * 3, 200),
            new Tuple<Vector2, int>(right * ENEMY_MOVEMENT_SPEED_FACTOR, 800),
            flyoff
        };
        static List<Tuple<Vector2, int>> right_spawn_across = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(left_down * ENEMY_MOVEMENT_SPEED_FACTOR * 3, 200),
            new Tuple<Vector2, int>(left * ENEMY_MOVEMENT_SPEED_FACTOR, 800),
            flyoff
        };
        static List<Tuple<Vector2, int>> right_spawn = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(left_down * ENEMY_MOVEMENT_SPEED_FACTOR * 3, 200),
            new Tuple<Vector2, int>(hover, 600),
            flyoff
        };
        static List<Tuple<Vector2, int>> center_spawn = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(down * ENEMY_MOVEMENT_SPEED_FACTOR * 3, 200),
            new Tuple<Vector2, int>(hover, 600),
            flyoff
        };
        static List<Tuple<Vector2, int>> straigt_down = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(down * ENEMY_MOVEMENT_SPEED_FACTOR * 2, 200),
            new Tuple<Vector2, int>(hover, 600),
            flyoff_left
        };
        static List<Tuple<Vector2, int>> straigt_down2 = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(down * ENEMY_MOVEMENT_SPEED_FACTOR * 2, 200),
            new Tuple<Vector2, int>(hover, 600),
            flyoff_right
        };
        static List<Tuple<Vector2, int>> dangerous_left_spawn = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(down * ENEMY_MOVEMENT_SPEED_FACTOR * 3, 200),
            new Tuple<Vector2, int>(right * ENEMY_MOVEMENT_SPEED_FACTOR * 2, 1000),
            dangerous_flyoff
        };
        static List<Tuple<Vector2, int>> dangerous_right_spawn = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(down * ENEMY_MOVEMENT_SPEED_FACTOR * 3, 200),
            new Tuple<Vector2, int>(left * ENEMY_MOVEMENT_SPEED_FACTOR * 2, 1000),
            dangerous_flyoff
        };
        static List<Tuple<Vector2, int>> mage_spawn = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(down * ENEMY_MOVEMENT_SPEED_FACTOR * 3, 150),
            new Tuple<Vector2, int>(hover, 200),
            new Tuple<Vector2, int>(down * ENEMY_MOVEMENT_SPEED_FACTOR * 2, 800),
            new Tuple<Vector2, int>(hover, 200),
            new Tuple<Vector2, int>(right_up * ENEMY_MOVEMENT_SPEED_FACTOR * 3, 200),
            new Tuple<Vector2, int>(hover, 200),
            new Tuple<Vector2, int>(left * ENEMY_MOVEMENT_SPEED_FACTOR * 2, 400),
            new Tuple<Vector2, int>(hover, 200),
            new Tuple<Vector2, int>(right_up * ENEMY_MOVEMENT_SPEED_FACTOR * 3, 200),
            new Tuple<Vector2, int>(hover, 200),
            new Tuple<Vector2, int>(left * ENEMY_MOVEMENT_SPEED_FACTOR * 2, 400),
            new Tuple<Vector2, int>(hover, 200),
            mage_flyoff
        };
        //`````````````````````````````````````````````````````````````````````````````````
        static List<Tuple<Vector2, int>> miniboss = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(right_down * MINIBOSS_MOVEMENT_SPEED_FACTOR * 2, 200),
            new Tuple<Vector2, int>(right * MINIBOSS_MOVEMENT_SPEED_FACTOR, 200),
            new Tuple<Vector2, int>(hover, 200),
            new Tuple<Vector2, int>(left_up * MINIBOSS_MOVEMENT_SPEED_FACTOR, 200),
            new Tuple<Vector2, int>(hover, 100),
            new Tuple<Vector2, int>(right * MINIBOSS_MOVEMENT_SPEED_FACTOR * 2, 200),
            new Tuple<Vector2, int>(hover, 100),
            new Tuple<Vector2, int>(left_down * MINIBOSS_MOVEMENT_SPEED_FACTOR, 200),
            new Tuple<Vector2, int>(hover, 100),
            new Tuple<Vector2, int>(left_up * MINIBOSS_MOVEMENT_SPEED_FACTOR, 200),
            new Tuple<Vector2, int>(hover, 100),
            new Tuple<Vector2, int>(right * MINIBOSS_MOVEMENT_SPEED_FACTOR * 2, 200),
            mini_flyoff
        };
        static List<Tuple<Vector2, int>> boss = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(right_down * BOSS_MOVEMENT_SPEED_FACTOR, 200),
            new Tuple<Vector2, int>(hover, 400),
            new Tuple<Vector2, int>(left_down * BOSS_MOVEMENT_SPEED_FACTOR * 2, 100),
            new Tuple<Vector2, int>(hover, 200),
            new Tuple<Vector2, int>(right_up * BOSS_MOVEMENT_SPEED_FACTOR * 2, 100),
            new Tuple<Vector2, int>(hover, 200),
            new Tuple<Vector2, int>(left * BOSS_MOVEMENT_SPEED_FACTOR * 2, 100),
            new Tuple<Vector2, int>(hover, 200),
            new Tuple<Vector2, int>(right_down * BOSS_MOVEMENT_SPEED_FACTOR * 2, 100),
            new Tuple<Vector2, int>(left_down * BOSS_MOVEMENT_SPEED_FACTOR * 2, 100),
            new Tuple<Vector2, int>(hover, 200),
            new Tuple<Vector2, int>(left_up * BOSS_MOVEMENT_SPEED_FACTOR * 2, 100),
            new Tuple<Vector2, int>(right_up * BOSS_MOVEMENT_SPEED_FACTOR * 2, 100),
            new Tuple<Vector2, int>(hover, 200),
            new Tuple<Vector2, int>(left * BOSS_MOVEMENT_SPEED_FACTOR * 2, 100),
            new Tuple<Vector2, int>(hover, 200),
            new Tuple<Vector2, int>(down * BOSS_MOVEMENT_SPEED_FACTOR * 2, 200),
            new Tuple<Vector2, int>(right * BOSS_MOVEMENT_SPEED_FACTOR * 2, 100),
            new Tuple<Vector2, int>(hover, 200),
            new Tuple<Vector2, int>(right * BOSS_MOVEMENT_SPEED_FACTOR * 2, 250),
            new Tuple<Vector2, int>(left_up * BOSS_MOVEMENT_SPEED_FACTOR,500),
            new Tuple<Vector2, int>(left_down * BOSS_MOVEMENT_SPEED_FACTOR, 500),
            new Tuple<Vector2, int>(down * BOSS_MOVEMENT_SPEED_FACTOR * 2, 250),
            new Tuple<Vector2, int>(right_up * BOSS_MOVEMENT_SPEED_FACTOR, 500),
            new Tuple<Vector2, int>(right_down * BOSS_MOVEMENT_SPEED_FACTOR, 500),
            new Tuple<Vector2, int>(up * BOSS_MOVEMENT_SPEED_FACTOR * 2, 250),
            new Tuple<Vector2, int>(left_up * BOSS_MOVEMENT_SPEED_FACTOR, 500),
            new Tuple<Vector2, int>(hover, 400),
            boss_flyoff
        };

        //`````````````````````````````````````````````````````````````````````````````````
        //                              Bullet Move Patterns
        //`````````````````````````````````````````````````````````````````````````````````
        static List<Tuple<Vector2, int, (float, float)>> fire_down = new List<Tuple<Vector2, int, (float, float)>>()
        {
            new Tuple<Vector2, int, (float, float)>(down, 200, (0, 5)),
        };
        static List<Tuple<Vector2, int, (float, float)>> fire_mage = new List<Tuple<Vector2, int, (float, float)>>()
        {
            new Tuple<Vector2, int, (float, float)>(left_up, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(right_up, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(left_down, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(right_down, 250, (0, 0)),
        };
        static List<Tuple<Vector2, int, (float, float)>> fire_dangerous = new List<Tuple<Vector2, int, (float, float)>>()
        {
            new Tuple<Vector2, int, (float, float)>(down, 200, (0, 5)),
            new Tuple<Vector2, int, (float, float)>(left, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(right, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(up, 250, (0, 0)),
        };
        static List<Tuple<Vector2, int, (float, float)>> fire_mini = new List<Tuple<Vector2, int, (float, float)>>()
        {
            new Tuple<Vector2, int, (float, float)>(down, 200, (-20, 0)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (0, 5)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (20, 10)),
            new Tuple<Vector2, int, (float, float)>(left, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(right, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(up, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(left_up, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(right_up, 250, (0, 0)),
        };
        static List<Tuple<Vector2, int, (float, float)>> fire_boss = new List<Tuple<Vector2, int, (float, float)>>()
        {
            //new Tuple<Vector2, int, (float, float)>(down, 200, (-20, 0)),
            //new Tuple<Vector2, int, (float, float)>(down, 200, (0, 5)),
            //new Tuple<Vector2, int, (float, float)>(down, 200, (20, 10)),
            new Tuple<Vector2, int, (float, float)>(left_up, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(right_up, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(left_down, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(right_down, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(left, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(right, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(up, 250, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (0, 100)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (10, 90)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (15, 80)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (15, 70)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (10, 60)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (0, 50)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-10, 55)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-20, 65)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-10, 70)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (0, 70)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (5, 60)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (15, 50)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (20, 40)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (20, 30)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (15, 20)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (10, 10)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (0, 0)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-10, -10)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-20, -20)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-30, -30)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-40, -35)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-50, -35)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-60, -30)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-70, -20)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-75, -10)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-75, 0)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-70, 10)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-65, 20)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-60, 30)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-50, 40)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-40, 45)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-30, 45)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-20, 40)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (-10, 30)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (0, 20)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (10, 10)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (20, 0)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (25, -10)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (30, -20)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (30, -30)),
            new Tuple<Vector2, int, (float, float)>(down, 200, (30, -40)),
        };

        public enum Pattern
        {
            left_spawn,
            right_spawn,
            center_spawn,
            miniboss,
            boss,
            left_spawn_across,
            right_spawn_across,
            dangerous_left_spawn,
            dangerous_right_spawn,
            straigt_down,
            straigt_down2,
            mage_spawn,
            fire_down,
            fire_mage,
            fire_dangerous,
            fire_mini,
            fire_boss
        }

        public static List<Tuple<Vector2, int>> GetPattern(Pattern pattern)
        {
            switch (pattern)
            {
                case (Pattern.left_spawn): { return left_spawn; }
                case (Pattern.left_spawn_across): { return left_spawn_across; }
                case (Pattern.right_spawn): { return right_spawn; }
                case (Pattern.right_spawn_across): { return right_spawn_across; }
                case (Pattern.center_spawn): { return center_spawn; }
                case (Pattern.miniboss): { return miniboss; }
                case (Pattern.boss): { return boss; }
                case (Pattern.dangerous_left_spawn): { return dangerous_left_spawn; }
                case (Pattern.dangerous_right_spawn): { return dangerous_right_spawn; }
                case (Pattern.straigt_down): { return straigt_down; }
                case (Pattern.straigt_down2): { return straigt_down2; }
                case (Pattern.mage_spawn): { return mage_spawn; }
                default: return left_spawn;
            }
        }

        public static List<Tuple<Vector2, int, (float, float)>> GetFiringPattern(Pattern pattern)
        {
            switch (pattern)
            {
                case (Pattern.fire_down): { return fire_down; }
                case (Pattern.fire_mage): { return fire_mage; }
                case (Pattern.fire_dangerous): { return fire_dangerous; }
                case (Pattern.fire_mini): { return fire_mini; }
                case (Pattern.fire_boss): { return fire_boss; }
                default: return fire_down;
            }
        }
    }
}
