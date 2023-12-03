using Lib.Utilities;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Collision
{
    /// <summary>
    /// Bounding circle collider
    /// </summary>
    public class BoundingCircle : IGameObjectCollision
    {

        private Vector2 _centerStore;

        /// <summary>
        /// The center of the circle
        /// </summary>
        public Vector2 Center
        {
            get => _centerStore;
            set
            {
                _centerStore = value;
            }
        }


        private float _radius = 1;

        /// <summary>
        /// The radius of the circle
        /// </summary>
        public float Radius => _radius;


        public Vector2 offset { get; set; } = Vector2.Zero;
        public float rotation { get; set; } = 0;
        public float scale { get; set; } = 0;
        public GameObject gameObject { get; private set; }


        /// <summary>
        /// Constructs a new bounding circle
        /// </summary>
        /// <param name="center">the center of the circle</param>
        /// <param name="radius">the radius of the circle</param>
        public BoundingCircle(Vector2 center, float radius, GameObject obj)
        {

            offset = center - obj.transform.position + obj.transform.origin;

            _centerStore = obj.transform.position + offset;
            _radius = radius * obj.transform.scaleValue;
            gameObject = obj;

            Debug.WriteLine($"radius ({obj.tag.name}): " + _radius);
            if (DebugHelper.Main != null && DebugHelper.Main.Collisions != null)
                DebugHelper.Main.Collisions.Add(this); // this is for debugging purposes
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

        public void Init(Vector2 center, float width, float height, Transform transform)
        {
            if (width != height)
            {
                Debug.WriteLine("the width and height are not the same - grabbing width");
            }


            //Center = center;
            //_radius = width / 2;

            //offset = transform.position - center;
            //rotation = transform.rotation; //careful
            //scale = transform.scaleValue;
        }

        public void Update(Transform transform)
        {
            _centerStore = transform.position + offset;
            //_centerStore = transform.position;
            //Debug.WriteLine(transform.position + offset);
            //Debug.WriteLine(_centerStore + offset);
        }
    }
}
