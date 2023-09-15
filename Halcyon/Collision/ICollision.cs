using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Collision
{
    public interface ICollision
    {
        bool CollidesWith(BoundingCircle other);
        bool CollidesWith(BoundingRectangle other);
    }
}
