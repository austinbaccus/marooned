using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned.Systems
{
    [With(typeof(DrawableComponent), typeof(TransformComponent))]
    [Without(typeof(ExtraDrawableInfoComponent), typeof(AnimationComponent))]
    public sealed partial class DrawSystem : AEntitySetSystem<GameContext>
    {
        public DrawSystem(World world) : base(world)
        {
        }

        protected override void PreUpdate(GameContext gameContext)
        {
            gameContext.SpriteBatch.Begin(sortMode: SpriteSortMode.Deferred, transformMatrix: gameContext.StateManager.CurrentState.Camera?.GetViewMatrix() ?? Matrix.Identity, samplerState: SamplerState.PointClamp);
        }

        protected override void Update(GameContext gameContext, in Entity entity)
        {
            DrawableComponent drawable = entity.Get<DrawableComponent>();
            TransformComponent transform = entity.Get<TransformComponent>();

            gameContext.SpriteBatch.Draw(
                drawable.Texture,
                transform.Position,
                drawable.Source,
                drawable.Color
            );
        }

        protected override void PostUpdate(GameContext gameContext)
        {
            gameContext.SpriteBatch.End();
        }
    }
}
