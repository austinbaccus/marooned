using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Marooned.Animation;

namespace Marooned.Sprites
{
    #nullable enable

    public class AnimatedSprite : Sprite
    {
        public AnimationOld? CurrentAnimation { get; set; }
        public override Rectangle Rectangle
        {
            get
            {
                if (CurrentAnimation == null) return Rectangle.Empty;
                return new Rectangle((int)Position.X, (int)Position.Y, CurrentAnimation.FrameWidth, CurrentAnimation.FrameHeight);
            }
        }

        // The origin of all sprites should be the center, rather than top-left, so that position calculation is much more simple and does
        // not have to take into account the sprite width and height.
        public override Vector2 Origin
        {
            get
            {
                if (CurrentAnimation == null) return Vector2.Zero;
                return new Vector2(CurrentAnimation.FrameWidth / 2f, CurrentAnimation.FrameHeight / 2f);
            }
        }

        public AnimatedSprite(Texture2D texture) : base (texture)
        {
            CurrentAnimation = null;
        }

        // For flexibility, we do not automatically play the animation upon creation, so
        // make sure to do <c>CurrentAnimation.Play()</c> in the constructor/initialization of the subclasses
        // if you do want to have the animation play upon creation.
        public AnimatedSprite(Texture2D texture, Rectangle[] animSources) : base(texture)
        {
            CurrentAnimation = new AnimationOld(texture, animSources);
        }

        public override void Update(GameContext gameContext)
        {
            base.Update(gameContext);

            // TODO: Should we even bother checking if CurrentAnimation is null?
            if (CurrentAnimation == null) return;
            CurrentAnimation.Update(gameContext);
        }

        public override void Draw(GameContext gameContext)
        {
            // TODO: Should we even bother checking if CurrentAnimation is null?
            if (CurrentAnimation == null) return;
            if (Destination is Rectangle destination)
            {
                gameContext.SpriteBatch.Draw(
                    texture: CurrentAnimation.Texture,
                    destinationRectangle: destination,
                    sourceRectangle: CurrentAnimation.CurrentSourceRectangle,
                    color: Color,
                    rotation: 0f,
                    origin: Origin,
                    effects: SpriteEffects.None,
                    layerDepth: 0f
                );
            }
            else
            {
                gameContext.SpriteBatch.Draw(
                    texture: CurrentAnimation.Texture,
                    position: Position,
                    sourceRectangle: CurrentAnimation.CurrentSourceRectangle,
                    color: Color,
                    rotation: 0f,
                    origin: Origin,
                    scale: Scale,
                    effects: SpriteEffects.None,
                    layerDepth: 0f
                );
            }
        }
    }
}
