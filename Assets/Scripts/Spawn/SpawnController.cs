using UnityEngine;

namespace Spawn
{
    public class SpawnController : MonoBehaviour
    {
        private EnemiesSpawner _enemiesSpawner;
        private CoinsSpawner _coinsSpawner;
        private ExplosionsSpawner _explosionsSpawner;
        private RoadSpawner _roadSpawner;

        void Start()
        {
            _enemiesSpawner = GameObject.Find("EnemiesSpawner").GetComponent<EnemiesSpawner>();
            _coinsSpawner = GameObject.Find("CoinsSpawner").GetComponent<CoinsSpawner>();
            _explosionsSpawner = GameObject.Find("ExplosionsSpawner").GetComponent<ExplosionsSpawner>();
            _roadSpawner = GameObject.Find("RoadSpawner").GetComponent<RoadSpawner>();
        }

        public void FixedUpdate()
        {
            Debug.Log("Enemies count: " + _enemiesSpawner.Enemies.Count);
            Debug.Log("Coins count: " + _coinsSpawner.Coins.Count);
            Debug.Log("Explosions count: " + _explosionsSpawner.Explosions.Count);
            Debug.Log("Roads count: " + _roadSpawner.Roads.Count);
        }
    }
}