using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweening_Lib
{
    public class GameObjectPool
    {

        /// <summary>
        /// the list of gameobjects
        /// </summary>
        public List<GameObject> GameObjectsToUpdate { get; private set; } = new List<GameObject>();
        private List<GameObject> DisabledObjects { get; set; } = new List<GameObject>();

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
                return objects;
            }
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
        public T InitGameObject<T>(T gameobject, Vector2 position, float rotation, Vector2 scale, SpriteBatch batch) where T : GameObject
        {
            var transform = new Transform(position, scale, rotation, gameobject);
            gameobject.transform = transform;
            gameobject.Enabled = true;
            gameobject.Visible = true;
            gameobject.batch = batch;

            gameobject.EnabledChanged += ObjectEnabledChanged;

            GameObjectsToUpdate.Add(gameobject);

            return gameobject;
        }

        public T Destroy<T>(T gameobject) where T : GameObject
        {
            gameobject.Enabled = false;
            gameobject.Visible = false;
            GameObjectsToUpdate.Remove(gameobject);
            return gameobject;
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
