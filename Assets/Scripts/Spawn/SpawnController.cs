using UnityEngine;

namespace Spawn
{
    public class SpawnController : MonoBehaviour
    {
        private EnemiesSpawner _enemiesSpawner;
        private CoinsSpawner _coinsSpawner;
        private ExplosionsSpawner _explosionsSpawner;
        private RoadSpawner _roadSpawner;
        private ObstaclesSpawner _obstaclesSpawner;

        void Start()
        {
            _enemiesSpawner = GameObject.Find("EnemiesSpawner").GetComponent<EnemiesSpawner>();
            _coinsSpawner = GameObject.Find("CoinsSpawner").GetComponent<CoinsSpawner>();
            _explosionsSpawner = GameObject.Find("ExplosionsSpawner").GetComponent<ExplosionsSpawner>();
            _roadSpawner = GameObject.Find("RoadSpawner").GetComponent<RoadSpawner>();
            _obstaclesSpawner = GameObject.Find("ObstaclesSpawner").GetComponent<ObstaclesSpawner>();
        }

        // TODO: positioning of them against each other
        public void GenerateNext(Collider other)
        {
            GameObject colliderParent = other.transform.parent.gameObject;
            var position = colliderParent.transform.position;
            _roadSpawner.SpawnRoad(colliderParent);
            var enemy = _enemiesSpawner.GenerateEnemy(350 + position.z);
            var activeObstacles = _obstaclesSpawner.GenerateObstacles(350 + position.z);
            activeObstacles.Add(enemy);
            activeObstacles.Sort((x, y) => x.transform.position.z.CompareTo(y.transform.position.z));
            // if enemy static
            // activeObstacles.Add(enemy);
            _coinsSpawner.GenerateCoins(activeObstacles, 350 + position.z);
        }
    }
}