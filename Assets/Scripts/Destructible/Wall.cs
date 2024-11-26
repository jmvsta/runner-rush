using Spawn;
using UnityEngine;

namespace Destructible
{
    public class Wall : Destructible
    {
        public override void OnChildTriggerEnter(Collider other, ChildTrigger childTrigger)
        {
            switch (childTrigger.gameObject.tag)
            {
                case "Died":
                    ExplosionsSpawner.GenerateExplosion(gameObject.transform.position,
                        ExplosionsSpawner.ExplosionType.Wall);
                    gameObject.SetActive(false);
                    break;
                case "Hit":
                default:
                    break;
            }
        }
    }
}
