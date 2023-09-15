using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Collision
{
    public struct BoundingCircle
    {
        /// <summary>
        /// The center of the circle
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// The radius of the circle
        /// </summary>
        public float Radius;


        /// <summary>
        /// Constructs a new bounding circle
        /// </summary>
        /// <param name="center">the center of the circle</param>
        /// <param name="radius">the radius of the circle</param>
        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }


        /// <summary>
        /// test for a collision between two bounding circles
        /// </summary>
        /// <param name="other">the other collider</param>
        /// <returns>returns true if the collision was found, false if not</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);


        }

        /// <summary>
        /// test for a collision between a bounding circle and a bounding rectangle
        /// </summary>
        /// <param name="other">the other collider</param>
        /// <returns>returns true if the collision was found, false if not</returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }
    }
}
