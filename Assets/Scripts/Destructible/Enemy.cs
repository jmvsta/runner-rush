using Spawn;
using UnityEngine;

namespace Destructible
{
    public class Enemy : Destructible
    {
        private ExplosionsSpawner _explosionsSpawner;
        
        void Start()
        {
            _explosionsSpawner = GameObject.Find("ExplosionsSpawner").GetComponent<ExplosionsSpawner>();
        }
        
        
        public override void OnChildTriggerEnter(Collider other, ChildTrigger childTrigger)
        {
            switch (childTrigger.gameObject.tag)
            {
                case "Died":  
                    _explosionsSpawner.GenerateExplosion(gameObject.transform.position, ExplosionsSpawner.ExplosionType.Enemy);
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