﻿using System.Collections.Generic;
using System.Linq;
using Spawn.CoinsGeneration;
using UnityEngine;
using Random = System.Random;

namespace Spawn
{
    public class CoinsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _coinPrefab;
        private readonly List<GameObject> _coins = new();
        private readonly int _coinsSize = 300;
        private readonly int _coinsBatchSize = 40;
        private readonly Random Random = new();
        public List<GameObject> Coins => _coins;

        void Start()
        {
            for (var i = 0; i < _coinsSize; i++)
            {
                var initializedCoin = Instantiate(_coinPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                initializedCoin.SetActive(false);
                _coins.Add(initializedCoin);
            }
        }

        // TODO: generation depending on obstacles, also maybe static enemies
        public void GenerateCoins(List<GameObject> obstacles, float roadPos)
        {
            // Fabric.GetStrategy((Strategy)Random.Next(0, 3))
            Fabric.GetStrategy(0)
                .Apply(_coins.FindAll(r => !r.activeSelf).Take(_coinsBatchSize).ToList(), obstacles, roadPos);
        }

        void Update()
        {
            _coins.FindAll(r => r.activeSelf && r.transform.position.z < -10).ForEach(r => r.SetActive(false));
        }
    }
}