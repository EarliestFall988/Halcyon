using Lib.Utilities;

using Microsoft.Xna.Framework;

using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

using System;

namespace Lib.GameObjectComponents
{
    /// <summary>
    /// The Physics Body Component for the game object
    /// </summary>
    public class RigidBodyComponent : IGameObjectComponent
    {
        public GameObject gameObject { get; set; }

        public Body body { get; set; }

        public World world { get; private set; }

        public RigidBodyComponent(GameObject gameObject) // required for the activator to attach this to the game object
        {
            this.gameObject = gameObject;
            world = ScenesManager.Instance.GetCurrentWorld();

            if (gameObject.Enabled)
            {
                ToggleBody(true);
            }
            else
            {
                ToggleBody(false);
            }
        }

        public void OnDisable()
        {
            ToggleBody(false);
        }

        public void OnEnable()
        {
            ToggleBody(true);
        }

        private void ToggleBody(bool isEnabled)
        {
            if (!isEnabled && body != null)
            {
                //this.body.OnCollision -= OnCollision;
                //body.World.Remove(this.body);
            }

            if (isEnabled && body != null && world != null)
            {
                //world.Add(body); TODO: we should only add the body once in the world...
                //body.OnCollision += OnCollision;
            }
        }

        public void Update(GameTime time)
        {
            if (body != null && gameObject.transform != null)
            {
                this.gameObject.transform.position = body.Position.ToSystemVector2();
            }
            else
                throw new NullReferenceException("you have not defined the body for this game object");
        }

        /// <summary>
        /// Add a fixture to the body
        /// </summary>
        /// <param name="fixture">the fixture</param>
        public void Add(Fixture fixture)
        {
            if (body == null)
            {
                throw new Exception("body is null");
            }

            this.body.Add(fixture);
        }


        /// <summary>
        /// Set the position of the body
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            if (body == null)
                return;

            body.Position = new nkast.Aether.Physics2D.Common.Vector2(position.X, position.Y);
        }

        public void Draw()
        {

        }
    }
}
