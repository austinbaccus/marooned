using Marooned.Animation;
using Marooned.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Marooned.Builders
{
    public class AnimationComponentBuilder
    {
        private List<AnimationInfo> _animationInfos;

        public AnimationComponentBuilder(int? capacity = null)
        {
            if (capacity.HasValue)
            {
                _animationInfos = new List<AnimationInfo>(capacity.Value);
            }
            else
            {
                _animationInfos = new List<AnimationInfo>();
            }
        }

        public void AddAnimationInfo(AnimationInfo AnimationInfo)
        {
            _animationInfos.Add(AnimationInfo);
        }

        public void AddAnimationInfo(string name, Vector2[] frameLocations, int frameWidth, int frameHeight, bool isLooping, float frameDuration)
        {
            AddAnimationInfo(new AnimationInfo
            {
                Name = name,
                FrameLocations = frameLocations,
                CurrentFrameIndex = 0,
                FrameCount = frameLocations.Length,
                FrameWidth = frameWidth,
                FrameHeight = frameHeight,
                IsLooping = isLooping,
                FrameDuration = frameDuration,
            });
        }

        public AnimationComponent Build()
        {
            return new AnimationComponent(_animationInfos.Count)
            {
                Animations = _animationInfos.ToArray(),
                CurrentAnimationIndex = 0,
            };
        }
    }
}
