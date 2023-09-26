using Lib.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Dynamic_Game_Objects
{
    /// <summary>
    /// pressure plate class
    /// </summary>
    public class PressurePlate
    {
        /// <summary>
        /// the name of the event
        /// </summary>
        public string EventName { get; set; } = "";

        /// <summary>
        /// the On event arguments
        /// </summary>
        public string OnEventArgs { get; set; } = "";

        /// <summary>
        /// the Off event arguments
        /// </summary>
        public string OffEventArgs { get; set; } = "";

        /// <summary>
        /// called when the player enters the pressure plate
        /// </summary>
        public void OnEnter()
        {
            GameEventBus.Main.TriggerGameEvent(EventName, OnEventArgs);
        }

        /// <summary>
        /// called when the player exits the pressure plate
        /// </summary>
        public void OnExit()
        {
            GameEventBus.Main.TriggerGameEvent(EventName, OffEventArgs);
        }

    }
}
