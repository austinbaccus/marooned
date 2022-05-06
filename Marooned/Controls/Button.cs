using Marooned.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Marooned.Controls
{
    public class Button : DrawableGameComponent
    {
        #region Fields

        private SpriteFont _font;

        private bool _isHovering;

        private Texture2D _texture;

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; }

        public Vector2 Position { get; set; }

        public State State { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public string Text { get; set; }

        #endregion

        #region Methods

        public Button(State state, Texture2D texture, SpriteFont font) : base(state.GameContext.Game)
        {
            State = state;

            _texture = texture;

            _font = font;

            PenColour = Color.Black;

            State.InputController.OnMouseButton1PressEvent += OnMouseButton1Press;
        }

        public override void Draw(GameTime gameTime)
        {
            State.GameContext.SpriteBatch.Begin();

            var color = _isHovering ? Color.Gray : Color.White;

            State.GameContext.SpriteBatch.Draw(_texture, Rectangle, color);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                State.GameContext.SpriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }

            State.GameContext.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public bool IsMouseIntersecting()
        {
            var currentMouse = State.InputController.CurrentMouseState;
            var mouseRectangle = new Rectangle(currentMouse.Position.X, currentMouse.Position.Y, 1, 1);
            return mouseRectangle.Intersects(Rectangle);
        }

        public override void Update(GameTime gameTime)
        {
            _isHovering = IsMouseIntersecting();
            base.Update(gameTime);
        }

        public void OnMouseButton1Press(object sender, MouseEventArgs mouseEventArgs)
        {
            if (IsMouseIntersecting())
            {
                Click?.Invoke(this, new EventArgs());
            }
        }

        protected override void Dispose(bool disposing)
        {
            State.InputController.OnMouseButton1PressEvent -= OnMouseButton1Press;

            base.Dispose(disposing);
        }

        #endregion
    }
}
