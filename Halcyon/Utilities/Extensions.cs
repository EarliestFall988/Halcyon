using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Utilities
{
    /// <summary>
    /// Extension class to add functionality to a few important libraries
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Convert the Vector2 to a Vector2 from the Aether Physics2D library
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static Vector2 ToSystemVector2(this nkast.Aether.Physics2D.Common.Vector2 vector2)
        {
            return new Vector2(vector2.X, vector2.Y);
        }

        /// <summary>
        ///  Convert the Vector2 to a Vector2 from the Aether Physics2D library
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static nkast.Aether.Physics2D.Common.Vector2 ToAetherVector2(this Vector2 vector2)
        {
            return new nkast.Aether.Physics2D.Common.Vector2(vector2.X, vector2.Y);
        }

    }
}
