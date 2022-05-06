using Marooned.Animation;

namespace Marooned.Components
{
    public struct AnimationComponent
    {
        public AnimationComponent(int numAnimations) : this()
        {
            Animations = new AnimationInfo[numAnimations];
            IsPlaying = false;
            Timer = 0;
        }

        public AnimationInfo[] Animations;
        public bool IsPlaying { get; set; }
        public int CurrentAnimationIndex { get; set; }
        public AnimationInfo CurrentAnimation { get => Animations[CurrentAnimationIndex]; }
        public float Timer { get; set; }

        public void Play(string animationName)
        {
            for (int i = 0; i < Animations.Length; i++)
            {
                if (Animations[i].Name == animationName)
                {
                    CurrentAnimationIndex = i;
                    break;
                }
            }
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
        }

        public void SetCurrentAnimationFrame(int index)
        {
            Animations[CurrentAnimationIndex].CurrentFrameIndex = index;
        }
    }
}
