
using Microsoft.Xna.Framework;

namespace Lib.Particle
{
    public interface IParticleEmitter
    {
        public Vector2 Position { get; }

        public Vector2 Velocity { get; }
    }
}
