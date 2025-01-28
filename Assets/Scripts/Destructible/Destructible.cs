using Spawn;
using UnityEngine;

namespace Destructible
{
    public abstract class Destructible : MonoBehaviour
    {
        public abstract void OnChildTriggerEnter(Collider other, ChildTrigger childTrigger);
    }
}