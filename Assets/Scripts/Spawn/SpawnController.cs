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
            _enemiesSpawner.GenerateEnemy(500 + position.z);
            _coinsSpawner.GenerateCoins(450 + position.z);
            _obstaclesSpawner.GenerateObstacles(350 + position.z);
        }
    }
}