using UnityEngine;

namespace Spawn
{
    public class SpawnFacade : MonoBehaviour
    {
        protected EnemiesSpawner EnemiesSpawner;
        protected CoinsSpawner CoinsSpawner;
        protected ExplosionsSpawner ExplosionsSpawner;
        protected RoadSpawner RoadSpawner;

        void Start()
        {
            EnemiesSpawner = GameObject.Find("EnemiesSpawner").GetComponent<EnemiesSpawner>();
            CoinsSpawner = GameObject.Find("CoinsSpawner").GetComponent<CoinsSpawner>();
            ExplosionsSpawner = GameObject.Find("ExplosionsSpawner").GetComponent<ExplosionsSpawner>();
            RoadSpawner = GameObject.Find("RoadSpawner").GetComponent<RoadSpawner>();
        }

        // public void FixedUpdate()
        // {
        //     Debug.Log("Enemies count: " + EnemiesSpawner.Enemies.Count);
        //     Debug.Log("Coins count: " + CoinsSpawner.Coins.Count);
        //     Debug.Log("Explosions count: " + ExplosionsSpawner.Explosions.Count);
        //     Debug.Log("Roads count: " + RoadSpawner.Roads.Count);
        // }
    }
}