using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawn
{
    public class ExplosionsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _explosionPrefabs;
        private readonly Dictionary<ExplosionType, List<GameObject>> _explosions = new();
        private int _explosionsSize = 10;
        public Dictionary<ExplosionType, List<GameObject>> Explosions => _explosions;

        public enum ExplosionType
        {
            Enemy,
            Wall
        }

        void Start()
        {
            for (var i = 0; i <= _explosionsSize; i++)
            {
                foreach (ExplosionType value in Enum.GetValues(typeof(ExplosionType)))
                {
                    var list = _explosions.GetValueOrDefault(value, new List<GameObject>());
                    if (!list.Any()) _explosions.Add(value, list);
                    var initialized = Instantiate(_explosionPrefabs[(int)value], new Vector3(0, 0, 0),
                        Quaternion.identity);
                    initialized.SetActive(false);
                    list.Add(initialized);
                }
            }
        }

        public GameObject GenerateExplosion(Vector3 vector, ExplosionType explosionType)
        {
            var explosions = _explosions.GetValueOrDefault(explosionType, new List<GameObject>());
            var disabledExplosions = explosions.FindAll(r => r.activeSelf == false);
            var explosionToActivate = disabledExplosions[Random.Range(0, disabledExplosions.Count)];
            explosionToActivate.transform.position = vector;
            explosionToActivate.SetActive(true);
            return explosionToActivate;
        }

        public static IEnumerator DisableExplosionDelay(GameObject gameObject)
        {
            yield return new WaitForSeconds(2);
            gameObject.SetActive(false);
        }
    }
}