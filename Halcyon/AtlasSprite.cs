using Lib.GameObjectComponents;

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
        public float Paralax = 0.75f;

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

        //protected override void Start()
        //{
        //    base.Start();

        //    var rbody = GetComponent<RigidBodyComponent>();

            
        //    //{
        //    //    new Vector2(0, 0),
        //    //    new Vector2(SpriteLocation.Width, 0),
        //    //    new Vector2(SpriteLocation.Width, SpriteLocation.Height),
        //    //    new Vector2(0, SpriteLocation.Height)
        //    //};

        //    if(rbody != null)
        //    {
        //        rbody.Add(new nkast.Aether.Physics2D.Dynamics.Fixture(new nkast.Aether.Physics2D.Collision.Shapes.PolygonShape(verts, 1)));
        //    }
        //}

        /// <summary>
        /// The Draw object method
        /// </summary>
        protected override void DrawObject(GameTime time, Vector2 cameraPositionOffset, float rotationOffset)
        {
            if (WorldType == SpriteType.Forground)
            {
                cameraPositionOffset.X *= Paralax + 1;
                cameraPositionOffset.Y *= Paralax + 1;
            }

            if (WorldType == SpriteType.UI)
            {
                cameraPositionOffset = Vector2.Zero;
            }

            if (WorldType == SpriteType.Background)
            {
                cameraPositionOffset.X *= -Paralax + 1;
                cameraPositionOffset.Y *= -Paralax + 1;
            }

            var pos = transform.position + transform.origin - cameraPositionOffset;

            var rbody = GetComponent<RigidBodyComponent>();

            if (rbody != null)
            {
                rbody.SetPosition(new Vector2(pos.X, pos.Y));
            }



            batch.Draw(Atlas, pos, SpriteLocation, color, transform.rotation, transform.origin, transform.scaleValue, Effect, LayerValue);

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
