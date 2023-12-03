using Lib.GameObjectComponents;
using Lib.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.WorldItems
{
    /// <summary>
    /// The Character Controller that works with Physics engine
    /// </summary>
    public sealed class PhysicsCharacterController : GameObject
    {
        RigidBodyComponent m_rbody;
        private const float c_fixAmt = 10000f;
        public Vector2 PlayerDirection { get; private set; }

        private float gravityAmt = 0;

        public Texture2D Atlas { get; set; }
        private CharacterState _currentState;
        public SpriteEffects effect { get; set; } = SpriteEffects.None;
        public Dictionary<string, CharacterState> States { get; set; } = new Dictionary<string, CharacterState>();

        bool grounded = false;

        #region audio

        public SoundEffect RunningBreathe;
        private SoundEffectInstance runningSFXInstance;

        public List<SoundEffect> WalkingSoundEffects;
        public SoundEffect GroundedSoundEffect;

        #endregion  


        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="origin"></param>
        public PhysicsCharacterController(Vector2 origin)
        {

            pool.SpawnObject(this, new Vector2(100, 100), 0, origin);

            m_rbody = AddComponent<RigidBodyComponent>();
            m_rbody.body = m_rbody.world.CreateRectangle(128, 256, 0, bodyType: BodyType.Dynamic);
            m_rbody.body.OnCollision += OnCollision;
            m_rbody.body.LinearDamping = 0f;
            m_rbody.body.FixedRotation = false;
        }

        protected override void DrawObject(GameTime time, Vector2 cameraPositionOffset, float cameraRotationOffset)
        {
            if (Atlas == null)
                throw new NullReferenceException("The texture atlas is null");

            if (_currentState == null)
                _currentState = States["idle"];

            _currentState.DrawObject(time, effect, Atlas, batch, this, cameraPositionOffset, cameraRotationOffset);
        }

        protected override void UpdateObject(GameTime time)
        {
            var x = HorizontalMovement();
            VerticalMovement();

            var finalPlayerVelocity = new Vector2(x, 0).ToAetherVector2() * c_fixAmt * 500000f;

            Debug.WriteLine("final velocity:" + finalPlayerVelocity);

            m_rbody.body.ApplyForce(finalPlayerVelocity);

            m_rbody.body.IgnoreGravity = true;
        }

        /// <summary>
        /// Character Horizontal Movement
        /// </summary>
        /// <param name="time"></param>
        private float HorizontalMovement()
        {

            if (m_rbody == null)
                return 0;


            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                return -1;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                return 1;
            }


            return 0;
        }

        private void VerticalMovement()
        {
            if (m_rbody == null)
                return;


            if (grounded)
                gravityAmt = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.W) && grounded)
            {
                m_rbody.body.ApplyLinearImpulse(new nkast.Aether.Physics2D.Common.Vector2(0, -50000000));
                gravityAmt = -5000000;
                grounded = false;
                return;
            }

            Debug.WriteLine(gravityAmt);

            gravityAmt += 10000;

            //m_rbody.body.ApplyForce(new nkast.Aether.Physics2D.Common.Vector2(0, gravityAmt += 10000));

        }

        public bool OnCollision(Fixture f, Fixture other, Contact contact)
        {

            grounded = true;

            return true;
        }
    }
}
