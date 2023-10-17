using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _roadPrefabs;
    [SerializeField] private GameObject _spawnObj;
    [SerializeField] private float _spawnPos = 0;
    [SerializeField] private float _speed = 10;
    private List<GameObject> _activeRoad = new List<GameObject>();
    private float _roadLenght = 100;
    private int _startRoad = 5;

    public float Speed { get { return _speed; } private set { } }


    void Start()
    {
        for (int i = 0; i < _startRoad; i++)
        {
            SpawnRoad();
            _spawnPos += _roadLenght;
        }

        _spawnPos = _spawnObj.transform.position.z;
    }

    public void SpawnRoad()
    {
        GameObject _nextRoad = Instantiate(_roadPrefabs[Random.Range(0, _roadPrefabs.Length - 1)], transform.forward * _spawnPos, Quaternion.identity);
        _activeRoad.Add(_nextRoad);
    }

    public void DeleteRoad()
    {
        _activeRoad[0].SetActive(false);
        _activeRoad.RemoveAt(0);
    }
}
