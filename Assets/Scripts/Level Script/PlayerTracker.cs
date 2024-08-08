using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    public PlayerController player;
    public Grid grid;
    public TilesGenerator tilesGenerator;

    private int floorHeight = 20;
    private int floorHeightGen = 4;

    // Update is called once per frame
    void Update()
    {
        if (grid.WorldToCell(player.transform.position).y >= floorHeight*floorHeightGen)
        {
            floorHeightGen += 12;
            Debug.Log("generating");
            tilesGenerator.temp_y += tilesGenerator.height;
            tilesGenerator.GenerateAndSmooth(floorHeightGen - 4);
        }
    }
}
