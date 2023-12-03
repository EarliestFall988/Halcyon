using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
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
