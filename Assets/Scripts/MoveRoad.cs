using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoad : MonoBehaviour
{
    private void FixedUpdate()
    {
        MovedRoad(GameObject.Find("RoadSpawner").GetComponent<RoadSpawner>().Speed);
    }

    private void MovedRoad(float speed)
    {
        transform.Translate(-transform.forward * speed * Time.deltaTime);
    }
}
