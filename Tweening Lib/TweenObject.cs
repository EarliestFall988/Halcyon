using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tweening_Lib
{
    public class TweenObject
    {
        public string id { get; set; }
        public float time { get; set; }
        public float value { get; set; }

        public float _end { get; set; }
        public float _duration { get; set; }
        public Action<float> delegateValueToTween { get; set; }

        public bool running = false;
        public bool stopped = false;

        public TweenType type { get; set; }


        public TweenObject Tween(TweenType type)
        {

            this.type = type;
            return this;
        }

        public void Update(GameTime gameTime)
        {
            if (delegateValueToTween == null || !running)
            {
                return;
            }

            if (type == TweenType.EaseInOutCubic)
            {

                if (_end > 0)
                {
                    value = (float)(-(_end / 2) * (Math.Cos(Math.PI * time / _duration) - 1));

                }
                else
                {
                    value = (float)((_end / 2) * (Math.Cos(Math.PI * time / _duration) + 1));
                }
            }

            delegateValueToTween(value);
            time += 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (MathF.Abs(value) > MathF.Abs(_end))
            {
                value = _end;
                Cancel();
            }
        }

        /// <summary>
        /// Start the tween
        /// </summary>
        /// <returns></returns>
        public TweenObject Start()
        {
            running = true;
            return this;
        }

        /// <summary>
        /// Pause the tween
        /// </summary>
        /// <returns></returns>
        public TweenObject Pause()
        {
            running = false;
            return this;
        }


        /// <summary>
        /// cancel the tween and reset all values + turn the stopped flag on for cleanup
        /// </summary>
        /// <returns></returns>
        public TweenObject Cancel()
        {

            time = 0;
            value = 0;
            _end = 0;
            _duration = 0;
            delegateValueToTween = null;
            running = false;
            stopped = true;

            return this;
        }
    }


    /// <summary>
    /// The suppported tweening types
    /// </summary>
    public enum TweenType
    {
        EaseInOutlinear,
        EaseInOutCubic,
        EaseInOutQuad,
        EaseInOutQuart,
        EasInOutSine,
        EaseInOutExpo,
        EaseInOutCirc,
        EaseInOutElastic,
    }
}
