using System.Collections.Generic;
using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _coinPrefab;
    private List<GameObject> _coins = new();
    private int _coinsSize = 60;
    void Start()
    {
        for (var i = 0; i <= _coinsSize; i++)
        {
            var initializedEnemy = Instantiate(_coinPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            initializedEnemy.SetActive(false);
            _coins.Add(initializedEnemy);
        }
        // _roadSpawner = GameObject.Find("RoadSpawner").GetComponent<RoadSpawner>();
    }

    public void GenerateCoins(float roadPos)
    {
        _coins.FindAll(r => r.activeSelf == false).GetRange(0, 10).ForEach(coin =>
        {
            coin.transform.position = new Vector3(Random.Range(-1, 1) * 3, Random.Range(0, 4) * 3, roadPos += 10);
            coin.SetActive(true);
        });

        // return enemyToActivate;
    }

    // Update is called once per frame
    void Update()
    {
        _coins.FindAll(r => r.activeSelf && r.transform.position.z < -10).ForEach(r => r.SetActive(false));
    }
}