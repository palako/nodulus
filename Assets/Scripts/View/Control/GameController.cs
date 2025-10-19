¿using UnityEngine;
using Core.Data;

namespace View.Control
{
	/// <summary>
	/// The main game controller.
	/// </summary>
	public class GameController : MonoBehaviour
	{
		private KeyboardInput _keyboardInput;

		private void Start()
		{
			_keyboardInput = GetComponent<KeyboardInput>();
		}

		private void Update() 
		{
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Application.Quit();
			}

			if (_keyboardInput != null && _keyboardInput.enabled)
			{
				// Check for Shift + Arrow keys (rotation)
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				{
					if (Input.GetKeyDown(KeyCode.UpArrow))
					{
						_keyboardInput.RotateNode(Direction.Up);
					}
					else if (Input.GetKeyDown(KeyCode.DownArrow))
					{
						_keyboardInput.RotateNode(Direction.Down);
					}
					else if (Input.GetKeyDown(KeyCode.LeftArrow))
					{
						_keyboardInput.RotateNode(Direction.Left);
					}
					else if (Input.GetKeyDown(KeyCode.RightArrow))
					{
						_keyboardInput.RotateNode(Direction.Right);
					}
				}
				// Check for Arrow keys alone (cursor movement)
				else
				{
					if (Input.GetKeyDown(KeyCode.UpArrow))
					{
						_keyboardInput.MoveCursor(Direction.Up);
					}
					else if (Input.GetKeyDown(KeyCode.DownArrow))
					{
						_keyboardInput.MoveCursor(Direction.Down);
					}
					else if (Input.GetKeyDown(KeyCode.LeftArrow))
					{
						_keyboardInput.MoveCursor(Direction.Left);
					}
					else if (Input.GetKeyDown(KeyCode.RightArrow))
					{
						_keyboardInput.MoveCursor(Direction.Right);
					}
				}
			}
		}
	}
}
