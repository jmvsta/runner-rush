using System.Collections.Generic;
using System.Linq;
using Spawn.CoinsGeneration;
using UnityEngine;
using UnityEngine.UI;

namespace Spawn
{
    public class CoinsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _coinPrefab;
        [SerializeField] private Text _coinsText;
        [SerializeField] private int _coins;
        private readonly int _coinsSize = 300;
        private readonly int _coinsBatchSize = 41;
        private GameObject _tail;
        public List<GameObject> Coins { get; } = new();

        void Start()
        {
            for (var i = 0; i < _coinsSize; i++)
            {
                var initializedCoin = Instantiate(_coinPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                initializedCoin.SetActive(false);
                Coins.Add(initializedCoin);
            }
        }
        
        public void GenerateCoins(List<GameObject> obstacles, float roadPos)
        {
            var coinsToActivateList = Coins.FindAll(r => !r.activeSelf).Take(_coinsBatchSize).ToList();
            Fabric.GetStrategy(3).Apply(coinsToActivateList, obstacles, roadPos, _tail);
            _tail = coinsToActivateList[^1];
        }

        void Update()
        {
            Coins.FindAll(r => r.activeSelf && r.transform.position.z < -10).ForEach(r => r.SetActive(false));
        }
    }
}