using Lib.Particle;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Particle
{
    public class CoinSparkle : ParticleSystem
    {
        IParticleEmitter _emitter;

        public CoinSparkle(Game game, IParticleEmitter emitter) : base(game, 5)
        {
            _emitter = emitter;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "sparkle";

            minNumParticles = 2;
            maxNumParticles = 5;

            blendState = BlendState.AlphaBlend;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {

            var velocity = _emitter.Velocity + RandomHelper.NextDirection() * RandomHelper.NextFloat(0, 100);
            var acceleration = Vector2.UnitY * -400;

            var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);
            var angularVelocity = RandomHelper.NextFloat(-MathHelper.Pi, MathHelper.Pi);

            var scale = RandomHelper.NextFloat(0.01f, 0.125f);
            var lifetime = RandomHelper.NextFloat(0.1f, 0.5f);

            p.Initialize(where, velocity, acceleration, Color.LightGoldenrodYellow, scale: scale, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            AddParticles(_emitter.Position);
        }
    }
}
