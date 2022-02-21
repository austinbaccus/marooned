using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned
{
    /// <summary>
    /// Does not actually handle the drawing of sprites, but rather their animation.
    /// Instead, it keeps track of the current frame as well as the frameCount and sourceRectangles.
    /// To use this, do something like this:
    /// <code>
    /// Rectangle[] sourceRectangles = { new Rectangle(32 * 0 , 0, 32, 32), new Rectangle(32 * 1, 0, 32, 32), new Rectangle(32 * 2, 0, 32, 32) };
    /// Animation anim = new Animation(texture, sourceRectangles);
    /// </code>
    /// then, do <c>anim.Play()</c> at some point.
    /// Then, in your <c>Update()</c> function:
    /// <code>
    /// void Update(GameTime gameTime)
    /// {
    ///     ...
    ///     anim.Update(gameTime);
    ///     ...
    /// }
    /// </code>
    /// And finally, when drawing the sprite to the screen, pass in the <c>Texture</c> and <c>CurrentSourceRectangle</c> like this:
    /// <code>
    /// void Draw(SpriteBatch batch, ...)
    /// {
    ///     ...
    ///     batch.Draw(anim.Texture, anim.Position, anim.CurrentSourceRectangle, Color.White);
    ///     ...
    /// }
    /// </code>
    /// </summary>
    public class Animation
    {
        private double _timer;

        public Texture2D Texture { get; }
        public int CurrentFrame { get; set; } = 0;
        public int FrameCount { get; }
        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }
        // Lower is faster, higher is slower
        public double Speed { get; set; } = 0.1f;
        public bool IsLoop { get; set; } = true;
        public bool IsPlaying { get; private set; } = false;

        public Rectangle[] SourceRectangles { get; }
        public Rectangle CurrentSourceRectangle { get { return SourceRectangles[CurrentFrame]; } }

        // The length of sourceRectangles should be the number of frames (i.e., frameCount)
        public Animation(Texture2D texture, Rectangle[] sourceRectangles)
        {
            Texture = texture;
            SourceRectangles = sourceRectangles;
            FrameCount = sourceRectangles.Length;
            FrameWidth = SourceRectangles[0].Width;
            FrameHeight = SourceRectangles[0].Height;
        }

        public void Play()
        {
            if (IsPlaying) return;

            IsPlaying = true;
            CurrentFrame = 0;
            _timer = 0;
        }

        public void Resume()
        {
            if (IsPlaying) return;
            
            IsPlaying = true;
        }

        public void Pause()
        {
            if (!IsPlaying) return;
            
            IsPlaying = false;
        }

        public void Stop()
        {
            if (!IsPlaying) return;

            IsPlaying = false;
            CurrentFrame = 0;
            _timer = 0f;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (IsPlaying)
            {
                _timer += gameTime.ElapsedGameTime.TotalSeconds;

                if (_timer >= Speed)
                {
                    _timer = 0f;
                    CurrentFrame++;

                    if (CurrentFrame >= FrameCount)
                    {
                        if (IsLoop)
                        {
                            CurrentFrame = 0;
                        }
                        else
                        {
                            Stop();
                        }
                    }
                }
            }
        }
    }
}
