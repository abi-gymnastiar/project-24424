using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public List<Tilemap> chunks_lr = new List<Tilemap>();
    public List<Tilemap> chunks_lt = new List<Tilemap>();
    public List<Tilemap> chunks_rt = new List<Tilemap>();
    public List<Tilemap> chunks_lrt = new List<Tilemap>();
    public List<Tilemap> chunks_lrtb = new List<Tilemap>();
    public List<Tilemap> chunks_lb = new List<Tilemap>();
    public List<Tilemap> chunks_rb = new List<Tilemap>();
    public List<Tilemap> chunks_lrb = new List<Tilemap>();
    public List<Tilemap> chunks_tb = new List<Tilemap>();

    public int chunkWidth = 10;
    public int chunkHeight = 10;
    //random
    public string seed;
    public bool useRandomSeed = true;

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
            // random chunk index
            int chunkIndex = Random.Range(0, 5);
            GenerateChunk(GetChunk(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);

            // check previous and next cells
            //bool hasLeft = path.Contains(new Vector2Int(cell.x - 1, cell.y));
            //bool hasRight = path.Contains(new Vector2Int(cell.x + 1, cell.y));
            //bool hasTop = path.Contains(new Vector2Int(cell.x, cell.y + 1));
            //bool hasBottom = path.Contains(new Vector2Int(cell.x, cell.y - 1));

            // generate chunk based on the surrounding cells
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
        tilemap.SetTilesBlock(new BoundsInt(x, y, 0, chunkWidth, chunkHeight, 1), chunk.GetTilesBlock(new BoundsInt(0, 0, 0, chunkWidth, chunkHeight, 1)));
    }

    Tilemap GetChunk(int index)
    {
        return chunks[index];
    }

    Tilemap GetChunkLR(int index)
    {
        return chunks_lr[index];
    }
    Tilemap GetChunkLT(int index)
    {
        return chunks_lt[index];
    }
    Tilemap GetChunkRT(int index)
    {
        return chunks_rt[index];
    }
    Tilemap GetChunkLRT(int index)
    {
        return chunks_lrt[index];
    }
    Tilemap GetChunkLRTB(int index)
    {
        return chunks_lrtb[index];
    }
    Tilemap GetChunkLB(int index)
    {
        return chunks_lb[index];
    }
    Tilemap GetChunkRB(int index)
    {
        return chunks_rb[index];
    }
    Tilemap GetChunkLRB(int index)
    {
        return chunks_lrb[index];
    }
    Tilemap GetChunkTB(int index)
    {
        return chunks_tb[index];
    }
}
