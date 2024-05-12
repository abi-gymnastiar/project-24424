using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject sparkEffect;

    public void SpawnBullet()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        sparkEffect.GetComponent<Animator>().SetTrigger("PistolShoot");
    }
}
