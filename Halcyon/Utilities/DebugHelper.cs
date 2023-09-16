using Lib.Collision;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Utilities
{

    /// <summary>
    /// Provides utilities to aid in the debugging process
    /// </summary>
    public class DebugHelper : IDrawable
    {
        public Texture2D debugBox { get; private set; }
        public Texture2D debugCircle { get; private set; }
        public Texture2D debugDot { get; set; }

        public bool ShowCollisions { get; set; } = false;

        private SpriteBatch _spriteBatch;

        public static DebugHelper Main { get; private set; }

        public int DrawOrder => 100;

        public bool Visible => true;

        public List<IGameObjectCollision> Collisions = new List<IGameObjectCollision>();

        private Camera _camera;

        public DebugHelper(Texture2D debugBox, Texture2D debugCircle, Texture2D DebugDot, SpriteBatch batch, Camera cameraControls)
        {

            if (Main != null)
                throw new Exception("There can only be one instance of the debug helper");

            Main = this;

            this.debugBox = debugBox;
            this.debugCircle = debugCircle;
            this.debugDot = DebugDot;
            _spriteBatch = batch;
            _camera = cameraControls;
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public void Draw(GameTime gameTime)
        {
            if (ShowCollisions)
            {
                for (int i = 0; i < Collisions.Count; i++)
                {
                    if (!Collisions[i].gameObject.Enabled)
                        continue;

                    if (Collisions[i] is BoundingRectangle r)
                    {
                        _spriteBatch.Draw(debugDot, new Vector2(r.X, r.Y) - _camera.position, Color.Red);
                        _spriteBatch.Draw(debugDot, new Vector2(r.X + r.Width, r.Y) - _camera.position, Color.Orange);
                        _spriteBatch.Draw(debugDot, new Vector2(r.X + r.Width, r.Y + r.Height) - _camera.position, Color.Yellow);
                        _spriteBatch.Draw(debugDot, new Vector2(r.X, r.Y + r.Height) - _camera.position, Color.Purple);

                    }
                    else if (Collisions[i] is BoundingCircle c)
                    {
                        _spriteBatch.Draw(debugDot, c.Center - _camera.position, Color.Magenta);
                        //for (double k = 0; k <= 2 * Math.PI; k += 0.1)
                        //{
                        _spriteBatch.Draw(debugDot, c.Center + new Vector2(-c.Radius, -c.Radius) - _camera.position, Color.Magenta);
                        _spriteBatch.Draw(debugDot, c.Center + new Vector2(-c.Radius, c.Radius) - _camera.position, Color.Magenta);
                        _spriteBatch.Draw(debugDot, c.Center + new Vector2(c.Radius, -c.Radius) - _camera.position, Color.Magenta);
                        _spriteBatch.Draw(debugDot, c.Center + new Vector2(c.Radius, c.Radius) - _camera.position, Color.Magenta);
                        //}
                        //_spriteBatch.Draw(debugDot, c.Center + new Vector2(c.Radius * (float)Math.Cos(2 * MathHelper.Pi / 4), -c.Radius * (float)Math.Sin(2 * MathHelper.Pi / 4)) - _camera.position, Color.Magenta);
                        //_spriteBatch.Draw(debugDot, c.Center + new Vector2(-c.Radius * (float)Math.Cos(3 * MathHelper.Pi / 4), c.Radius * (float)Math.Sin( 3 * MathHelper.Pi / 4)) - _camera.position, Color.Magenta);
                        //_spriteBatch.Draw(debugDot, c.Center + new Vector2(-c.Radius * (float)Math.Cos(4 * MathHelper.Pi / 4), -c.Radius * (float)Math.Sin(4 * MathHelper.Pi / 4)) - _camera.position, Color.Magenta);

                    }
                }
            }
        }
    }
}
