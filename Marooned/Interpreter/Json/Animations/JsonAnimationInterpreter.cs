using DefaultEcs;
using Marooned.Builders;
using Marooned.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Marooned.Interpreter.Json.Animations
{
    public class JsonAnimationInterpreter : JsonInterpreter, IAnimationsInterpreter
    {
        public JsonAnimationInterpreter(GameContext gameContext) : base(gameContext)
        {
        }

        public override string Path { get; set; } = "Animations";

        public void CreateAnimationForEntity(Entity entity, string name)
        {
            AnimationComponent animationComponent = CreateAnimationComponentFrom(name);
            DrawableComponent drawableComponent = CreateDrawableComponentFrom(name);
            entity.Set(animationComponent);
            entity.Set(drawableComponent);
        }

        public AnimationComponent CreateAnimationComponentFrom(string name)
        {
            // ContentManager does not accept "Content/" in the beginning of the path.
            string animationJsonPath = FindJsonByName(name, false);
            AnimationJson animationInfo = Load<AnimationJson>(animationJsonPath);
            Texture2D texture = GameContext.Content.Load<Texture2D>(animationInfo.TextureAtlas.Texture);
            int numAnimations = animationInfo.Cycles.Count;

            AnimationComponentBuilder builder = new AnimationComponentBuilder(numAnimations);
            foreach (var entry in animationInfo.Cycles)
            {
                builder.AddAnimationInfo(
                    name: entry.Key,
                    frameLocations: entry.Value.Frames.Select(
                        (_, i) => entry.Value.GetFrameLocation(
                            i,
                            animationInfo.TextureAtlas.RegionWidth,
                            animationInfo.TextureAtlas.RegionHeight,
                            texture.Width
                        )
                    ).ToArray(),
                    frameWidth: animationInfo.TextureAtlas.RegionWidth,
                    frameHeight: animationInfo.TextureAtlas.RegionHeight,
                    isLooping: entry.Value.IsLooping,
                    frameDuration: entry.Value.FrameDuration
                );
            }

            return builder.Build();
        }

        public DrawableComponent CreateDrawableComponentFrom(string name)
        {
            // ContentManager does not accept "Content/" in the beginning of the path.
            string animationJsonPath = FindJsonByName(name, false);
            AnimationJson animationInfo = Load<AnimationJson>(animationJsonPath);
            Texture2D texture = GameContext.Content.Load<Texture2D>(animationInfo.TextureAtlas.Texture);
            return new DrawableComponent
            {
                Texture = texture,
                Source = null,
                Color = Color.White,
            };
        }
    }
}
