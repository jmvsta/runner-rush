using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 10;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPosition.forward * bulletSpeed;
        }
    }
}
