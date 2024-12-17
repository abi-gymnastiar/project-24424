using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusSpawner : MonoBehaviour
{
    [SerializeField] private Transform followTransform;
    [SerializeField] private GameObject Prefab;

    // radius
    private float radius = 15f;

    private void Start()
    {
        // start spawn
        StartCoroutine(Spawn());
    }

    private void Update()
    {
        // follow
        transform.position = followTransform.position;
    }

    // enumerator
    private IEnumerator Spawn()
    {
        while (true)
        {
            // random position
            Vector3 randomPosition = Random.insideUnitSphere * radius;
            randomPosition.y = 0f;

            // instantiate
            Instantiate(Prefab, transform.position + randomPosition, Quaternion.identity);

            // wait
            yield return new WaitForSeconds(10f);
        }
    }
}
