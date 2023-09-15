﻿using Microsoft.Xna.Framework;

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
    public struct BoundingRectangle : ICollision
    {

        public float X;
        public float Y;
        public float Width;
        public float Height;

        Vector2 position
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

        public BoundingRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

        }

        public BoundingRectangle(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
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
    }
}