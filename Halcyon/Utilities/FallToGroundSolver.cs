using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Utilities
{
    public class FallToGroundSolver
    {

        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }

        public float bufferPercent { get; set; } = 0.1f;
        public float upDownFudge { get; set; } = 0.1f;

        public FallToGroundSolver(Vector2 start, Vector2 end)
        {

            if (start.X > end.X)
            {
                var temp = end; end = start; start = temp;
            }

            Start = start;
            End = end;
        }

        /// <summary>
        /// Check to see if the player is falling down
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public bool IsFallingDown(Vector2 loc)
        {

            if (loc.X > Start.X && loc.X < End.X)
            {

                float dif = End.X - Start.X;
                float difLoc = loc.X - Start.X;

                float normalizedDifference = difLoc / dif;
                float yLoc = normalizedDifference * (End.Y - Start.Y) + Start.Y;

                if (loc.Y <= yLoc - upDownFudge)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}
