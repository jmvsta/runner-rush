using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemiesPrefabs;
    private ExplosionsSpawner _explosionsSpawner;
    private List<GameObject> _enemies = new();
    private int _enemiesSize = 10;
    public List<GameObject> Enemies => _enemies; 
    
    void Start()
    {
        _explosionsSpawner = GameObject.Find("ExplosionsSpawner").GetComponent<ExplosionsSpawner>();
        for (var i = 0; i <= _enemiesSize; i++)
        {
            var initializedEnemy = Instantiate(_enemiesPrefabs[Random.Range(0, _enemiesPrefabs.Length)],
                new Vector3(0, 0, 0), Quaternion.identity);
            initializedEnemy.SetActive(false);
            _enemies.Add(initializedEnemy);
        }
    }

    public void GenerateEnemy(float roadPos)
    {
        var activationCandidates = _enemies.FindAll(r => r.activeSelf == false);
        var enemyToActivate = activationCandidates[Random.Range(0, activationCandidates.Count)];
        enemyToActivate.transform.position = new Vector3(Random.Range(-1, 1) * 3, 0, roadPos + Random.Range(0, 100));
        enemyToActivate.SetActive(true);
    }

    public void KillEnemy(Collider other)
    {
        var enemy = other.transform.parent.gameObject.transform.parent.gameObject;
        var place = new Vector3(enemy.transform.position.x, 2, 0);
        var explosion = _explosionsSpawner.GenerateExplosion(place, ExplosionsSpawner.ExplosionType.Enemy);
        enemy.SetActive(false);
        StartCoroutine(ExplosionsSpawner.DisableExplosionDelay(explosion));
    }
    
    void Update()
    {
        _enemies.FindAll(r => r.activeSelf && r.transform.position.z < -10).ForEach(r => r.SetActive(false));
    }
}
