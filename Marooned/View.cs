using System.Collections.Generic;
using Marooned.Sprites;
using Marooned.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled.Renderers;

namespace Marooned
{
    public abstract class View
    {
        protected State _state;

        public View(State state)
        {
            _state = state;
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }

    public class MapView : View
    {
        private readonly OrthographicCamera _camera;
        private readonly TiledMapRenderer _tiledMapRenderer;

        public MapView(State state, TiledMapRenderer tiledMapRenderer, OrthographicCamera camera) : base(state)
        {
            _tiledMapRenderer = tiledMapRenderer;
            _camera = camera;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, transformMatrix: _camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);

            _tiledMapRenderer.Draw(_camera.GetViewMatrix());

            foreach (var component in _state.GetComponents())
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }

    public class HUDView : View
    {
        private List<Sprite> _hearts;

        public HUDView(State state, List<Sprite> hearts) : base(state)
        {
            _hearts = hearts;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);

            foreach (Sprite heart in _hearts)
            {
                heart.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }

    public class InteractiveView : View
    {
        private readonly MapView _mapView;
        private readonly HUDView _hudView;

        public InteractiveView(State state, TiledMapRenderer tiledMapRender, OrthographicCamera camera, List<Sprite> hearts) : base(state)
        {
            _mapView = new MapView(state, tiledMapRender, camera);
            _hudView = new HUDView(state, hearts);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _mapView.Draw(gameTime, spriteBatch);
            _hudView.Draw(gameTime, spriteBatch);
        }
    }

    public class MenuView : View
    {
        public MenuView(State state) : base(state)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _state.GetComponents())
            {
                component.Draw(gameTime, spriteBatch);
            }
            
            spriteBatch.End();
        }
    }

    public class GameOverView : MenuView
    {
        public GameOverView(State state) : base(state)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            State backgroundState = ((GameOverState)_state).BackgroundState;

            backgroundState.View.Draw(gameTime, spriteBatch);

            spriteBatch.Begin();

            spriteBatch.FillRectangle(
                new RectangleF(0f, 0f, _state.graphicsDevice.Viewport.Width, _state.graphicsDevice.Viewport.Height),
                Color.Black * 0.5f
            );

            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}
