using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile square;
    public Grid grid;

    public int width = 10;
    public int height = 3;

    public List<Tilemap> chunks = new List<Tilemap>();
    public List<Tilemap> corridors = new List<Tilemap>();
    public List<Tilemap> openRoofLeftDoor = new List<Tilemap>();
    public List<Tilemap> openRoofRightDoor = new List<Tilemap>();

    public int chunkWidth = 10;
    public int chunkHeight = 10;
    //private int chunkPosX = 0;
    //private int chunkPosY = 0;


    void Start()
    {
        GenerateLevel();
    }

    // button for generating the level
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Generate Path and Rooms"))
        {
            GenerateLevel();
        }
    }

    void GenerateLevel()
    {
        List<Vector2Int> path = GenerateRandomPath();
        foreach (Vector2Int cell in path)
        {
            GenerateChunk(GetChunk(0), cell.x * chunkWidth, cell.y * chunkHeight);
        }
    }

    private List<Vector2Int> GenerateRandomPath()
    {
        List<Vector2Int> path = new List<Vector2Int>();
        int startX = 0; // Start X can be anywhere within the width
        int startY = 0; // Start at the bottom
        int endX = Random.Range(0, width); // End X can also be anywhere within the width
        int endY = height; // End Y must be above startY

        // Generate path
        int currentX = startX;
        int currentY = startY;
        while (currentY < endY)
        {
            path.Add(new Vector2Int(currentX, currentY));
            // Randomly decide whether to move left, right, or stay in the same column
            int direction = Random.Range(-1, 2);
            if (direction != 0) // Move horizontally
            {
                currentX = Mathf.Clamp(currentX + direction, 0, width - 1);
            }
            else // Move vertically
            {
                currentY++;
            }
        }

        return path;
    }

    private void GenerateChunk(Tilemap chunk, int x, int y)
    {
        for (int i = 0; i < chunkWidth; i++)
        {
            for (int j = 0; j < chunkHeight; j++)
            {
                // Get the tile at the current cell
                TileBase tile = chunk.GetTile(new Vector3Int(i, j, 0));
                // Set the tile at the current cell
                tilemap.SetTile(new Vector3Int(x + i, y + j, 0), tile);
                Debug.Log("Tile position generated: " + (x + i) + ", " + (y + j));
            }
        }
    }

    Tilemap GetChunk(int index)
    {
        return chunks[index];
    }

    Tilemap GetChunkCorridor(int index)
    {
        return corridors[index];
    }
    Tilemap GetChunkOpenRoofLeftDoor(int index)
    {
        return openRoofLeftDoor[index];
    }
    Tilemap GetChunkOpenRoofRightDoor(int index)
    {
        return openRoofRightDoor[index];
    }
}
