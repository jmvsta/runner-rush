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

        // TODO: more complicated generation of obstacles
        public void GenerateObstacles(float roadPos)
        {
            var obstaclesToActivate = _obstacles.FindAll(r => !r.activeSelf).Take(4).ToList();
            for (var i = 0; i < 4; i++, roadPos += 25)
            {
                obstaclesToActivate[i].transform.position = new Vector3(_random.Next(-1, 1) * 3, 0, roadPos);
                obstaclesToActivate[i].SetActive(true);
            }
        }

        void Update()
        {
            _obstacles.FindAll(r => r.activeSelf && r.transform.position.z < -10).ForEach(r => r.SetActive(false));
        }
    }
}