using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    // private RoadSpawner _roadSpawner;
    [SerializeField] private GameObject[] _enemiesPrefabs;
    private List<GameObject> _enemies = new();
    private int _enemiesSize = 10;
    void Start()
    {
        for (var i = 0; i <= _enemiesSize; i++)
        {
            var initializedEnemy = Instantiate(_enemiesPrefabs[Random.Range(0, _enemiesPrefabs.Length)],
                new Vector3(0, 0, 0), Quaternion.identity);
            initializedEnemy.SetActive(false);
            _enemies.Add(initializedEnemy);
        }
        // _roadSpawner = GameObject.Find("RoadSpawner").GetComponent<RoadSpawner>();
    }

    public void GenerateEnemy(float roadPos)
    {
        var activationCandidates = _enemies.FindAll(r => r.activeSelf == false);
        var enemyToActivate = activationCandidates[Random.Range(0, activationCandidates.Count)];
        enemyToActivate.transform.position = new Vector3(Random.Range(-1, 1) * 3, 0, roadPos + Random.Range(0, 100));
        enemyToActivate.SetActive(true);
        // return enemyToActivate;
    }

    // Update is called once per frame
    void Update()
    {
        _enemies.FindAll(r => r.activeSelf && r.transform.position.z < -10).ForEach(r => r.SetActive(false));
    }
}
