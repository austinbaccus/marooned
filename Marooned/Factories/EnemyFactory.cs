using Marooned.Sprites;
using Marooned.Sprites.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Marooned.Factories
{
    public static class EnemyFactory
    {
        // TODO: Remove this
        private static readonly Rectangle[] DEFAULT_ANIM_SOURCES = new Rectangle[] { new Rectangle(0, 0, 16, 32) };

        // TODO: Improve this Json class to deserialize everything into their respective types
        class EnemyJson
        {
            [JsonProperty("health")]
            public int Health { get; set; } = 0;

            [JsonProperty("texture", Required = Required.Always)]
            public string Texture { get; set; }

            [JsonProperty("firing_pattern")]
            [JsonConverter(typeof(StringEnumConverter))]
            public FiringPattern.Pattern? FiringPattern { get; set; } = null;

            [JsonProperty("movement_pattern")]
            [JsonConverter(typeof(StringEnumConverter))]
            public MovementPattern.Pattern? MovementPattern { get; set; } = null;

            [JsonProperty("animation_sources")]
            public List<List<int>>? AnimationSources { get; set; } = null;
        }

        public static ContentManager content;
        public static Grunt MakeGrunt(string enemyType, Vector2 position, int hitboxRadius)
        {
            // load json file
            string json = File.ReadAllText($"./Content/Enemies/{enemyType}.json");
            EnemyJson enemyJson = JsonConvert.DeserializeObject<EnemyJson>(json);

            var texture = content.Load<Texture2D>(enemyJson.Texture);

            FiringPattern.Pattern firingPattern = enemyJson.FiringPattern ?? FiringPattern.Pattern.straight;
            MovementPattern.Pattern movementPattern = enemyJson.MovementPattern ?? MovementPattern.Pattern.down_left;

            // TODO: Probably make a Json parser for Rectangle (?)
            Rectangle[] animSources;
            if (enemyJson.AnimationSources is List<List<int>> jsonAnimSources)
            {
                animSources = new Rectangle[jsonAnimSources.Count];
                for (int i = 0; i < jsonAnimSources.Count; i++)
                {
                    animSources[i] = new Rectangle(
                        jsonAnimSources[i][0],
                        jsonAnimSources[i][1],
                        jsonAnimSources[i][2],
                        jsonAnimSources[i][3]
                    );
                }
            }
            else
            {
                animSources = DEFAULT_ANIM_SOURCES;
            }

            Grunt grunt = new Grunt(texture, animSources, firingPattern, movementPattern, enemyJson.Health);
            grunt.Hitbox = new Hitbox(grunt) { Radius = hitboxRadius };
            grunt.Position = position;
#if DEBUG
            grunt.HitboxSprite = new Sprite(content.Load<Texture2D>("Sprites/PlayerHitbox"));
#endif
            return grunt;
        }
    }
}
