using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _coinPrefab;
    private readonly List<GameObject> _coins = new();
    private readonly int _coinsSize = 300;

    public enum GenerationStrategy
    {
        Mountain,
        River,
        Straight
    }

    private static class CalculateStrategyPos
    {
        public static void GetNextPos(GenerationStrategy enumValue, Vector3 vector3)
        {
            switch (enumValue)
            {
                case GenerationStrategy.Mountain:
                    // vector3.y += 3;
                    break;
                case GenerationStrategy.River:
                    // vector3.x += 3;
                    break;
                case GenerationStrategy.Straight:
                default:
                    break;
            }
        }
    }

    void Start()
    {
        for (var i = 0; i < _coinsSize; i++)
        {
            var initializedCoin = Instantiate(_coinPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            initializedCoin.SetActive(false);
            _coins.Add(initializedCoin);
        }
    }

    public void GenerateCoins(float roadPos, GenerationStrategy strategy = GenerationStrategy.Straight)
    {
        var posX = Random.Range(-1, 1) * 3;
        var nonActiveCoins = _coins.FindAll(r => r.activeSelf == false);
        var rBorder = Math.Min(40, nonActiveCoins.Count);
        nonActiveCoins.GetRange(0, rBorder).ForEach(coin =>
        {
            coin.transform.position = new Vector3(posX, 0, roadPos);
            CalculateStrategyPos.GetNextPos(strategy, coin.transform.position);
            roadPos += 5;
            coin.SetActive(true);
        });
    }

    void Update()
    {
        _coins.FindAll(r => r.activeSelf && r.transform.position.z < -10).ForEach(r => r.SetActive(false));
    }
}