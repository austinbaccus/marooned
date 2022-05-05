using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Marooned.Controllers
{
    public delegate void OnKeyEvent(object sender, EventArgs e);

    public class KeyEventArgs : EventArgs
    {
        public Keys Key { get; set; }
        public GameContext GameContext { get; set; }
    }

    public class MouseEventArgs : EventArgs
    {
        public GameContext GameContext { get; set; }
    }

    public class InputController
    {
        #region Static Keys

        public static HashSet<Keys> UP_KEYS = new HashSet<Keys>() { Keys.W };
        public static HashSet<Keys> LEFT_KEYS = new HashSet<Keys>() { Keys.A };
        public static HashSet<Keys> DOWN_KEYS = new HashSet<Keys>() { Keys.S };
        public static HashSet<Keys> RIGHT_KEYS = new HashSet<Keys>() { Keys.D };
        public static HashSet<Keys> SHOOT_UP_KEYS = new HashSet<Keys>() { Keys.Up };
        public static HashSet<Keys> SHOOT_LEFT_KEYS = new HashSet<Keys>() { Keys.Left };
        public static HashSet<Keys> SHOOT_DOWN_KEYS = new HashSet<Keys>() { Keys.Down };
        public static HashSet<Keys> SHOOT_RIGHT_KEYS = new HashSet<Keys>() { Keys.Right };
        public static HashSet<Keys> FOCUS_KEYS = new HashSet<Keys>() { Keys.LeftShift };
        public static HashSet<Keys> CHEAT_KEYS = new HashSet<Keys>() { Keys.OemTilde };

        #endregion

        #region Keyboard Events

        public event EventHandler<KeyEventArgs> OnKeyPressEvent;
        public event EventHandler<KeyEventArgs> OnKeyDownEvent;
        public event EventHandler<KeyEventArgs> OnKeyUpEvent;

        #endregion

        #region Mouse Events

        public event EventHandler<MouseEventArgs> OnMouseButton1PressEvent;
        public event EventHandler<MouseEventArgs> OnMouseButton1DownEvent;
        public event EventHandler<MouseEventArgs> OnMouseButton1UpEvent;
        public event EventHandler<MouseEventArgs> OnMouseButton2PressEvent;
        public event EventHandler<MouseEventArgs> OnMouseButton2DownEvent;
        public event EventHandler<MouseEventArgs> OnMouseButton2UpEvent;

        #endregion

        private State _state;

        public InputController(State state)
        {
            _state = state;
        }

        public KeyboardState CurrentKeyboardState { get; private set; } = new KeyboardState();
        public KeyboardState PreviousKeyboardState { get; private set; } = new KeyboardState();
        public MouseState CurrentMouseState { get; private set; } = new MouseState();
        public MouseState PreviousMouseState { get; private set; } = new MouseState();

        #region Keyboard Event Invokers

        public void OnKeyPress(Keys key, GameContext gameContext)
        {
            OnKeyPressEvent?.Invoke(this, new KeyEventArgs()
            {
                Key = key,
                GameContext = gameContext,
            });
        }

        public void OnKeyDown(Keys key, GameContext gameContext)
        {
            OnKeyDownEvent?.Invoke(this, new KeyEventArgs()
            {
                Key = key,
                GameContext = gameContext,
            });
        }

        public void OnKeyUp(Keys key, GameContext gameContext)
        {
            OnKeyUpEvent?.Invoke(this, new KeyEventArgs()
            {
                Key = key,
                GameContext = gameContext,
            });
        }

        #endregion

        #region Mouse Event Invokers

        public void OnMouseButton1Press(GameContext gameContext)
        {
            OnMouseButton1PressEvent?.Invoke(this, new MouseEventArgs()
            {
                GameContext = gameContext,
            });
        }

        public void OnMouseButton1Down(GameContext gameContext)
        {
            OnMouseButton1DownEvent?.Invoke(this, new MouseEventArgs()
            {
                GameContext = gameContext,
            });
        }

        public void OnMouseButton1Up(GameContext gameContext)
        {
            OnMouseButton1UpEvent?.Invoke(this, new MouseEventArgs()
            {
                GameContext = gameContext,
            });
        }

        public void OnMouseButton2Press(GameContext gameContext)
        {
            OnMouseButton2PressEvent?.Invoke(this, new MouseEventArgs()
            {
                GameContext = gameContext,
            });
        }

        public void OnMouseButton2Down(GameContext gameContext)
        {
            OnMouseButton2DownEvent?.Invoke(this, new MouseEventArgs()
            {
                GameContext = gameContext,
            });
        }

        public void OnMouseButton2Up(GameContext gameContext)
        {
            OnMouseButton2UpEvent?.Invoke(this, new MouseEventArgs()
            {
                GameContext = gameContext,
            });
        }

        #endregion

        public void UpdateKeyboard(GameContext gameContext)
        {
            foreach (Keys key in CurrentKeyboardState.GetPressedKeys())
            {
                if (!PreviousKeyboardState.GetPressedKeys().Contains(key))
                {
                    OnKeyPress(key, gameContext);
                }
                OnKeyDown(key, gameContext);
            }

            foreach (Keys key in PreviousKeyboardState.GetPressedKeys())
            {
                if (!CurrentKeyboardState.GetPressedKeys().Contains(key))
                {
                    OnKeyUp(key, gameContext);
                }
            }
        }

        public void UpdateMouse(GameContext gameContext)
        {
            if (CurrentMouseState.LeftButton.HasFlag(ButtonState.Pressed))
            {
                if (!PreviousMouseState.LeftButton.HasFlag(ButtonState.Released))
                {
                    OnMouseButton1Press(gameContext);
                }
                OnMouseButton1Down(gameContext);
            }

            if (CurrentMouseState.LeftButton.HasFlag(ButtonState.Released))
            {

                OnMouseButton1Up(gameContext);
            }

            if (CurrentMouseState.RightButton.HasFlag(ButtonState.Pressed))
            {
                if (!PreviousMouseState.RightButton.HasFlag(ButtonState.Released))
                {
                    OnMouseButton2Press(gameContext);
                }
                OnMouseButton2Down(gameContext);
            }

            if (CurrentMouseState.RightButton.HasFlag(ButtonState.Released))
            {

                OnMouseButton2Up(gameContext);
            }
        }

        public void Update(GameContext gameContext)
        {
            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();

            UpdateKeyboard(gameContext);
            UpdateMouse(gameContext);

            PreviousKeyboardState = CurrentKeyboardState;
            PreviousMouseState = CurrentMouseState;
        }
    }
}
