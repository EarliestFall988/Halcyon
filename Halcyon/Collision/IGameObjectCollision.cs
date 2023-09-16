using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Collision
{
    public interface IGameObjectCollision
    {

        Vector2 offset { get; set; }
        float rotation { get; set; }

        float scale { get; set; }

        public GameObject gameObject { get; }


        bool CollidesWith(BoundingCircle other);
        bool CollidesWith(BoundingRectangle other);

        void Init(Vector2 center, float width, float height, Transform transform);
        void Update(Transform transform);
    }
}
