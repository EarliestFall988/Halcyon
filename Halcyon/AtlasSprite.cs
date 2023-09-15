using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    /// <summary>
    /// The atlas sprite class
    /// </summary>
    public class AtlasSprite : GameObject
    {
        /// <summary>
        /// the texture atlas
        /// </summary>
        public Texture2D Atlas;

        /// <summary>
        /// Paralax value
        /// </summary>
        public float Paralax = 2;

        /// <summary>
        /// the sprite type
        /// </summary>
        public SpriteType WorldType = SpriteType.Props;

        /// <summary>
        /// the sprite rect
        /// </summary>
        public Rectangle SpriteLocation;

        /// <summary>
        /// The sprite effect
        /// </summary>
        public SpriteEffects Effect = SpriteEffects.None;

        /// <summary>
        /// The color of the sprite
        /// </summary>
        public Color color = Color.White;

        /// <summary>
        /// Update the game object
        /// </summary>
        protected override void UpdateObject(GameTime time)
        {

        }

        /// <summary>
        /// The Draw object method
        /// </summary>
        protected override void DrawObject(GameTime time, Vector2 cameraPositionOffset, float rotationOffset)
        {
            if(WorldType == SpriteType.Forground)
            {
                cameraPositionOffset.X *= Paralax + 1;
                cameraPositionOffset.Y *= Paralax + 1;
            }

            if(WorldType == SpriteType.UI)
            {
                cameraPositionOffset = Vector2.Zero;
            }

            if (WorldType == SpriteType.Background)
            {
                cameraPositionOffset.X *= -Paralax + 1;
                cameraPositionOffset.Y *= -Paralax + 1;
            }

            batch.Draw(Atlas, transform.position + transform.origin - cameraPositionOffset, SpriteLocation, color, transform.rotation, transform.origin, transform.scaleValue, Effect, LayerValue);
        }
    }

    public enum SpriteType
    {
        UI,
        Forground,
        Background,
        Props
    }
}
