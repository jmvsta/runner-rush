using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CoinsGeneration;
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

        public void GenerateCoins(float roadPos)
        {
            Fabric.GetStrategy((Strategy)Random.Next(0, 3))
                .Apply(_coins.FindAll(r => !r.activeSelf).Take(_coinsBatchSize).ToList(), roadPos);
        }

        void Update()
        {
            _coins.FindAll(r => r.activeSelf && r.transform.position.z < -10).ForEach(r => r.SetActive(false));
        }
    }
}