using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Spawn
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _obstaclesPrefabs;
        private readonly List<GameObject> _obstacles = new();
        private readonly int _size = 20;
        private readonly Random _random = new();
        public List<GameObject> Obstacles => _obstacles;

        void Start()
        {
            for (var i = 0; i < _size; i++)
            {
                var initializedObstacle = Instantiate(_obstaclesPrefabs[_random.Next(0, _obstaclesPrefabs.Length)], new Vector3(0, 0, 0), Quaternion.identity);
                initializedObstacle.SetActive(false);
                _obstacles.Add(initializedObstacle);
            }
        }

        public void GenerateObstacles(float roadPos)
        {
            
        }

        void Update()
        {
            _obstacles.FindAll(r => r.activeSelf && r.transform.position.z < -10).ForEach(r => r.SetActive(false));
        }
    }
}