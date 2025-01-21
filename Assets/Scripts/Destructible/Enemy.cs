using Spawn;
using UnityEngine;

namespace Destructible
{
    public class Enemy : Destructible
    {
        public override void OnChildTriggerEnter(Collider other, ChildTrigger childTrigger)
        {
            switch (childTrigger.gameObject.tag)
            {
                case "Died":  
                    ExplosionsSpawner.GenerateExplosion(gameObject.transform.position, ExplosionsSpawner.ExplosionType.Enemy);
                    gameObject.SetActive(false);
                    break;
                case "Hit":
                    break;
                default:
                    break;
            }
            
        }
    }
}