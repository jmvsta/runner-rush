using Spawn;
using UnityEngine;

namespace Destructible
{
    public class Enemy : Destructible
    {
        private void OnTriggerEnter(Collider other)
        {
            // var enemy = other.transform.parent.gameObject.transform.parent.gameObject;
            Debug.Log("Destroy enemy");
            ExplosionsSpawner.GenerateExplosion(gameObject.transform.position,
                ExplosionsSpawner.ExplosionType.Enemy);
            gameObject.SetActive(false);
        }
    }
}