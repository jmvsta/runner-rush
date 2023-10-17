using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoad : MonoBehaviour
{
    [SerializeField] private RoadSpawner _roadSpawner;
    [SerializeField] private float Speed = 10;

    private void FixedUpdate()
    {
        MovedRoad();
    }

    private void MovedRoad()
    {
        transform.Translate(-transform.forward * Speed * Time.deltaTime);
    }
}
