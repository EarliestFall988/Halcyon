using Lib.Particle;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Particle_System
{
    /// <summary>
    /// The coin sparkle emitter
    /// </summary>
    public class CoinSparkleEmitter : GameObject, IParticleEmitter
    {
        public Vector2 Position { get; set; } = Vector2.Zero;

        public Vector2 Velocity { get; set; } = Vector2.Zero;

        private Vector2 CameraPositionOffset = Vector2.Zero;

        CoinSparkle sparkle;

        public CoinSparkleEmitter(Game game)
        {
            sparkle = new CoinSparkle(game, this);
            sparkle.Load();
        }

        protected override void DrawObject(GameTime time, Vector2 cameraPositionOffset, float cameraRotationOffset)
        {
            if (!Enabled || !Visible)
                return;

            CameraPositionOffset = cameraPositionOffset;
            sparkle.Draw(time);
        }

        protected override void UpdateObject(GameTime time)
        {
            if (!Enabled)
                return;

            Position = transform.position;
            sparkle.CameraPosition = CameraPositionOffset;
            sparkle.Update(time);
        }
    }
}
