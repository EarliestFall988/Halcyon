using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Collision
{
    public class CollisionHelper
    {
        /// <summary>
        /// Detects collisions between two circles
        /// </summary>
        /// <param name="a">the first bounding circle</param>
        /// <param name="b">the second bouding circle</param>
        /// <returns>retunrs true if there is a collision, false if not</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >= Math.Pow(a.Center.X - b.Center.X, 2) + Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Detects collisions between two rectangles
        /// </summary>
        /// <param name="a">the first rectangle</param>
        /// <param name="b">the second rectangle</param>
        /// <returns>returns true if there is a collision, false if not</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right || a.Bottom < b.Top || a.Top > b.Bottom);
        }

        /// <summary>
        /// Detects a collision between a rectangle and circle
        /// </summary>
        /// <param name="a">the bounding circle</param>
        /// <param name="b">the bouding rectangle</param>
        /// <returns>returns true if there is a collision, false if not</returns>
        public static bool Collides(BoundingCircle a, BoundingRectangle b)
        {
            float nearestX = MathHelper.Clamp(a.Center.X, b.Left, b.Right);
            float nearestY = MathHelper.Clamp(a.Center.Y, b.Top, b.Bottom);

            return Math.Pow(a.Radius, 2) >= Math.Pow(a.Center.X - nearestX, 2) + Math.Pow(a.Center.Y - nearestY, 2);
        }


        public static bool Collides(BoundingRectangle a, BoundingCircle b) => Collides(b, a);
    }
}
