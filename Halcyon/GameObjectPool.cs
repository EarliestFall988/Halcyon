using Lib.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    /// <summary>
    /// Stores and handles the list of game objects in the game at a given time
    /// </summary>
    public sealed class GameObjectPool
    {

        /// <summary>
        /// the list of gameobjects
        /// </summary>
        public List<GameObject> GameObjectsToUpdate { get; private set; } = new List<GameObject>();

        /// <summary>
        /// the list of disabled objects
        /// </summary>
        private List<GameObject> DisabledObjects { get; set; } = new List<GameObject>();

        /// <summary>
        /// the list of game objects to call draw outside the existing sprite batch
        /// </summary>
        public List<GameObject> GameObjectsToDrawIndependently { get; private set; } = new List<GameObject>();

        private SpriteBatch _batch;

        /// <summary>
        /// the camera
        /// </summary>
        public Camera Camera { get; private set; }

        /// <summary>
        /// The instance of the game object pool
        /// </summary>
        public static GameObjectPool Main { get; private set; }

        public GameObjectPool(SpriteBatch batch, Camera camera)
        {
            if (Main != null)
                throw new Exception("There can only be on game object pool instance in the game");

            Main = this;

            _batch = batch;
            this.Camera = camera;
        }

        /// <summary>
        /// Get a list of all the objects
        /// </summary>
        public List<GameObject> AllObjects
        {
            get
            {
                var objects = new List<GameObject>();
                objects.AddRange(GameObjectsToUpdate);
                objects.AddRange(DisabledObjects);
                objects.AddRange(GameObjectsToDrawIndependently);
                return objects;
            }
        }


        /// <summary>
        /// Initialize the game object
        /// </summary>
        /// <typeparam name="T">the type of the gameobject</typeparam>
        /// <param name="obj">the object</param>
        /// <returns>returns the instantiated object</returns>
        /// <remarks>This object will spawn at (100,100) and have an origin point of (0,0)</remarks>
        public T SpawnObject<T>(T obj) where T : GameObject
        {
            return SpawnObject<T>(obj, new Vector2(100, 100), 0, Vector2.One * 100);
        }

        /// <summary>
        /// Initialize the game object
        /// </summary>
        /// <typeparam name="T">the object to initialize</typeparam>
        /// <param name="gameobject">the game object</param>
        /// <param name="position">the position of the gameobject</param>
        /// <param name="rotation">the rotation of the gameobject</param>
        /// <param name="batch">the sprite batch</param>
        /// <returns>returns the object</returns>
        public T SpawnObject<T>(T gameobject, Vector2 position, float rotation, Vector2 scale, bool drawIndependently = false) where T : GameObject
        {
            var transform = new Transform(position, scale, rotation, gameobject);
            gameobject.transform = transform;
            gameobject.Enabled = true;
            gameobject.Visible = true;

            if (!drawIndependently)
                gameobject.batch = _batch;

            gameobject.EnabledChanged += ObjectEnabledChanged;


            if (drawIndependently)
            {
                GameObjectsToDrawIndependently.Add(gameobject);
            }
            else
            {
                GameObjectsToUpdate.Add(gameobject);
            }

            return gameobject;
        }

        /// <summary>
        /// destroy the game object
        /// </summary>
        /// <typeparam name="T">
        /// the type of the game object
        /// </typeparam>
        /// <param name="gameobject">
        /// the game object to destroy
        /// </param>
        /// <returns>
        /// returns the destroyed game object
        /// </returns>
        public T Destroy<T>(T gameobject) where T : GameObject
        {
            gameobject.Enabled = false;
            gameobject.Visible = false;
            GameObjectsToUpdate.Remove(gameobject);
            return gameobject;
        }

        /// <summary>
        /// Clear the Game Object Pool
        /// </summary>
        /// <param name="removeStatic">
        /// remove the static reference to the game object pool
        /// </param>
        public void Clear(bool removeStatic = true)
        {
            for (int i = 0; i < AllObjects.Count; i++)
            {
                AllObjects[i].Enabled = false;
                AllObjects[i].Visible = false;
            }

            AllObjects.Clear();
            DisabledObjects.Clear();
            GameObjectsToUpdate.Clear();

            if (removeStatic)
            {
                Main = null;
            }
        }

        /// <summary>
        /// Remove the game objects that are disabled from the list of objects to be updated
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        public void ObjectEnabledChanged(object obj, EventArgs args)
        {
            if (obj != null && obj is GameObject o)
            {
                if (!o.Enabled)
                {
                    GameObjectsToUpdate.Remove(o);
                    DisabledObjects.Add(o);
                }
                else
                {
                    DisabledObjects.Remove(o);
                    GameObjectsToUpdate.Add(o);
                }
            }
        }
    }
}
