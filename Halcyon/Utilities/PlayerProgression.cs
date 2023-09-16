using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Utilities
{
    /// <summary>
    /// this class represents the player's progression in the game
    /// </summary>
    public static class PlayerProgression
    {
        /// <summary>
        /// the current level of the player
        /// </summary>
        public static int CurrentLevel { get; set; } = 0;

        /// <summary>
        /// the current xp of the player
        /// </summary>
        public static int CurrentXP { get; set; } = 0;


        private static int _currentCollectedCointsStore = 0;

        /// <summary>
        /// the current amount of coins the player has
        /// </summary>
        public static int CurrentCollectedCoins
        {
            get => _currentCollectedCointsStore;
            set
            {
                _currentCollectedCointsStore = value;
                OnCurrentCollectedCoinsChanged?.Invoke(value);
                Debug.WriteLine("current collected: " + value);
            }
        }

        public delegate void CurrentCollectedCoinsChangedDelegate(int level);
        public static event CurrentCollectedCoinsChangedDelegate OnCurrentCollectedCoinsChanged;


        public static void IncrementCoinsCollected()
        {
            CurrentCollectedCoins++;
        }
    }
}
