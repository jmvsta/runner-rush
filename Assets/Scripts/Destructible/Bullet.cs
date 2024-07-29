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
    }
}
