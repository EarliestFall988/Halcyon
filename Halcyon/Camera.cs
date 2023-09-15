using Lib.Utilities;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class Camera
    {

        private Vector2 _positionStore = Vector2.Zero;
        private float _rotationStore = 0;

        /// <summary>
        /// Camera Position
        /// </summary>
        public Vector2 position
        {
            get => _positionStore;

            set
            {
                _positionStore = value + new Vector2(lastIterationValueX, lastIterationValueY);
            }
        }

        /// <summary>
        /// Camera Rotation
        /// </summary>
        public float rotation
        {
            get => _rotationStore;
            set
            {
                _rotationStore = value + lastRotationValue;
            }
        }

        private OpenSimplexNoise noise = new OpenSimplexNoise();

        private double iterator = 0;
        float lastIterationValueX = 0;
        float lastIterationValueY = 0;
        float lastRotationValue = 0;

        public void HandheldCameraShake(GameTime time, float frequency = 1, float amount = 1)
        {
            float x = (float)noise.Evaluate(iterator, iterator) * amount * 5;
            float y = (float)noise.Evaluate(-iterator, -iterator) * amount * 5;
            float rotation = (float)noise.Evaluate(iterator, -iterator) * amount * 0.1f;

            _positionStore = new Vector2(_positionStore.X - lastIterationValueX + x, _positionStore.Y - lastIterationValueY + y);
            //_rotationStore = _rotationStore - lastRotationValue + rotation; <- need to revisit the rotation attribute later

            lastIterationValueX = x;
            lastIterationValueY = y;
            lastRotationValue = rotation;

            iterator += frequency * 0.10 * time.ElapsedGameTime.TotalSeconds;
        }
    }
}
