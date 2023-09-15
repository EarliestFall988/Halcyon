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

        public GameObject TargetCharacter { get; set; }
        public Vector2 CharacterCameraOffset { get; set; }

        private OpenSimplexNoise noise = new OpenSimplexNoise();

        public bool HandheldCameraShakeEnabled { get; set; } = false;
        public float HandheldCameraShakeFrequency { get; set; } = 1;
        public float HandheldCameraShakeAmount { get; set; } = 1;

        private double iterator = 0;
        float lastIterationValueX = 0;
        float lastIterationValueY = 0;
        float lastRotationValue = 0;


        public void UpdateCamera(GameTime time)
        {
            if (TargetCharacter != null)
            {
                position = TargetCharacter.transform.position + CharacterCameraOffset;
            }

            if (HandheldCameraShakeEnabled)
                HandheldCameraShake(time);
        }

        private void HandheldCameraShake(GameTime time)
        {
            float x = (float)noise.Evaluate(iterator, iterator) * HandheldCameraShakeAmount * 5;
            float y = (float)noise.Evaluate(-iterator, -iterator) * HandheldCameraShakeAmount * 5;
            float rotation = (float)noise.Evaluate(iterator, -iterator) * HandheldCameraShakeAmount * 0.1f;

            _positionStore = new Vector2(_positionStore.X - lastIterationValueX + x, _positionStore.Y - lastIterationValueY + y);
            //_rotationStore = _rotationStore - lastRotationValue + rotation; <- need to revisit the rotation attribute later

            lastIterationValueX = x;
            lastIterationValueY = y;
            lastRotationValue = rotation;

            iterator += HandheldCameraShakeFrequency * 0.10 * time.ElapsedGameTime.TotalSeconds;
        }
    }
}
