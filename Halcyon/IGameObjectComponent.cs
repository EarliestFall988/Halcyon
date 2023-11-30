using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public interface IGameObjectComponent
    {
            
        GameObject gameObject { get; set; }

        void OnEnable();

        void OnDisable();

        void Draw();

        void Update(GameTime time);
    }
}
