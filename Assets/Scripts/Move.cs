using UnityEngine;
using Spawn;

public class Move : MonoBehaviour
{
    private RoadSpawner _roadSpawner;

    private void Start()
    {
        _roadSpawner = GameObject.Find("RoadSpawner").GetComponent<RoadSpawner>();
    }

    private void FixedUpdate()
    {
        transform.Translate(-transform.forward * _roadSpawner.Speed * Time.deltaTime);
    }
}
