using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class GameCharacter : GameObject
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

        private float walkSFXTime = 0.25f;
        private float walkSFXTimer = 0f;
        public List<SoundEffect> WalkingSoundEffects;
        public SoundEffect GroundedSoundEffect;

        public Texture2D Atlas;

        private CharacterState _currentState;

        public SpriteEffects effect { get; set; } = SpriteEffects.None;
        public Dictionary<string, CharacterState> States { get; set; } = new Dictionary<string, CharacterState>();


        public GameCharacter(Vector2 origin)
        {
            pool.SpawnObject(this, new Vector2(100, 100), 0, origin);

        }

        private void HorizontalMovement(GameTime time)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A) && !Keyboard.GetState().IsKeyDown(Keys.D))
            {

                VelocityX -= speedUpTime * (float)time.ElapsedGameTime.TotalSeconds;

                effect = SpriteEffects.FlipHorizontally;
                if (grounded) _currentState = States["walk"];
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) && !Keyboard.GetState().IsKeyDown(Keys.A))
            {

                VelocityX += speedUpTime * (float)time.ElapsedGameTime.TotalSeconds;

                effect = SpriteEffects.None;
                if (grounded) _currentState = States["walk"];
            }
            else
            {
                if (VelocityX > 0.125)
                    VelocityX -= breakSpeedTime * (float)time.ElapsedGameTime.TotalSeconds;
                else if (VelocityX < -0.125)
                    VelocityX += breakSpeedTime * (float)time.ElapsedGameTime.TotalSeconds;
                else
                    VelocityX = 0;

                if (grounded) _currentState = States["idle"];
            }

            if (!grounded) _currentState = States["jump"];

            VelocityX = MathHelper.Clamp(VelocityX, -1, 1);

            if (MathF.Abs(VelocityX) > 0.25f)
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

            VelocityY += 9.8f * (float)time.ElapsedGameTime.TotalSeconds;

            if (transform.position.Y >= 163 && VelocityY > 0)
            {
                VelocityY = 0;
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
                running = true;
            }
            else
            {
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
            transform.Translate(finalVector);
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
    }

    public class CharacterState
    {

        public int currentFrame { get; set; } = 0;
        public float animatedSpriteSpeed { get; set; } = 0.125f;
        private float _animatedSpriteSpeedTime { get; set; } = 0;
        public Color color { get; set; } = Color.White;
        public List<Rectangle> frames { get; set; } = new List<Rectangle>();


        public void DrawObject(GameTime time, SpriteEffects effect, Texture2D atlas, SpriteBatch batch, GameObject gameObject, Vector2 cameraPositionOffset, float cameraRotationOffset)
        {
            if (atlas == null)
                throw new Exception("atlas is null");

            if (frames.Count == 0)
                throw new Exception("frames is empty");

            if (animatedSpriteSpeed <= 0)
                throw new Exception("animatedSpriteSpeed is less than or equal to 0");

            if (frames[currentFrame] == default)
                throw new Exception("the frames");

            if (batch == null)
                throw new Exception("batch is null");

            if (_animatedSpriteSpeedTime > animatedSpriteSpeed)
            {
                _animatedSpriteSpeedTime = 0;
                currentFrame++;
            }

            if (currentFrame >= frames.Count)
            {
                currentFrame = 0;
            }

            batch.Draw(atlas, gameObject.transform.position + gameObject.transform.origin - cameraPositionOffset, frames[currentFrame], color, gameObject.transform.rotation - cameraRotationOffset, gameObject.transform.origin, gameObject.transform.scaleValue, effect, gameObject.LayerValue);
            _animatedSpriteSpeedTime += (float)time.ElapsedGameTime.TotalSeconds;
        }
    }
}
