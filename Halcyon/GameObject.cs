using Lib.Collision;
using Lib.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    /// <summary>
    /// An abstract class representing a Game Object
    /// </summary>
    public abstract class GameObject : IUpdateable, IDrawable
    {
        #region props
        /// <summary>
        /// the name (or a user-defined id) of the object
        /// </summary>
        public string name { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The tag of the object
        /// </summary>
        public Tag tag { get; set; }

        /// <summary>
        /// The transform of the object
        /// </summary>
        public Transform transform { get; set; }

        /// <summary>
        /// the collider the game object has
        /// </summary>
        public List<IGameObjectCollision> colliders { get; set; } = new List<IGameObjectCollision>();

        /// <summary>
        /// the sprite batch
        /// </summary>
        public SpriteBatch batch { get; set; }

        /// <summary>
        /// The layer value
        /// </summary>
        public int LayerValue { get; set; } = 0;

        /// <summary>
        /// The main game object pool
        /// </summary>
        public GameObjectPool pool => GameObjectPool.Main;

        /// <summary>
        /// The camera class
        /// </summary>
        public Camera camera => pool.Camera;

        /// <summary>
        /// Is this object enabled?
        /// </summary>
        public bool Enabled
        {
            get => m_enabled;
            set
            {
                if (value != m_enabled)
                {
                    m_enabled = value; EnabledChanged?.Invoke(this, new EventArgs());

                    if (value)
                        OnEnable();
                    else
                        OnDisable();

                    foreach (var x in components)
                    {
                        if (value)
                            x.OnEnable();
                        else
                            x.OnDisable();
                    }

                    if (value && firstTimeEnabled)
                    {
                        firstTimeEnabled = false;
                        Start();
                    }

                    foreach (var x in transform.children)
                    {
                        x.gameObject.SetActive(value);
                    }



                }
            }
        }
        private bool m_enabled = false;
        private bool firstTimeEnabled = false;

        [Obsolete("not being used at this time")]
        public int UpdateOrder { get; set; }

        [Obsolete("not being implemented in the class at this time")]
        public int DrawOrder
        {
            get; set;
        }

        /// <summary>
        /// is this object visible?
        /// </summary>
        public bool Visible
        {
            get => m_visible;
            set
            {
                if (value != m_visible)
                {
                    m_visible = value; VisibleChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        private bool m_visible = false;

        #endregion

        #region events

        /// <summary>
        /// This object is now enabled/disabled
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;
        [Obsolete("not implemented at this time")]
        public event EventHandler<EventArgs> UpdateOrderChanged;

        [Obsolete("not implemented at this time")]
        public event EventHandler<EventArgs> DrawOrderChanged;

        /// <summary>
        /// This object is now visible/invisible
        /// </summary>
        public event EventHandler<EventArgs> VisibleChanged;

        #endregion

        #region interface implementations 


        /// <summary>
        /// Update the game object
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {

            if (!Enabled)
                return;

            foreach (var x in components)
            {
                x.Update(gameTime);
            }

            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].Update(transform);
            }

            UpdateObject(gameTime);
        }

        /// <summary>
        /// update the game object
        /// </summary>
        protected abstract void UpdateObject(GameTime time);

        /// <summary>
        /// draw the game object
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            if (!Visible || !Enabled)
                return;

            foreach (var x in components)
            {
                x.Draw();
            }

            DrawObject(gameTime, camera.position, camera.rotation);
        }

        /// <summary>
        /// Draw the game object
        /// </summary>
        protected abstract void DrawObject(GameTime time, Vector2 cameraPositionOffset, float cameraRotationOffset);

        #endregion

        #region state based triggers

        /// <summary>
        /// Triggered when the object is enabled for the first time, initalizes the object
        /// </summary>
        protected virtual void Start()
        {
            transform = new Transform(transform.position, transform.rect, transform.rotation, this);
        }

        /// <summary>
        /// triggered when the object is enabled
        /// </summary>
        protected virtual void OnEnable()
        {
            //do nothing (can extend later)

        }

        /// <summary>
        /// triggered when the object is disbled
        /// </summary>
        protected virtual void OnDisable()
        {
            //do nothing (can extend later)
        }

        /// <summary>
        /// set the object active or inactive
        /// </summary>
        /// <param name="active">object active?</param>
        public void SetActive(bool active)
        {
            Enabled = active;
            Visible = active;
        }

        #endregion

        #region static methods


        /// <summary>
        /// find gameobjects with a specific tag
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static List<GameObject> FindObjectsWithTag(Tag tag)
        {
            List<GameObject> result = new List<GameObject>();

            foreach (GameObject obj in GameObjectPool.Main.AllObjects)
            {
                if (obj.tag == tag)
                    result.Add(obj);
            }

            return result;
        }

        /// <summary>
        /// Find the list of gameobjects with a specific name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static List<GameObject> FindGameObjectsWithName(string name)
        {

            List<GameObject> result = new List<GameObject>();

            foreach (GameObject obj in GameObjectPool.Main.AllObjects)
            {
                if (obj.name == name)
                    result.Add(obj);
            }

            return result;
        }


        /// <summary>
        /// Find the list of gameobjects with a specific game object component
        /// </summary>
        /// <typeparam name="T">the type of gameobject component</typeparam>
        /// <returns></returns>
        public static List<GameObject> FindGameObjectsWithComponent<T>() where T : class, IGameObjectComponent
        {
            List<GameObject> result = new List<GameObject>();

            foreach (var obj in GameObjectPool.Main.AllObjects)
            {
                if (obj.GetComponent<T>() != null)
                    result.Add(obj);
            }

            return result;
        }

        #endregion




        #region Component Architecture

        /// <summary>
        /// the hashset of components
        /// </summary>
        private HashSet<IGameObjectComponent> components { get; set; } = new HashSet<IGameObjectComponent>();

        /// <summary>
        /// Get the component of the game object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : class, IGameObjectComponent
        {
            foreach (var x in components)
            {
                if (x is T)
                {
                    return (T)x;
                }
            }

            return null;
        }

        /// <summary>
        /// Add the component to the game object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T AddComponent<T>() where T : class, IGameObjectComponent
        {
            if (GetComponent<T>() != null)
                return GetComponent<T>();

            var component = (IGameObjectComponent)Activator.CreateInstance(typeof(T), this);
            components.Add(component);
            return component as T;
        }

        public void RemoveComponent<T>() where T : class, IGameObjectComponent
        {
            if (GetComponent<T>() == null)
                return;

            components.Remove(GetComponent<T>());
        }

        /// <summary>
        /// Get the components of the game object
        /// </summary>
        public IGameObjectComponent[] GetComponents => components.ToArray();

        #endregion

    }
}
