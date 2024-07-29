using Spawn;
using UnityEngine;

namespace Destructible
{
    public class Wall : Destructible
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Destroy enemy");
            // var enemy = other.transform.parent.gameObject.transform.parent.gameObject;
            ExplosionsSpawner.GenerateExplosion(gameObject.transform.position,
                ExplosionsSpawner.ExplosionType.Wall);
            gameObject.SetActive(false);
        }
    }
}
