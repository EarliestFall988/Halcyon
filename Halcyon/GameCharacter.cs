using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class GameCharacter : GameObject
    {
        private float lerpTime = 1;
        public float speed = 5;

        public float jumpHeight;

        public float VelocityX = 0;
        public float VelocityY = 0;


        public Texture2D Atlas;

        private CharacterState _currentState;

        public SpriteEffects effect { get; set; } = SpriteEffects.None;
        public Dictionary<string, CharacterState> States { get; set; } = new Dictionary<string, CharacterState>();

        public GameCharacter()
        {
            pool.SpawnObject(this);
        }

        protected override void UpdateObject(GameTime time)
        {
            if (States.Count == 0)
                throw new Exception("States is empty");

            if (Keyboard.GetState().IsKeyDown(Keys.A) && !Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (VelocityX > -1)
                    VelocityX -= lerpTime * (float)time.ElapsedGameTime.TotalSeconds;

                effect = SpriteEffects.FlipHorizontally;
                _currentState = States["walk"];
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) && !Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (VelocityX < 1)
                    VelocityX += lerpTime * (float)time.ElapsedGameTime.TotalSeconds;

                effect = SpriteEffects.None;
                _currentState = States["walk"];
            }
            else
            {
                if (VelocityX > 0)
                    VelocityX -= lerpTime * (float)time.ElapsedGameTime.TotalSeconds;
                else if (VelocityX < 0)
                    VelocityX += lerpTime * (float)time.ElapsedGameTime.TotalSeconds;

                _currentState = States["idle"];
            }

            var finalVector = new Vector2(VelocityX, VelocityY);
            finalVector.Normalize();

            transform.Translate(finalVector * speed);
        }

        protected override void DrawObject(GameTime time)
        {
            if (Atlas == null)
                throw new Exception("Atlas is null");
            if (_currentState == null)
                throw new Exception("Current state is null");

            _currentState.DrawObject(time, effect, Atlas, batch, this);
        }
    }

    public class CharacterState
    {

        public int currentFrame { get; set; } = 0;
        public float animatedSpriteSpeed { get; set; } = 0.125f;
        private float _animatedSpriteSpeedTime { get; set; } = 0;
        public Color color { get; set; } = Color.White;
        public List<Rectangle> frames { get; set; } = new List<Rectangle>();


        public void DrawObject(GameTime time, SpriteEffects effect, Texture2D atlas, SpriteBatch batch, GameObject gameObject)
        {
            if (atlas == null)
                throw new Exception("atlas is null");

            if (frames.Count == 0)
                throw new Exception("frames is empty");

            if (animatedSpriteSpeed <= 0)
                throw new Exception("animatedSpriteSpeed is less than or equal to 0");

            if (frames[0] == default)
                throw new Exception("the frames");

            if (batch == null)
                throw new Exception("batch is null");

            if (_animatedSpriteSpeedTime > animatedSpriteSpeed)
            {
                _animatedSpriteSpeedTime = 0;
                currentFrame++;

                if (currentFrame >= frames.Count)
                {
                    currentFrame = 0;
                }

                batch.Draw(atlas, gameObject.transform.position + gameObject.transform.origin, frames[currentFrame], color, gameObject.transform.rotation, gameObject.transform.origin, gameObject.transform.scaleValue, effect, gameObject.LayerValue);

                _animatedSpriteSpeedTime += (float)time.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
