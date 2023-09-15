using Lib.Collision;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
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
        public ICollision collider { get; set; }

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
            get => _enabledStore;
            set
            {
                if (value != _enabledStore)
                {
                    _enabledStore = value; EnabledChanged?.Invoke(this, new EventArgs());

                    if (value)
                        OnEnable();
                    else
                        OnDisable();

                    if (value && firstTimeEnabled)
                    {
                        firstTimeEnabled = false;
                        Start();
                    }

                }
            }
        }
        private bool _enabledStore = false;
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
            get => _visibleStore;
            set
            {
                if (value != _visibleStore)
                {
                    _visibleStore = value; VisibleChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        private bool _visibleStore = false;

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
        public static List<GameObject> FindObjectsWithTag(Tag tag, List<GameObject> objects)
        {
            List<GameObject> result = new List<GameObject>();

            foreach (GameObject obj in objects)
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
        public static List<GameObject> FindGameObjectsWithName(string name, List<GameObject> objects)
        {
            List<GameObject> result = new List<GameObject>();

            foreach (GameObject obj in objects)
            {
                if (obj.name == name)
                    result.Add(obj);
            }

            return result;
        }

        #endregion
    }
}
