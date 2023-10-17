using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;
    [SerializeField] private float _lineDistanse = 3;
    private CharacterController _characterController;
    [SerializeField]  private RoadSpawner _generator;
    private Vector3 _dir;
    private int _lineToMove = 1;
    
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (SwipeController.SwipeRight)
        {
            if (_lineToMove < 2)
            {
                _lineToMove++;
            }
        }

        if (SwipeController.SwipeLeft)
        {
            if (_lineToMove > 0)
            {
                _lineToMove--;
            }
        }

        if (SwipeController.SwipeUp)
        {
            if (_characterController.isGrounded)
            {
                Jump();
            }
        }

        Vector3 _targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (_lineToMove == 0)
        {
            _targetPosition += Vector3.left * _lineDistanse;
        }
        else if (_lineToMove == 2)
        {
            _targetPosition += Vector3.right * _lineDistanse;
        }

        transform.position = _targetPosition;
    }

    void FixedUpdate()
    {
    //    _dir.z = _speed;
        _dir.y += _gravity * Time.fixedDeltaTime;
        _characterController.Move(_dir * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        _dir.y = _jumpForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        _generator.SpawnRoad();
        _generator.DeleteRoad();
    }
}
