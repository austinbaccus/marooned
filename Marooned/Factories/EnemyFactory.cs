using Marooned.Sprites;
using Marooned.Sprites.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using DefaultEcs;
using Marooned.Components;

namespace Marooned.Factories
{
    #nullable enable

    public static class EnemyFactory
    {
        public static Entity MakeGrunt(GameContext gameContext, World world, string name, Vector2 at)
        {
            Entity entity = gameContext.EntitiesInterpreter.CreateEntityFrom(world, name);
            if (!entity.Has<TransformComponent>())
            {
                entity.Set(new TransformComponent()
                {
                    Position = at,
                });
            }
            entity.Set<IsEnemyComponent>();
            entity.Set(new CollisionComponent());
            return entity;
            //            // load json file
            //            string json = File.ReadAllText($"./Content/Enemies/{enemyType}.json");
            //            EnemyJson? enemyJson = JsonConvert.DeserializeObject<EnemyJson>(json);

            //            if (enemyJson == null)
            //            {
            //                throw new Exception($"Could not find enemy: {enemyType}.json");
            //            }

            //            var texture = gameContext.Content.Load<Texture2D>(enemyJson.Texture);

            //            FiringPattern.Pattern firingPattern = enemyJson.FiringPattern ?? FiringPattern.Pattern.straight;
            //            MovePatternOld.Pattern movementPattern = MovePatternOld.Pattern.test;
            //                //enemyJson.MovementPattern ?? MovePatternOld.Pattern.down_left;

            //            // TODO: Probably make a Json parser for Rectangle (?)
            //            Rectangle[] animSources;
            //            if (enemyJson.AnimationSources is List<List<int>> jsonAnimSources)
            //            {
            //                animSources = new Rectangle[jsonAnimSources.Count];
            //                for (int i = 0; i < jsonAnimSources.Count; i++)
            //                {
            //                    animSources[i] = new Rectangle(
            //                        jsonAnimSources[i][0],
            //                        jsonAnimSources[i][1],
            //                        jsonAnimSources[i][2],
            //                        jsonAnimSources[i][3]
            //                    );
            //                }
            //            }
            //            else
            //            {
            //                animSources = DEFAULT_ANIM_SOURCES;
            //            }

            //            GruntOld grunt = new GruntOld(texture, animSources, firingPattern, movementPattern, enemyJson.Health);
            //            grunt.Hitbox = new Hitbox(grunt) { Radius = hitboxRadius };
            //            grunt.Position = position;
            //#if DEBUG
            //            grunt.HitboxSprite = new Sprite(gameContext.Content.Load<Texture2D>("Sprites/PlayerHitbox"));
            //#endif
            //            return grunt;
        }
    }
}
