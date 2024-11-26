using System;
using Spawn;
using UnityEngine;

namespace Destructible
{
    public class Coin : Destructible
    {
        public void OnTriggerEnter(Collider other)
        {
            gameObject.SetActive(false);
        }

        public override void OnChildTriggerEnter(Collider other, ChildTrigger childTrigger)
        {
            throw new NotImplementedException();
        }
    }
}