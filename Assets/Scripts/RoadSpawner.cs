using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _roadPrefabs;
    [SerializeField] private float _spawnPos;
    [SerializeField] private float _speed = 10;
    private readonly List<GameObject> _activeRoads = new();
    private readonly List<GameObject> _noneActiveRoad = new();
    private const float RoadLength = 100;
    private const int InitRoads = 3;
    private const int MaxRoads = 10;

    public float Speed => _speed;
    
    void Start()
    {
        for (var i = 0; i < MaxRoads; i++)
        {
            if (i >= InitRoads)
            {
                var initializedRoad = Instantiate(_roadPrefabs[Random.Range(0, _roadPrefabs.Length)],
                    new Vector3(0, 0, 0), Quaternion.identity);
                initializedRoad.SetActive(false);
                _noneActiveRoad.Add(initializedRoad);
            }
            else
            {
                var initializedRoad = Instantiate(_roadPrefabs[Random.Range(0, _roadPrefabs.Length)],
                    transform.forward * _spawnPos, Quaternion.identity);
                initializedRoad.SetActive(true);
                _spawnPos += RoadLength;
                _activeRoads.Add(initializedRoad);
            }
        }
    }

    public void DeactivateRoad(GameObject road)
    {
        _activeRoads.Remove(road);
        road.transform.position = new Vector3(0, 0, 0);
        road.SetActive(false);
        _noneActiveRoad.Add(road);
    }

    public void ActivateRoad()
    {
        var roadToActivate = _noneActiveRoad[Random.Range(0, _noneActiveRoad.Count)];
        _noneActiveRoad.Remove(roadToActivate);
        roadToActivate.transform.position = new Vector3(0, 0, 200);
        roadToActivate.SetActive(true);
        _activeRoads.Add(roadToActivate);
    }
}