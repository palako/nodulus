using UnityEngine;

namespace Model
{
    /// <summary>
    /// Manages game settings and preferences.
    /// </summary>
    public static class GameSettings
    {
        private const string ShowMinimumMovesKey = "ShowMinimumMoves";
        
        /// <summary>
        /// Gets or sets whether to show the minimum number of moves counter.
        /// Default is true (show the counter).
        /// </summary>
        public static bool ShowMinimumMoves
        {
            get => PlayerPrefs.GetInt(ShowMinimumMovesKey, 1) == 1;
            set => PlayerPrefs.SetInt(ShowMinimumMovesKey, value ? 1 : 0);
        }
    }
}
