using Spawn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
    [SerializeField] private ObstaclesSpawner _obstaclesSpawner;
    [SerializeField] private float life = 10;

    void Awake()
    {
        Destroy(gameObject, life);
    }


    void Start()
    {
        _enemiesSpawner = GameObject.Find("EnemiesSpawner").GetComponent<EnemiesSpawner>();
        _obstaclesSpawner = GameObject.Find("ObstaclesSpawner").GetComponent <ObstaclesSpawner>();
    }

        private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Died":
                //_obstaclesSpawner.KillEnemy(other);
                _enemiesSpawner.KillEnemy(other);
                Destroy(gameObject);
                break;
        }
    }
}
