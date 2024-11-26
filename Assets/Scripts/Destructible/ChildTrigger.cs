using UnityEngine;

namespace Destructible
{
    public class ChildTrigger: MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            GetComponentInParent<Destructible>()?.OnChildTriggerEnter(other, this);
        }
    }
}