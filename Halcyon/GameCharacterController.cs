using Lib.GameObjectComponents;
using Lib.Utilities;
using Lib.WorldItems;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Dynamics;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lib
{
    public class GameCharacterController : GameObject
    {
        private float speedUpTime = 4f;
        private float breakSpeedTime = 4f;
        public float movementSpeed = 4;
        public float runSpeed = 7;

        private bool breatheIn = false;

        public float jumpHeight = 5;

        public float VelocityX = 0;
        public float VelocityY = 0;

        private bool grounded = false;
        private bool running = false;

        public SoundEffect RunningBreathe;
        private SoundEffectInstance runningSFXInstance;

        public List<SoundEffect> WalkingSoundEffects;
        public SoundEffect GroundedSoundEffect;

        private float walkSFXTime = 0.25f;
        private float walkSFXTimer = 0f;

        public Texture2D Atlas;

        private CharacterState _currentState;

        public SpriteEffects effect { get; set; } = SpriteEffects.None;
        public Dictionary<string, CharacterState> States { get; set; } = new Dictionary<string, CharacterState>();


        public GameCharacterController(Vector2 origin)
        {
            pool.SpawnObject(this, new Vector2(100, 100), 0, origin);


            //adding physics component to the character
            AddComponent<RigidBodyComponent>();
            var rbody = GetComponent<RigidBodyComponent>();
            rbody.body = rbody.world.CreateCapsule(1, 1, 1, bodyType: BodyType.Dynamic);

            rbody.Add(new nkast.Aether.Physics2D.Dynamics.Fixture(new CircleShape(transform.scaleValue * 128 / 2, 1)));
        }

        private void HorizontalMovement(GameTime time)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A) && !Keyboard.GetState().IsKeyDown(Keys.D))
            {

                VelocityX -= speedUpTime;

                effect = SpriteEffects.FlipHorizontally;
                if (grounded) _currentState = States["walk"];
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) && !Keyboard.GetState().IsKeyDown(Keys.A))
            {

                VelocityX += speedUpTime;

                effect = SpriteEffects.None;
                if (grounded) _currentState = States["walk"];
            }
            else
            {
                if (VelocityX > 0.125)
                    VelocityX -= breakSpeedTime;
                else if (VelocityX < -0.125)
                    VelocityX += breakSpeedTime;
                else
                    VelocityX = 0;

                if (grounded) _currentState = States["idle"];
            }

            if (!grounded) _currentState = States["jump"];

            VelocityX = MathHelper.Clamp(VelocityX, -1, 1);

            if (MathF.Abs(VelocityX) > 0.25f && grounded)
            {
                WalkSFX(time);
            }
        }

        private void VerticalMovment(GameTime time)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) && grounded)
            {
                VelocityY = -jumpHeight;
                grounded = false;
            }

            VelocityY += 9.8f;

            if (transform.position.Y >= 163 && VelocityY > 0)
            {
                VelocityY = 0;

                if (!grounded)
                    GroundedSoundEffect.Play();

                grounded = true;
            }

            //VelocityY = MathHelper.Clamp(VelocityY, -1, 1);
        }

        protected override void UpdateObject(GameTime time)
        {
            if (States.Count == 0)
                throw new Exception("States is empty");

            VerticalMovment(time);
            HorizontalMovement(time);

            if (grounded)
            {
                if (Math.Abs(VelocityX) <= 0.75f)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        _currentState = States["duck"];
                    }
                }
            }

            float finalHorizontalMovementSpeed = VelocityX * movementSpeed;

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Math.Abs(VelocityX) >= 0.98f)
            {
                finalHorizontalMovementSpeed = (VelocityX * runSpeed);
                transform.rotation = MathHelper.ToRadians(10) * VelocityX;

                if (!running)
                {
                    runningSFXInstance = RunningBreathe.CreateInstance();
                    runningSFXInstance.IsLooped = true;
                    runningSFXInstance.Play();
                }

                running = true;
            }
            else
            {
                if (runningSFXInstance != null)
                {
                    runningSFXInstance.Stop();
                    runningSFXInstance.Dispose();
                    runningSFXInstance = null;
                }

                running = false;
            }

            if (grounded && !running)
            {
                if (transform.rotation <= MathHelper.ToRadians(1))
                {
                    transform.rotation += MathHelper.ToRadians(30) * (float)time.ElapsedGameTime.TotalSeconds;
                }
                else if (transform.rotation >= MathHelper.ToRadians(1))
                {
                    transform.rotation -= MathHelper.ToRadians(30) * (float)time.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    transform.rotation = MathHelper.ToRadians(0);
                }
            }


            if (!grounded)
            {
                transform.rotation = MathHelper.ToRadians(15 * VelocityX);

            }

            if (Math.Abs(VelocityX) < 0.125f && MathF.Abs(VelocityY) < 0.125f)
            {
                if (transform.scaleValue >= 0.52f)
                {
                    breatheIn = false;
                }
                else if (transform.scaleValue <= 0.50f)
                {
                    breatheIn = true;
                }

                if (breatheIn)
                {
                    transform.scaleValue += 0.0225f * (float)time.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    transform.scaleValue -= 0.0225f * (float)time.ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                transform.scaleValue = 0.5f;
            }


            Vector2 finalVector = new Vector2(finalHorizontalMovementSpeed, VelocityY);
            //transform.Translate(finalVector);
            var rbody = GetComponent<RigidBodyComponent>();
            rbody.body.LinearVelocity = new nkast.Aether.Physics2D.Common.Vector2(finalVector.X, finalVector.Y);

            //Debug.WriteLine(rbody.body.Position.ToSystemVector2());
            transform.position = rbody.body.Position.ToSystemVector2();
        }

        protected override void DrawObject(GameTime time, Vector2 cameraPositionOffset, float rotationOffset)
        {
            if (Atlas == null)
                throw new Exception("Atlas is null");
            if (_currentState == null)
                return;

            _currentState.DrawObject(time, effect, Atlas, batch, this, cameraPositionOffset, rotationOffset);
        }

        private void WalkSFX(GameTime time)
        {
            walkSFXTimer += (float)time.ElapsedGameTime.TotalSeconds;

            if (walkSFXTimer >= walkSFXTime)
            {
                var walk = WalkingSoundEffects[new Random().Next(0, WalkingSoundEffects.Count)].CreateInstance();
                walk.Play();
                walkSFXTimer = 0;
            }
        }

        public static void SetupCharacter()
        {

        }
    }
}
