using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : MonoBehaviour
{
    public static SwipeManager instance;

    public delegate void MoveDelegate(bool[] swipes);
    public MoveDelegate MoveEvent;

    public delegate void ClickDelegate(bool[] swipes);
    public MoveDelegate ClickEvent;

    public enum Direction { Left, Right, Up, Down };

    private const float SWIPE_THRESHOLD = 50;

    private Vector2 _startTouch;
    private Vector2 _swipeDelta;
    private Vector2 _touchPosition() { return (Vector2)Input.mousePosition; }
    private bool _touchBegan() { return Input.GetMouseButtonDown(0); }
    private bool _touchEnded() { return Input.GetMouseButtonUp(0); }
    private bool _getTouch() { return Input.GetMouseButton(0); }
    private bool _touchMoved;
    private bool[] _swipe = new bool[4];


    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (_touchBegan())
        {
            _startTouch = _touchPosition();
            _touchMoved = true;
        }
        else if (_touchEnded() && _touchMoved)
        {
            SendSwipe();
            _touchMoved = false;
        }

        _swipeDelta = Vector2.zero;

        if (_touchMoved && _getTouch())
        {
            _swipeDelta = _touchPosition() - _startTouch;
        }

        if (_swipeDelta.magnitude > SWIPE_THRESHOLD)
        {
            if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
            {
                _swipe[(int)Direction.Left] = _swipeDelta.x < 0;
                _swipe[(int)Direction.Right] = _swipeDelta.x > 0;
            }
            else
            {
                _swipe[(int)Direction.Up] = _swipeDelta.y > 0;
                _swipe[(int)Direction.Down] = _swipeDelta.y < 0;
            }
            SendSwipe();
        }
    }

    private void SendSwipe()
    {
        if (_swipe[0] || _swipe[1] || _swipe[2] || _swipe[3])
        {
            MoveEvent?.Invoke(_swipe);
        }
        else
        {
            ClickEvent?.Invoke(_swipe);
        }
        Reset();
    }

    private void Reset()
    {
        _startTouch = Vector2.zero;
        _swipeDelta = Vector2.zero;
        _touchMoved = false;
        for (int i = 0; i < _swipe.Length; i++)
        {
            _swipe[i] = false;
        }
    }
}