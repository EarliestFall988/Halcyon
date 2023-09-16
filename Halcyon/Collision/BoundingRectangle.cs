using Lib.Utilities;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Collision
{
    /// <summary>
    /// the bounding rectangle collider
    /// </summary>
    public struct BoundingRectangle : IGameObjectCollision
    {

        public float X;
        public float Y;
        public float Width;
        public float Height;

        public Vector2 position
        {
            get => new Vector2(X, Y);
            set
            {
                var x = value.X;
                var y = value.Y;

                X = x;
                Y = y;
            }
        }

        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;

        public Vector2 offset { get; set; } = Vector2.Zero;
        public float rotation { get; set; } = 0;
        public float scale { get; set; } = 0;

        public GameObject gameObject { get; private set; }

        public BoundingRectangle(float x, float y, float width, float height, GameObject obj)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            gameObject = obj;

            DebugHelper.Main.Collisions.Add(this); // adding the collision to the list of collisions for visual debugging
        }

        public BoundingRectangle(Vector2 position, float width, float height, GameObject obj)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
            gameObject = obj;

            DebugHelper.Main.Collisions.Add(this); // adding the collision to the list of collisions for visual debugging
        }


        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }


        /// <summary>
        /// test for a collision between a bounding circle and a bounding rectangle
        /// </summary>
        /// <param name="other">the other collider</param>
        /// <returns>returns true if the collision was found, false if not</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public void Init(Vector2 center, float width, float height, Transform transform)
        {
            Vector2 top = center + new Vector2(-width, height) / 2;
            X = top.X;
            Y = top.Y;
            Width = width;
            Height = height;

            offset = new Vector2(top.X - transform.position.X, top.Y - transform.position.Y);
        }

        public void Update(Transform transform)
        {
            X = transform.position.X + offset.X;
            Y = transform.position.Y + offset.Y;
        }
    }
}
