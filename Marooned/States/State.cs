﻿using System.Collections.Generic;
using Marooned.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Marooned.States
{
    // TODO: Use MonoGame.Extended's Screen Management instead (?)
    public abstract class State
    {
        #region Fields

        public View View { get; protected set; }

        public ContentManager content;

        public GraphicsDevice graphicsDevice;

        public Game1 game;

        public InputController inputController;

        #endregion

        #region Methods

        public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            this.game = game;

            this.graphicsDevice = graphicsDevice;

            this.content = content;

            inputController = new InputController(this);
        }

        public abstract void PostUpdate(GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public abstract List<ComponentOld> GetComponents();

        #endregion
    }
}
