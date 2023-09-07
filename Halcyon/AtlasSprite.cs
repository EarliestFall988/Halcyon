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
        protected override void DrawObject(GameTime time)
        {
            batch.Draw(Atlas, transform.position + transform.origin, SpriteLocation, color, transform.rotation, transform.origin, transform.scale, Effect, LayerValue);
        }
    }
}
