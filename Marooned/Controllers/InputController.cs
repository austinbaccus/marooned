using System;
using System.Collections.Generic;
using System.Linq;
using Marooned.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Marooned.Controllers
{
    public delegate void OnKeyEvent(object sender, EventArgs e);

    public class KeyEventArgs : EventArgs
    {
        public Keys Key { get; set; }
        public GameTime GameTime { get; set; }
    }

    public class InputController
    {
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

        public event EventHandler<KeyEventArgs> OnKeyPressEvent;
        public event EventHandler<KeyEventArgs> OnKeyDownEvent;
        public event EventHandler<KeyEventArgs> OnKeyUpEvent;

        // TODO: Do we need this?
        private State _state;

        public InputController(State state)
        {
            _state = state;
        }

        public KeyboardState CurrentState { get; private set; } = new KeyboardState();
        public KeyboardState PreviousState { get; private set; } = new KeyboardState();

        public void OnKeyPress(Keys key, GameTime gameTime)
        {
            OnKeyPressEvent?.Invoke(this, new KeyEventArgs()
            {
                Key = key,
                GameTime = gameTime,
            });
        }

        public void OnKeyDown(Keys key, GameTime gameTime)
        {
            OnKeyDownEvent?.Invoke(this, new KeyEventArgs()
            {
                Key = key,
                GameTime = gameTime,
            });
        }

        public void OnKeyUp(Keys key, GameTime gameTime)
        {
            OnKeyUpEvent?.Invoke(this, new KeyEventArgs()
            {
                Key = key,
                GameTime = gameTime,
            });
        }

        public void Update(GameTime gameTime)
        {
            CurrentState = Keyboard.GetState();

            foreach (Keys key in CurrentState.GetPressedKeys())
            {
                if (!PreviousState.GetPressedKeys().Contains(key))
                {
                    OnKeyPress(key, gameTime);
                }
                OnKeyDown(key, gameTime);
            }

            foreach (Keys key in PreviousState.GetPressedKeys())
            {
                if (!CurrentState.GetPressedKeys().Contains(key))
                {
                    OnKeyUp(key, gameTime);
                }
            }

            PreviousState = CurrentState;
        }
    }
}
