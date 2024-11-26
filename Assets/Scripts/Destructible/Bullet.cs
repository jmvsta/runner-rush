using UnityEngine;

namespace Destructible
{
    public class Bullet : Destructible
    {
        [SerializeField] private float life = 10;

        void Awake()
        {
            Destroy(gameObject, life);
        }

        public override void OnChildTriggerEnter(Collider other, ChildTrigger childTrigger)
        {
            throw new System.NotImplementedException();
        }
    }
}
