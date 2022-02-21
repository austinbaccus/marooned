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
        public static ContentManager content;
        public static Grunt MakeGrunt(string enemyType, Vector2 position, int hitboxRadius, string hitboxSpritePath)
        {
            // load json file
            string json = File.ReadAllText(String.Format("./Content/Enemies/{0}.json",enemyType));
            dynamic jsObj = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());

            var texture = content.Load<Texture2D>(jsObj.texture);

            Grunt grunt = new Grunt(texture, new Rectangle[] { new Rectangle(0, 0, 16, 32) }, FiringPattern.Pattern.straight, MovementPattern.Pattern.down_left, (int)jsObj.health);
            grunt.Hitbox = new Hitbox(grunt) { Radius = hitboxRadius };
            grunt.Position = position;
#if DEBUG
            grunt.HitboxSprite = new Sprite(content.Load<Texture2D>("Sprites/PlayerHitbox"));
#endif
            return grunt;
        }
    }
}
