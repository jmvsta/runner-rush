using Spawn;
using UnityEngine;

namespace Destructible
{
    public abstract class Destructible : MonoBehaviour
    {
        protected ExplosionsSpawner ExplosionsSpawner;
        
        void Start()
        {
            ExplosionsSpawner = GameObject.Find("ExplosionsSpawner").GetComponent<ExplosionsSpawner>();
        }

        public abstract void OnChildTriggerEnter(Collider other, ChildTrigger childTrigger);
    }
}