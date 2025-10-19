using UnityEngine;
using UnityEngine.UI;

namespace View.Control
{
    /// <summary>
    /// Controls the settings menu UI.
    /// </summary>
    public class SettingsMenu : MonoBehaviour
    {
        private Toggle _showMinimumMovesToggle;

        private void Awake()
        {
            _showMinimumMovesToggle = GameObject.FindGameObjectWithTag("ShowMinimumMovesToggle").GetComponent<Toggle>();
        }

        private void Start()
        {
            // Initialize toggle state from saved settings
            _showMinimumMovesToggle.isOn = Model.GameSettings.ShowMinimumMoves;
            
            // Add listener for toggle changes
            _showMinimumMovesToggle.onValueChanged.AddListener(OnShowMinimumMovesChanged);
        }

        private void OnDestroy()
        {
            // Remove listener to prevent memory leaks
            if (_showMinimumMovesToggle != null)
            {
                _showMinimumMovesToggle.onValueChanged.RemoveListener(OnShowMinimumMovesChanged);
            }
        }

        private void OnShowMinimumMovesChanged(bool value)
        {
            Model.GameSettings.ShowMinimumMoves = value;
            PlayerPrefs.Save();
        }
    }
}
