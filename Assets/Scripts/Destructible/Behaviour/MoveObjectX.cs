using UnityEngine;
using Random = System.Random;

namespace Destructible.Behaviour
{
    public class MoveObjectX : MonoBehaviour
    {
        
        [SerializeField] private float _speed = 10.0f;
        private readonly Random _random = new();

        private void FixedUpdate()
        {
            if (transform.position.x < -5)
            {
                transform.position += Vector3.right * _random.Next(0, 6) * Time.fixedDeltaTime;
            }
            else if (transform.position.x > 5)
            {
                transform.position += Vector3.right * _random.Next(-5, 1) * Time.fixedDeltaTime;
            }
            else
            {
                transform.position += Vector3.right * _random.Next(-2, 3) * _speed * Time.fixedDeltaTime;
            }
            
        }
    }
}