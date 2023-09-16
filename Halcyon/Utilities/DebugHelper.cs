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

        public bool ShowCollisions { get; set; } = true;

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
                    if (Collisions[i] is BoundingRectangle r)
                    {
                        _spriteBatch.Draw(debugDot, new Vector2(r.X, r.Y) + r.gameObject.transform.position - _camera.position, Color.Red);
                        _spriteBatch.Draw(debugDot, new Vector2(r.X + 128 / 2, r.Y) + r.gameObject.transform.position - _camera.position, Color.Orange);
                        _spriteBatch.Draw(debugDot, new Vector2(r.X + 128 / 2, r.Y + 128 / 2) + r.gameObject.transform.position - _camera.position, Color.Yellow);
                        _spriteBatch.Draw(debugDot, new Vector2(r.X, r.Y + 128 / 2) + r.gameObject.transform.position - _camera.position, Color.Purple);

                    }
                    else if (Collisions[i] is BoundingCircle c)
                    {
                        //do nothing for now
                        _spriteBatch.Draw(debugDot, c.Center - c.offset - _camera.position, Color.Magenta);
                        //Debug.WriteLine(c.Center - (c.gameObject.transform.position + c.gameObject.transform.origin));
                    }
                }
            }
        }
    }
}
