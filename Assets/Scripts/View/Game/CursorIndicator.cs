using System.Collections;
using UnityEngine;

namespace View.Game
{
    /// <summary>
    /// Visual indicator showing which node is currently selected by keyboard control
    /// </summary>
    public class CursorIndicator : MonoBehaviour
    {
        private GameObject _indicatorLine;
        private Material _lineMaterial;
        private bool _isBlinking;

        private const float LineWidth = 0.1f;
        private const float LineLength = 0.8f;
        private const float LineYOffset = -0.45f;
        private const float BlinkDuration = 0.3f;
        private const int BlinkCount = 2;

        private void Awake()
        {
            CreateIndicatorLine();
            Hide();
        }

        private void CreateIndicatorLine()
        {
            _indicatorLine = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _indicatorLine.transform.SetParent(transform);
            _indicatorLine.transform.localPosition = new Vector3(0, LineYOffset, 0);
            _indicatorLine.transform.localRotation = Quaternion.identity;
            _indicatorLine.transform.localScale = new Vector3(LineLength, LineWidth, LineWidth);

            // Remove collider as we don't need it for visual indicator
            var collider = _indicatorLine.GetComponent<Collider>();
            if (collider != null)
            {
                Destroy(collider);
            }

            // Create white material for the line
            _lineMaterial = new Material(Shader.Find("Standard"));
            _lineMaterial.color = new Color(1f, 1f, 1f, 0.8f);
            _lineMaterial.SetFloat("_Metallic", 0.5f);
            _lineMaterial.SetFloat("_Glossiness", 0.5f);

            var renderer = _indicatorLine.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = _lineMaterial;
            }
        }

        /// <summary>
        /// Shows the cursor indicator
        /// </summary>
        public void Show()
        {
            if (_indicatorLine != null)
            {
                _indicatorLine.SetActive(true);
            }
        }

        /// <summary>
        /// Hides the cursor indicator
        /// </summary>
        public void Hide()
        {
            if (_indicatorLine != null)
            {
                _indicatorLine.SetActive(false);
            }
        }

        /// <summary>
        /// Triggers a blink animation to indicate the cursor cannot move in the requested direction
        /// </summary>
        public void Blink()
        {
            if (!_isBlinking)
            {
                StartCoroutine(BlinkCoroutine());
            }
        }

        private IEnumerator BlinkCoroutine()
        {
            _isBlinking = true;
            var originalColor = _lineMaterial.color;

            for (int i = 0; i < BlinkCount; i++)
            {
                // Fade out
                _lineMaterial.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.2f);
                yield return new WaitForSeconds(BlinkDuration / (BlinkCount * 2));

                // Fade in
                _lineMaterial.color = originalColor;
                yield return new WaitForSeconds(BlinkDuration / (BlinkCount * 2));
            }

            _isBlinking = false;
        }

        private void OnDestroy()
        {
            if (_lineMaterial != null)
            {
                Destroy(_lineMaterial);
            }
        }
    }
}
