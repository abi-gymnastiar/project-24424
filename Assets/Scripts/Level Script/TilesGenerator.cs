using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TilesGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile square;
    public Grid grid;

    public int width = 50;
    public int height = 50;

    public string seed;
    public bool useRandomSeed = true;

    [Range(0, 100)] public int fillPercent = 50;

    public int minWallsForSmooth = 4;
    public int maxWallsForNull = 4;

    public int horizontalStretch = 2;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Generate"))
        {
            tilemap.ClearAllTiles();
            for (int i = 0; i < 6; i++)
            {
                tilemap.SetTile(new Vector3Int(-i-1, -1, 0), square);
            }
            GenerateLevel();
        }
        if (GUI.Button(new Rect(10, 50, 100, 30), "Smooth"))
        {
            CelularAutomataSmooth();
        }
        if (GUI.Button(new Rect(10, 90, 100, 30), "Stretch"))
        {
            Stretch();
        }
    }

    void Start()
    {
        GenerateLevel();
        //EnsureConnectivity();
    }

    void GenerateLevel()
    {
        // If useRandomSeed is true, generate a random seed
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }
        // Generate a random seed based on the current time
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        // Loop through each cell of the tilemap and set the ground tile
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // border tiles
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    tilemap.SetTile(tilePosition, square);
                }
                else
                {
                    // Generate a random number between 0 and 100
                    int random = pseudoRandom.Next(0, 100);
                    // If the random number is less than the fill percent, set the tile
                    if (random < fillPercent)
                    {
                        Vector3Int tilePosition = new Vector3Int(x, y, 0);
                        tilemap.SetTile(tilePosition, square);
                    }
                }
            }
        }
    }

    void CelularAutomataSmooth()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                if (neighbourWallTiles > minWallsForSmooth)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), square);
                }
                else if (neighbourWallTiles < maxWallsForNull)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }   
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        if (tilemap.GetTile(new Vector3Int(neighbourX, neighbourY, 0)) == square)
                        {
                            wallCount++;
                        }
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    void Stretch()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int sum = 0;
                for (int offsetX = -horizontalStretch; offsetX <= horizontalStretch; offsetX++)
                {
                    int nx = Mathf.Clamp(x + offsetX, 0, width - 1);
                    if (tilemap.GetTile(new Vector3Int(nx, y, 0)) == square)
                    {
                        sum++;
                    }
                }
                // If the sum is greater than half the width of the stretch, set the tile
                tilemap.SetTile(new Vector3Int(x, y, 0), sum > horizontalStretch ? square : null);
            }
        }
    }
}
