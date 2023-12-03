using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Utilities
{
    public class ResetGameHandler
    {
        public float ResetTimer { get; set; } = 1f;
        private float store { get; set; } = 0;

        public bool Reset { get; set;} = false;

        public void Update(float dt)
        {
            if (!Reset)
            {
                return;
            }

            store += dt;
            if (store >= ResetTimer)
            {
                store = 0;
                ResetGame();
            }
        }

        private void ResetGame()
        {
            ScenesManager.Main.ResetScenes();
            Reset = false;
            store = 0;
        }
    }
}
