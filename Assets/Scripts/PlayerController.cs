using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 _targetPos;
    private float _laneOffset = 4f;
    private float _laneChangeSpeed = 15;
    private float _timeElapsed;
    private float _lerpDuration = 0.5f;

    void Start()
    {
       _targetPos = transform.position;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && _targetPos.x > -_laneOffset)
        {
            _timeElapsed = 0;
            _targetPos = new Vector3(_targetPos.x - _laneOffset, transform.position.y, transform.position.z);
        }

        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && _targetPos.x < _laneOffset)
        {
            _timeElapsed = 0;
            _targetPos = new Vector3(_targetPos.x + _laneOffset, transform.position.y, transform.position.z);
        }
    }

    private void FixedUpdate()
    {
        if (_timeElapsed < _lerpDuration)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPos, _timeElapsed / _lerpDuration);
            _timeElapsed += Time.deltaTime;
        }
        else
        {
            transform.position = _targetPos;
        }
    }
}
