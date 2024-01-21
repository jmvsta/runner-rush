using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _roadPrefabs;
    [SerializeField] private GameObject _roadPrefab;
    [SerializeField] private GameObject _spawnObj;
    [SerializeField] private Text _scoreText;
    [SerializeField] private float _spawnPos = 0;
    [SerializeField] private int _speed = 10;
    private List<GameObject> _activeRoad = new List<GameObject>();
    private float _roadLenght = 200;
    private int _startRoad = 5;
    private float _score;

    public float Speed {get { return _speed; } private set { Speed = _speed; } }


    void Start()
    {
        for (int i = 0; i < _startRoad; i++)
        {
            if (i == 0)
            {
                SpawnRoad(i);
            }
            else
            {
                SpawnRoad();
            }
            _spawnPos += _roadLenght;
        }

        _spawnPos = _spawnObj.transform.position.z;
    }

    public void SpawnRoad()
    {
        GameObject _nextRoad = Instantiate(_roadPrefabs[Random.Range(0, _roadPrefabs.Length)], transform.forward * _spawnPos, Quaternion.identity);
        _activeRoad.Add(_nextRoad);
    }

    public void SpawnRoad(int roadId)
    {
        GameObject _nextRoad = Instantiate(_roadPrefab, transform.forward * _spawnPos, Quaternion.identity);
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
