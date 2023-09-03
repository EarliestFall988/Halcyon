using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class Transform
    {
        /// <summary>
        /// the position of the object
        /// </summary>
        private Vector2 _positionStore { get; set; }

        /// <summary>
        /// The position of the object
        /// </summary>
        public Vector2 position
        {
            get => _positionStore;
            set
            {
                for (int i = 0; i < children.Count; i++)
                {
                    Vector2 offset = children[i].position - position;
                    children[i].position = value + offset;
                }

                _positionStore = value;
            }
        }

        /// <summary>
        /// The scale of the object
        /// </summary>
        public Vector2 rect { get; set; }

        /// <summary>
        /// the scale value
        /// </summary>
        public float scaleValue { get; set; } = 1;


        private float _localRotation = 0;

        /// <summary>
        /// the rotation of the object
        /// </summary>
        public float rotation
        {
            get
            {
                if (parent == null)
                    return _localRotation;
                else
                    return parent.rotation + _localRotation;
            }
            set
            {
                _localRotation = value;
            }
        }

        /// <summary>
        /// The gameobject the transform represents
        /// </summary>
        public GameObject gameObject { get; set; }


        /// <summary>
        /// The origin of the object
        /// </summary>
        public Vector2 localOrigin { get; set; } = Vector2.Zero;

        /// <summary>
        /// The origin of the object
        /// </summary>
        public Vector2 origin
        {
            get
            {
                if (parent == null)
                {
                    return localOrigin;
                }
                else
                {
                    return parent.origin + (parent.position - position);
                }
            }
        }

        /// <summary>
        /// The transform parent
        /// </summary>
        public Transform parent { get; private set; } = null;

        /// <summary>
        /// the children
        /// </summary>
        public List<Transform> children { get; private set; } = new List<Transform>();


        /// <summary>
        /// The transform constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rect"></param>
        /// <param name="rotation"></param>
        /// <param name="gameObject"></param>
        public Transform(Vector2 position, Vector2 origin, float rotation, GameObject gameObject)
        {
            this.position = position;
            this.rotation = rotation;
            this.gameObject = gameObject;
            localOrigin = origin;
        }


        /// <summary>
        /// transform constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rect"></param>
        /// <param name="gameObject"></param>
        public Transform(Vector2 position, Vector2 origin, GameObject gameObject)
        {
            this.position = position;
            this.rotation = 0;
            this.gameObject = gameObject;
            localOrigin = origin;
        }


        /// <summary>
        /// transform constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="gameObject"></param>
        public Transform(Vector2 position, GameObject gameObject)
        {
            this.position = position;
            this.rotation = 0;
            this.gameObject = gameObject;
            localOrigin = Vector2.Zero;
        }

        /// <summary>
        /// Translate the gameobject
        /// </summary>
        /// <param name="vector">the position</param>
        public void Translate(Vector2 vector)
        {
            position = position + vector;
        }

        /// <summary>
        /// rotate the gameobject
        /// </summary>
        /// <param name="newValue">the quaternion</param>
        public void Rotate(float newValue)
        {
            rotation = rotation + newValue;
        }

        /// <summary>
        /// Set the parent of the object
        /// </summary>
        /// <param name="parent">the object parent</param>
        public void SetParent(Transform parent)
        {
            if (this.parent != null)
            {
                this.parent.children.Remove(this);
            }

            this.parent = parent;
            parent.children.Add(this);
        }


        /// <summary>
        /// remove the parent of the transform
        /// </summary>
        public void RemoveParent()
        {
            if (parent != null)
            {
                parent.children.Remove(this);
            }

            parent = null;
        }

        ///// <summary>
        ///// provide a direction that the transform is facing
        ///// </summary>
        ///// <returns>returns a vector2 </returns>
        //public Vector2 Forward()
        //{
        //    return Vector2.Transform(Vector2.UnitY, rotation);
        //}
    }
}
