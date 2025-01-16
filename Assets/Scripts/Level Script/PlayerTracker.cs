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
    public GameObject gameOverPanel;

    // subscribe to playerDead event
    private void OnEnable()
    {
        PlayerController.playerDead += PlayerDead;
    }

    // Update is called once per frame
    void Update()
    {
        if (grid.WorldToCell(player.transform.position).y >= floorHeight*floorHeightGen)
        {
            floorHeightGen += 12;
            Debug.Log("generating");
            tilesGenerator.y_pos += tilesGenerator.height;
            tilesGenerator.GenerateAndSmooth(floorHeightGen - 4);
        }
    }

    // player dead event
    private void PlayerDead()
    {
        Debug.Log("Player dead");
        // pause the game
        Time.timeScale = 0;
        
        Instantiate(gameOverPanel, FindAnyObjectByType<Canvas>().transform);
    }
}
