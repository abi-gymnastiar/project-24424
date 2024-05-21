using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerNPC : MonoBehaviour
{
    public GameObject npc;
    // spawn chance int with slider for the inspector
    [Range(0, 100)] public int spawnChance = 100;

    public void SpawnNPC()
    {
        Instantiate(npc, transform.position, Quaternion.identity);
    }
    public void SpawnNPC(float x, float y)
    {
        Vector2 position = new Vector2(x + transform.position.x, y + transform.position.y);
        Instantiate(npc, position, Quaternion.identity);
    }
}
