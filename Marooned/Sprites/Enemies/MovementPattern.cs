﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marooned.Sprites.Enemies
{
    public class MovementPattern
    {
        static float ENEMY_MOVEMENT_SPEED_FACTOR = 10f;
        static float MINIBOSS_MOVEMENT_SPEED_FACTOR = 50f;
        static float BOSS_MOVEMENT_SPEED_FACTOR = 50f;

        static List<Tuple<Vector2, int>> down_left = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(new Vector2(0,1) * ENEMY_MOVEMENT_SPEED_FACTOR,1000),
            new Tuple<Vector2, int>(new Vector2(-1,0) * ENEMY_MOVEMENT_SPEED_FACTOR,1000)
        };
        static List<Tuple<Vector2, int>> down_right = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(new Vector2(0,1) * ENEMY_MOVEMENT_SPEED_FACTOR,1000),
            new Tuple<Vector2, int>(new Vector2(1,0) * ENEMY_MOVEMENT_SPEED_FACTOR,1000)
        };
        static List<Tuple<Vector2, int>> miniboss = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(new Vector2(0.4f,-0.3f) * MINIBOSS_MOVEMENT_SPEED_FACTOR,1000),
            new Tuple<Vector2, int>(new Vector2(-0.4f,0.3f) * MINIBOSS_MOVEMENT_SPEED_FACTOR,1000),
            new Tuple<Vector2, int>(new Vector2(-0.4f,-0.3f) * MINIBOSS_MOVEMENT_SPEED_FACTOR,1000),
            new Tuple<Vector2, int>(new Vector2(0.4f,0.3f) * MINIBOSS_MOVEMENT_SPEED_FACTOR,1000),
        };
        static List<Tuple<Vector2, int>> boss = new List<Tuple<Vector2, int>>()
        {
            new Tuple<Vector2, int>(new Vector2(1f,0f) * BOSS_MOVEMENT_SPEED_FACTOR,500),
            new Tuple<Vector2, int>(new Vector2(-1f,-1f) * BOSS_MOVEMENT_SPEED_FACTOR,500),
            new Tuple<Vector2, int>(new Vector2(-1f,1f) * BOSS_MOVEMENT_SPEED_FACTOR,500),
            new Tuple<Vector2, int>(new Vector2(1f,-1f) * BOSS_MOVEMENT_SPEED_FACTOR,500),
            new Tuple<Vector2, int>(new Vector2(1f,1f) * BOSS_MOVEMENT_SPEED_FACTOR,500),
            new Tuple<Vector2, int>(new Vector2(-1f,-1f) * BOSS_MOVEMENT_SPEED_FACTOR,500),
            new Tuple<Vector2, int>(new Vector2(-1f,1f) * BOSS_MOVEMENT_SPEED_FACTOR,500),
            new Tuple<Vector2, int>(new Vector2(1f,-1f) * BOSS_MOVEMENT_SPEED_FACTOR,500),
            new Tuple<Vector2, int>(new Vector2(1f,1f) * BOSS_MOVEMENT_SPEED_FACTOR,500),
        };

        public enum Pattern
        {
            down_left,
            down_right,
            miniboss,
            boss
        }

        public static List<Tuple<Vector2, int>> GetPattern(Pattern pattern)
        {
            switch (pattern)
            {
                case (Pattern.down_left): { return down_left; }
                case (Pattern.down_right): { return down_right; }
                case (Pattern.miniboss): { return miniboss; }
                case (Pattern.boss): { return boss; }
                default: return down_left;
            }
        }
    }
}
