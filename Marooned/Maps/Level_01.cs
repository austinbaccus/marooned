﻿using Microsoft.Xna.Framework;

namespace Marooned.Maps
{
    class Level_01 : Map
    {
        private Tiles[] t =
        {
            new Tiles("Terrain/sand", new Rectangle(128, 32, 16, 16), 1, 1, true), // 0 - sand
            new Tiles("Terrain/spring_island_tilesheet_1", new Rectangle(128, 16, 16, 16), 1, 1, true), // 1 - sand with rocks
            new Tiles("Terrain/spring_island_tilesheet_1", new Rectangle(112, 64, 16, 16), 1, 1, true), // 2 - deep water
        };

        public override void Generate()
        {
            tilemap = new Tiles[,]
            {
                { t[2], t[1], t[0], t[2], t[1], t[0], t[2], t[1], t[0], t[2], t[1], t[0], },
                { t[1], t[0], t[2], t[2], t[1], t[0], t[2], t[1], t[0], t[2], t[1], t[0], },
                { t[2], t[1], t[0], t[2], t[1], t[0], t[2], t[1], t[1], t[2], t[1], t[0], },
                { t[1], t[1], t[0], t[2], t[1], t[0], t[2], t[1], t[2], t[1], t[1], t[0], },
                { t[2], t[1], t[0], t[2], t[1], t[0], t[2], t[1], t[2], t[2], t[1], t[0], },
                { t[1], t[1], t[0], t[2], t[1], t[0], t[2], t[1], t[0], t[2], t[1], t[0], },
            };
        }
    }
}
