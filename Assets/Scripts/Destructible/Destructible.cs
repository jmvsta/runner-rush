using Spawn;
using UnityEngine;

namespace Destructible
{
    public class Destructible : MonoBehaviour
    {
        protected ExplosionsSpawner ExplosionsSpawner;
        
        void Start()
        {
            ExplosionsSpawner = GameObject.Find("ExplosionsSpawner").GetComponent<ExplosionsSpawner>();
        }
    }
}