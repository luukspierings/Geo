using UnityEngine;

namespace Assets.Scripts.Calculators
{
	public static class InputHelper
	{
		private static bool _isMouseDragging;
		private static Vector3 _lastMousePosition;

		public static Vector2 GetDeltaPosition()
		{
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				return Input.GetTouch(0).deltaPosition;
			}

			if (Input.GetMouseButton(0))
			{
				if (!_isMouseDragging)
				{
					_lastMousePosition = Input.mousePosition;
					_isMouseDragging = true;
					return Vector2.zero;
				}

				var deltaPosition = Input.mousePosition - _lastMousePosition;
				_lastMousePosition = Input.mousePosition;
				return deltaPosition;
			}
			else if (_isMouseDragging)
				_isMouseDragging = false;

			return Vector2.zero;
		}

		public static float GetDeltaZoom()
		{
			// TODO: Touch zooming

			return Input.mouseScrollDelta.y;
		}

	}
}
