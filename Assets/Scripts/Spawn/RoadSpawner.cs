using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Spawn
{
    public class RoadSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _roadPrefabs;
        [SerializeField] private GameObject[] _asylumPrefabs;
        [SerializeField] private float _spawnPos;
        [SerializeField] private int _speed = 10;
        [SerializeField] private Text _scoreText;
        [SerializeField] private float _roadLength = 100;
        [SerializeField] private int _maxRoads = 10;
        [SerializeField] private int _activeRoads = 5;
        private float _score;
        private readonly List<GameObject> _roads = new();
        private GameObject _prev;
        private GameObject _next;
        private EnemiesSpawner _enemiesSpawner;
        private ObstaclesSpawner _obstaclesSpawner;
        private CoinsSpawner _coinsSpawner;


        public float Speed => _speed;
        public List<GameObject> Roads => _roads;


        void Start()
        {
            _enemiesSpawner = GameObject.Find("EnemiesSpawner").GetComponent<EnemiesSpawner>();
            _coinsSpawner = GameObject.Find("CoinsSpawner").GetComponent<CoinsSpawner>();
            _obstaclesSpawner = GameObject.Find("ObstaclesSpawner").GetComponent<ObstaclesSpawner>();

            if (_asylumPrefabs.Length > _activeRoads)
            {
                throw new Exception("Cannot have init prefabs more than active roads");
            }

            for (var i = 0; i < _maxRoads; i++)
            {
                if (i >= _activeRoads)
                {
                    var initializedRoad = Instantiate(_roadPrefabs[Random.Range(0, _roadPrefabs.Length)],
                        new Vector3(0, 0, 0), Quaternion.identity);
                    initializedRoad.SetActive(false);
                    _roads.Add(initializedRoad);
                }
                else if (i < _asylumPrefabs.Length)
                {
                    _prev = Instantiate(_asylumPrefabs[i],
                        transform.forward * _spawnPos, Quaternion.identity);
                    _prev.SetActive(true);
                    _spawnPos += _roadLength;
                }
                else
                {
                    // TODO: to replace with SpawnRoad call
                    var initializedRoad = Instantiate(_roadPrefabs[Random.Range(0, _roadPrefabs.Length)],
                        transform.forward * _spawnPos, Quaternion.identity);
                    initializedRoad.SetActive(true);
                    _enemiesSpawner.GenerateEnemy(_spawnPos);
                    // _coinsSpawner.GenerateCoins(_spawnPos - 50);
                    _spawnPos += _roadLength;
                    _roads.Add(initializedRoad);
                }
            }
        }

        public void FixedUpdate()
        {
            if (Time.timeScale != 0)
            {
                _score += (float)_speed / 10;
                _scoreText.text = Math.Round(_score, 0).ToString(CultureInfo.InvariantCulture);
            }
        }

        public void SpawnRoad(GameObject road)
        {
            _roads.Find(r => r.Equals(_prev))?.SetActive(false);
            _prev = road;
            var activationCandidates = _roads.FindAll(r => r.activeSelf == false);
            var roadToActivate = activationCandidates[Random.Range(0, activationCandidates.Count)];
            var position = road.transform.position;
            roadToActivate.transform.position = new Vector3(0, 0, _activeRoads * _roadLength + position.z);
            roadToActivate.SetActive(true);
        }
    }
}