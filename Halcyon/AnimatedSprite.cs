using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

namespace Lib
{
    /// <summary>
    /// The animated sprite class
    /// </summary>
    public class AnimatedSprite : GameObject
    {
        /// <summary>
        /// the animation speed
        /// </summary>
        public float animatedSpriteSpeed { get; set; } = 0.125f;

        /// <summary>
        /// the animated sprite speed
        /// </summary>
        private float _animatedSpriteSpeedTime = 0;

        /// <summary>
        /// the current frame
        /// </summary>
        public int CurrentFrame { get; private set; } = 0;

        /// <summary>
        /// the texture to use
        /// </summary>
        public Texture2D Atlas { get; set; }

        /// <summary>
        /// the list of frames to play (in order starting from 0)
        /// </summary>
        public List<Rectangle> frames { get; set; } = new List<Rectangle>();

        /// <summary>
        /// The sprite transform effect
        /// </summary>
        public SpriteEffects effect { get; set; } = SpriteEffects.None;

        /// <summary>
        /// the color
        /// </summary>
        public Color color { get; set; } = Color.White;

        /// <summary>
        /// the update action
        /// </summary>
        public Action<double> UpdateSprite { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AnimatedSprite(Vector2 origin)
        {
            pool.SpawnObject<AnimatedSprite>(this, new Vector2(100, 100), 0, origin); //<- might need to think about what this means if trying to combine multiple sprites...
        }

        protected override void DrawObject(GameTime time, Vector2 cameraPositionOffset, float cameraRotationOffset)
        {
            if (Atlas == null)
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
                CurrentFrame++;

                if (CurrentFrame >= frames.Count)
                {
                    CurrentFrame = 0;
                }
            }

            batch.Draw(Atlas, transform.position + transform.origin, frames[CurrentFrame], color, transform.rotation, transform.origin, transform.scaleValue, effect, LayerValue);

            _animatedSpriteSpeedTime += (float)time.ElapsedGameTime.TotalSeconds;
        }

        protected override void UpdateObject(GameTime time)
        {

            if (UpdateSprite != null)
                UpdateSprite(time.ElapsedGameTime.TotalSeconds); // handle the custom update function for this sprite object
        }
    }
}
