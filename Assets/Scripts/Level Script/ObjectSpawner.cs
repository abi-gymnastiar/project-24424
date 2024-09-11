using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject spawnObject;
    // spawn chance int with slider for the inspector
    [Range(0, 100)] public int spawnChance = 100;

    public void SpawnObject()
    {
        Instantiate(spawnObject, transform.position, Quaternion.identity);
    }
    public void SpawnObject(float x, float y)
    {
        // based on the spawn chance, we will spawn the object
        if (Random.Range(0, 100) > spawnChance)
            return;
        Vector2 position = new Vector2(x + transform.position.x, y + transform.position.y);
        Instantiate(spawnObject, position, Quaternion.identity);
    }
}
