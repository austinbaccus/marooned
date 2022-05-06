using DefaultEcs;
using DefaultEcs.System;
using Marooned.Components;

namespace Marooned.Systems
{
    public class AnimationSystem : AComponentSystem<GameContext, AnimationComponent>
    {
        public AnimationSystem(World world) : base(world)
        {
        }

        protected override void Update(GameContext gameContext, ref AnimationComponent animation)
        {
            animation.Timer += (float)gameContext.GameTime.ElapsedGameTime.TotalSeconds;

            if (animation.Timer >= animation.CurrentAnimation.FrameDuration)
            {
                animation.Timer = 0;
                int newFrameIndex = animation.CurrentAnimation.CurrentFrameIndex + 1;

                if (newFrameIndex >= animation.CurrentAnimation.FrameCount)
                {
                    if (animation.CurrentAnimation.IsLooping)
                    {
                        animation.SetCurrentAnimationFrame(0);
                    }
                    else
                    {
                        animation.Stop();
                    }
                }
                else
                {
                    animation.SetCurrentAnimationFrame(newFrameIndex);
                }
            }
        }
    }
}
