using Spawn;
using UnityEngine;

namespace Destructible
{
    public class Wall : Destructible
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
                    _explosionsSpawner.GenerateExplosion(gameObject.transform.position,
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
