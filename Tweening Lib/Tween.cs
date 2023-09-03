using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweening_Lib
{

    /// <summary>
    /// Tween class for tweening values
    /// </summary>
    public static class Tween
    {
        private static List<TweenObject> objects = new List<TweenObject>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="duration"></param>
        /// <param name="gameTime"></param>
        public static TweenObject Value(string id, Action<float> func, float start, float end, float duration)
        {
            var obj = new TweenObject()
            {
                id = id,
                running = false,
                time = 0,
                value = 0,
                _end = end,
                _duration = duration,
                delegateValueToTween = func
            };

            objects.Add(obj);

            return obj;
        }

        public static void Update(GameTime time)
        {
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].running)
                {
                    objects[i].Update(time);
                }

                if (objects[i].stopped)
                {
                    objects.RemoveAt(i);
                }
            }
        }
    }
}
