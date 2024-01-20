using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _roadPrefabs;
    [SerializeField] private GameObject _roadPrefab1;
    [SerializeField] private GameObject _roadPrefab2;
    [SerializeField] private GameObject _spawnObj;
    [SerializeField] private Text _scoreText;
    [SerializeField] private float _spawnPos = 0;
    [SerializeField] private int _speed = 10;
    private List<GameObject> _activeRoad = new List<GameObject>();
    private float _roadLenght = 100;
    private int _startRoad = 5;
    private float _score;

    public float Speed {get { return _speed; } private set { Speed = _speed; } }


    void Start()
    {
        for (var i = 0; i < MaxRoads; i++)
        {
            if (i == 0 || i == 1)
            {
                SpawnRoad(i);
            }
            else
            {
                SpawnRoad();
            }
            _spawnPos += _roadLenght;

        }
    }

    public void DeactivateRoad(GameObject road)
    {
        _activeRoads.Remove(road);
        road.transform.position = new Vector3(0, 0, 0);
        road.SetActive(false);
        _noneActiveRoad.Add(road);
    }
    
    public void SpawnRoad(int roadId)
    {
        GameObject _nextRoad = Instantiate(roadId == 1 ? _roadPrefab1 : _roadPrefab2, transform.forward * _spawnPos, Quaternion.identity);
        _activeRoad.Add(_nextRoad);
    }

    public void DeleteRoad()
    {
        _activeRoad[0].SetActive(false);
        _activeRoad.RemoveAt(0);
    }

    public void FixedUpdate()
    {
        if (Time.timeScale != 0)
        {
            _score += (float)_speed / 10;
            _scoreText.text = (System.Math.Round(_score, 0)).ToString();
        }
    }
}