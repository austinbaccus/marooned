using System;
using System.Collections.Generic;
using System.Text;
using Marooned.Sprites;
using Marooned.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled.Renderers;

namespace Marooned
{
    public interface View
    {
        public void Draw(State state, GameTime gameTime, SpriteBatch spriteBatch);
    }
    public class InteractiveView : View
    {
        public void Draw(State s, GameTime gameTime, SpriteBatch spriteBatch)
        {
            InteractiveState state = (InteractiveState)s;

            state.graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, transformMatrix: state.camera.Transform, samplerState: SamplerState.PointClamp);
            state.tiledMapRenderer.Draw(state.camera.CameraOrtho.GetViewMatrix());
            spriteBatch.End();

            // A second spriteBatch.Begin()/End() section is needed to render the player after the map has been rendered.
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, transformMatrix: state.camera.Transform, samplerState: SamplerState.PointClamp);

            // draw misc. components
            foreach (var component in state.components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            // draw bullets
            foreach (var bullet in state.player.BulletList)
            {
                bullet.Draw(gameTime, spriteBatch);
            }

            // draw grunts
            foreach (var grunt in state.grunts)
            {
                grunt.Draw(gameTime, spriteBatch);

                foreach (var bullet in grunt.BulletList)
                {
                    bullet.Draw(gameTime, spriteBatch);
                }
            }

            spriteBatch.End();

            // draw HUD
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);
            foreach (Sprite heart in state.hearts)
            {
                heart.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }
    }

    public class MenuView : View
    {
        public void Draw(State s, GameTime gameTime, SpriteBatch spriteBatch)
        {
            MenuState state = (MenuState)s;

            spriteBatch.Begin();

            foreach (var component in state.components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
