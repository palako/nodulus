using System.Collections.Generic;
using System.Linq;
using Core.Data;
using UnityEngine;
using View.Game;

namespace View.Control
{
    /// <summary>
    /// Handles keyboard input for cursor movement and node rotation
    /// </summary>
    public class KeyboardInput : MonoBehaviour
    {
        private BoardAction _boardAction;
        private IDictionary<Point, NodeView> _nodeMap;
        private NodeView _currentNode;
        private CursorIndicator _cursorIndicator;

        public void Initialize(BoardAction boardAction, IDictionary<Point, NodeView> nodeMap)
        {
            _boardAction = boardAction;
            _nodeMap = nodeMap;

            // Select the first node as the initial cursor position
            if (_nodeMap != null && _nodeMap.Count > 0)
            {
                _currentNode = _nodeMap.Values.First();
                UpdateCursorIndicator();
            }
        }

        private void Update()
        {
            if (!enabled || _currentNode == null)
            {
                return;
            }

            // Check for Shift + Arrow keys (rotation)
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    RotateNode(Direction.Up);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    RotateNode(Direction.Down);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    RotateNode(Direction.Left);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    RotateNode(Direction.Right);
                }
            }
            // Check for Arrow keys alone (cursor movement)
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    MoveCursor(Direction.Up);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    MoveCursor(Direction.Down);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    MoveCursor(Direction.Left);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    MoveCursor(Direction.Right);
                }
            }
        }

        private void MoveCursor(Direction direction)
        {
            var nextNode = FindNextNodeInDirection(_currentNode.Position, direction);

            if (nextNode != null)
            {
                _currentNode = nextNode;
                UpdateCursorIndicator();
            }
            else
            {
                // No node found in that direction, trigger blink
                if (_cursorIndicator != null)
                {
                    _cursorIndicator.Blink();
                }
            }
        }

        private void RotateNode(Direction direction)
        {
            if (_boardAction != null && _currentNode != null)
            {
                _boardAction.Play(_currentNode, direction);
            }
        }

        private NodeView FindNextNodeInDirection(Point currentPosition, Direction direction)
        {
            var directionVector = direction.ToPoint();
            NodeView closestNode = null;
            float closestDistance = float.MaxValue;

            foreach (var kvp in _nodeMap)
            {
                var nodePosition = kvp.Key;
                var nodeView = kvp.Value;

                // Skip the current node
                if (nodePosition == currentPosition)
                {
                    continue;
                }

                // Calculate the difference vector
                var diff = nodePosition - currentPosition;

                // Check if the node is in the correct direction
                bool isInDirection = false;
                switch (direction)
                {
                    case Direction.Up:
                        isInDirection = diff.Y > 0;
                        break;
                    case Direction.Down:
                        isInDirection = diff.Y < 0;
                        break;
                    case Direction.Left:
                        isInDirection = diff.X < 0;
                        break;
                    case Direction.Right:
                        isInDirection = diff.X > 0;
                        break;
                }

                if (!isInDirection)
                {
                    continue;
                }

                // Calculate distance (Manhattan distance for grid-based movement)
                float distance = Mathf.Abs(diff.X) + Mathf.Abs(diff.Y);

                // For primary axis movement, prioritize nodes that are more aligned with the direction
                float alignmentPenalty = 0f;
                if (direction.IsHorizontal())
                {
                    alignmentPenalty = Mathf.Abs(diff.Y) * 0.5f;
                }
                else if (direction.IsVertical())
                {
                    alignmentPenalty = Mathf.Abs(diff.X) * 0.5f;
                }

                float totalDistance = distance + alignmentPenalty;

                if (totalDistance < closestDistance)
                {
                    closestDistance = totalDistance;
                    closestNode = nodeView;
                }
            }

            return closestNode;
        }

        private void UpdateCursorIndicator()
        {
            // Remove indicator from previous node
            if (_cursorIndicator != null)
            {
                Destroy(_cursorIndicator.gameObject);
                _cursorIndicator = null;
            }

            // Add indicator to current node
            if (_currentNode != null)
            {
                _cursorIndicator = _currentNode.gameObject.AddComponent<CursorIndicator>();
                _cursorIndicator.Show();
            }
        }

        private void OnDisable()
        {
            // Hide cursor when input is disabled
            if (_cursorIndicator != null)
            {
                _cursorIndicator.Hide();
            }
        }

        private void OnEnable()
        {
            // Show cursor when input is enabled
            if (_cursorIndicator != null)
            {
                _cursorIndicator.Show();
            }
        }
    }
}
