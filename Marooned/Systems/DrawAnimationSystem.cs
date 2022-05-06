using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned.Systems
{
    [With(typeof(DrawableComponent), typeof(AnimationComponent), typeof(TransformComponent))]
    [Without(typeof(ExtraDrawableInfoComponent))]
    public sealed partial class DrawAnimationSystem : AEntitySetSystem<GameContext>
    {
        public DrawAnimationSystem(World world) : base(world)
        {
        }

        protected override void PreUpdate(GameContext gameContext)
        {
            gameContext.SpriteBatch.Begin(sortMode: SpriteSortMode.Deferred, transformMatrix: gameContext.StateManager.CurrentState.Camera?.GetViewMatrix() ?? Matrix.Identity, samplerState: SamplerState.PointClamp);
        }

        protected override void Update(GameContext gameContext, in Entity entity)
        {
            DrawableComponent drawable = entity.Get<DrawableComponent>();
            AnimationComponent animation = entity.Get<AnimationComponent>();
            TransformComponent transform = entity.Get<TransformComponent>();

            gameContext.SpriteBatch.Draw(
                drawable.Texture,
                transform.Position,
                new Rectangle(animation.CurrentAnimation.CurrentFrameLocation.ToPoint(), animation.CurrentAnimation.FrameSize.ToPoint()),
                drawable.Color
            );
        }

        protected override void PostUpdate(GameContext gameContext)
        {
            gameContext.SpriteBatch.End();
        }
    }
}
