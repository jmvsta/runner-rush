using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Spawn
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _obstaclesPrefabs;
        private readonly List<GameObject> _obstacles = new();
        private readonly int _size = 40;
        private readonly Random _random = new();
        private readonly int _genCount = 10;
        public List<GameObject> Obstacles => _obstacles;

        void Start()
        {
            for (var i = 0; i < _size; i++)
            {
                var initializedObstacle = Instantiate(_obstaclesPrefabs[_random.Next(0, _obstaclesPrefabs.Length)],
                    new Vector3(0, 0, 0), Quaternion.identity);
                initializedObstacle.SetActive(false);
                _obstacles.Add(initializedObstacle);
            }
        }
        
        public List<GameObject> GenerateObstacles(float roadPos)
        {
            var activeObstacles = _obstacles.FindAll(r => !r.activeSelf).Take(_genCount).ToList();
            activeObstacles.ForEach(obstacle =>
            {
                roadPos += 20;
                obstacle.transform.position = new Vector3(_random.Next(-1, 2) * 3, 0, roadPos);
                obstacle.SetActive(true);
            });
            return activeObstacles;
        }

        void Update()
        {
            _obstacles.FindAll(r => r.activeSelf && r.transform.position.z < -10).ForEach(r => r.SetActive(false));
        }
    }
}