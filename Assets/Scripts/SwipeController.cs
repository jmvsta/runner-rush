using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private bool _isDragging;
    private Vector2 _startTouch, _swipeDelta;
    public static Swipe CurrentSwipe = Swipe.None;
    public static int SwipeValue;

    public enum Swipe
    {
        SwipeLeft, SwipeRight, SwipeUp, SwipeDown, None
    }

    private void Update()
    {
        CurrentSwipe = Swipe.None;

        #region ПК-версия
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Reset();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CurrentSwipe = Swipe.SwipeUp;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CurrentSwipe = Swipe.SwipeRight;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CurrentSwipe = Swipe.SwipeLeft;
        }
        #endregion

        #region Мобильная версия
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                _isDragging = true;
                _startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                Reset();
            }
        }
        #endregion
        
        _swipeDelta = Vector2.zero;
        if (_isDragging)
        {
            if (Input.touches.Length < 0)
                _swipeDelta = Input.touches[0].position - _startTouch;
            else if (Input.GetMouseButton(0))
                _swipeDelta = (Vector2)Input.mousePosition - _startTouch;
        }
        
        if (_swipeDelta.magnitude > 100)
        {
            float x = _swipeDelta.x;
            float y = _swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                CurrentSwipe = x < 0 ? Swipe.SwipeLeft : Swipe.SwipeRight;
            }
            else
            {
                CurrentSwipe = y < 0 ? Swipe.SwipeDown : Swipe.SwipeUp;
            }

            Reset();
        }

    }

    private void Reset()
    {
        _startTouch = _swipeDelta = Vector2.zero;
        _isDragging = false;
    }
}