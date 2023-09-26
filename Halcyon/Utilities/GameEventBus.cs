using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Utilities
{
    /// <summary>
    /// This class is used to trigger and manage events in the game. 
    /// </summary>
    public class GameEventBus
    {
        /// <summary>
        /// the id of the next event to be added
        /// </summary>
        private Dictionary<string, GameEvent> GameEvents = new Dictionary<string, GameEvent>();

        public IEnumerator<GameEvent> GetEnumerator()
        {
            return GameEvents.Values.GetEnumerator();
        }

        public static GameEventBus Main;

        public GameEventBus()
        {
            if (Main == null)
            {
                Main = this;
            }
            else
            {
                throw new Exception("GameEventBus already exists!");
            }
        }

        public void NewGameEvent(string id, GameEvent someEvent)
        {
            if (GameEvents.ContainsKey(id))
            {
                throw new Exception("GameEvent with id " + id + " already exists!");
            }
            else
            {
                GameEvents.Add(id, someEvent);
            }
        }

        public void RemoveGameEvent(string id)
        {
            if (GameEvents.ContainsKey(id))
            {
                GameEvents.Remove(id);
            }
            else
            {
                throw new Exception("GameEvent with id " + id + " does not exist!");
            }
        }

        #region triggers

        public void TriggerGameEvent(string id, string args)
        {
            if (GameEvents.ContainsKey(id))
            {
                GameEvents[id].Event(args);
            }
            else
            {
                throw new Exception("GameEvent with id " + id + " does not exist!");
            }
        }


        public void TriggerGameEvent(string id, string args, bool removeAfter)
        {
            if (GameEvents.ContainsKey(id))
            {
                GameEvents[id].Event(args);
                if (removeAfter)
                {
                    RemoveGameEvent(id);
                }
            }
            else
            {
                throw new Exception("GameEvent with id " + id + " does not exist!");
            }
        }

        public void TriggerGameEvent(string id, string args, int times)
        {
            if (GameEvents.ContainsKey(id))
            {
                GameEvents[id].Event(args);

                times--;
                if (times <= 0)
                {
                    RemoveGameEvent(id);
                }
            }
            else
            {
                throw new Exception("GameEvent with id " + id + " does not exist!");
            }
        }

        public void TriggerGameEvent(string id, string args, int times, bool removeAfter)
        {
            if (GameEvents.ContainsKey(id))
            {
                GameEvents[id].Event(args);

                times--;

                if (removeAfter || times <= 0)
                {
                    RemoveGameEvent(id);
                }
            }
            else
            {
                throw new Exception("GameEvent with id " + id + " does not exist!");
            }
        }

        #endregion

        public void Clear()
        {
            GameEvents.Clear();
        }
    }

    public class GameEvent
    {
        public string Name { get; set; }
        public string Descripion { get; set; }
        public Action<string> Event { get; set; }
    }
}
