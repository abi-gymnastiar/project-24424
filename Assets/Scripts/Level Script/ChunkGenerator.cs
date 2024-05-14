using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public Tilemap bground;
    public Tilemap chunkBg;
    public Tilemap entranceL;
    public Tilemap entranceR;

    public List<GameObject> spawnableNPCs = new List<GameObject>();

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
    public List<Tilemap> chunks_ltb = new List<Tilemap>();
    public List<Tilemap> chunks_rtb = new List<Tilemap>();
    public List<Tilemap> chunks_l = new List<Tilemap>();
    public List<Tilemap> chunks_r = new List<Tilemap>();

    public int chunkWidth = 10;
    public int chunkHeight = 10;
    //random
    public string seed;
    public bool useRandomSeed = true;

    //private int chunkPosX = 0;
    //private int chunkPosY = 0;
    private int currentFloor = 0;


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
        // debug log path positions
        foreach (Vector2Int cell in path)
        {
            Debug.Log(cell);
        }
        List<Vector2Int> entrances = GenerateEntracePositions(path);
        // debug log entrance positions
        foreach (Vector2Int cell in entrances)
        {
            Debug.Log(cell);
        }
        foreach (Vector2Int cell in path)
        {
            // random chunk index
            // int chunkIndex = Random.Range(0, 5);
            //GenerateChunk(GetChunk(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);

            // check previous and next cells
            bool hasLeft = path.Contains(new Vector2Int(cell.x - 1, cell.y)) || entrances.Contains(new Vector2Int(cell.x - 1, cell.y));
            bool hasRight = path.Contains(new Vector2Int(cell.x + 1, cell.y)) || entrances.Contains(new Vector2Int(cell.x + 1, cell.y));
            bool hasTop = path.Contains(new Vector2Int(cell.x, cell.y + 1));
            bool hasBottom = path.Contains(new Vector2Int(cell.x, cell.y - 1));

            // generate chunk based on the surrounding cells
            if (hasLeft && hasRight && hasTop && hasBottom)
            {
                int chunkIndex = Random.Range(0, chunks_lrt.Count);
                Tilemap chunkTilemap = GetChunkLRTB(chunkIndex);
                int chunkx = cell.x * chunkWidth; int chunky = cell.y * chunkHeight;
                GenerateChunk(chunkTilemap, chunkx, chunky);
            }
            else if (hasLeft && hasRight && hasTop)
            {
                int chunkIndex = Random.Range(0, chunks_lrt.Count);
                GenerateChunk(GetChunkLRT(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);
            }
            else if (hasRight && hasTop && hasBottom)
            {
                int chunkIndex = Random.Range(0, chunks_rtb.Count);
                GenerateChunk(GetChunkRTB(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);
            }
            else if (hasLeft && hasTop && hasBottom)
            {
                int chunkIndex = Random.Range(0, chunks_ltb.Count);
                GenerateChunk(GetChunkLTB(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);
            }
            else if (hasLeft && hasRight && hasBottom)
            {
                int chunkIndex = Random.Range(0, chunks_lrb.Count);
                GenerateChunk(GetChunkLRB(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);
            }
            else if (hasLeft && hasRight)
            {
                int chunkIndex = Random.Range(0, chunks_lr.Count);
                GenerateChunk(GetChunkLR(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);
            }
            else if (hasLeft && hasTop)
            {
                int chunkIndex = Random.Range(0, chunks_lt.Count);
                GenerateChunk(GetChunkLT(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);
            }
            else if (hasRight && hasTop)
            {
                int chunkIndex = Random.Range(0, chunks_rt.Count);
                GenerateChunk(GetChunkRT(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);
            }
            else if (hasTop && hasBottom)
            {
                int chunkIndex = Random.Range(0, chunks_tb.Count);
                GenerateChunk(GetChunkTB(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);
            }
            else if (hasLeft && hasBottom)
            {
                int chunkIndex = Random.Range(0, chunks_lb.Count);
                GenerateChunk(GetChunkLB(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);
            }
            else if (hasRight && hasBottom)
            {
                int chunkIndex = Random.Range(0, chunks_rb.Count);
                GenerateChunk(GetChunkRB(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);
            }
            else if (hasRight)
            {
                int chunkIndex = Random.Range(0, chunks_r.Count);
                GenerateChunk(GetChunkR(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);
            }
            else if (hasLeft)
            {
                int chunkIndex = Random.Range(0, chunks_l.Count);
                GenerateChunk(GetChunkL(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);
            }
            else
            {
                int chunkIndex = Random.Range(0, chunks.Count);
                GenerateChunk(GetChunk(chunkIndex), cell.x * chunkWidth, cell.y * chunkHeight);
            }
        }
    }

    private List<Vector2Int> GenerateRandomPath()
    {
        List<Vector2Int> path = new List<Vector2Int>();
        int startX = 0; // Start X can be anywhere within the width
        int startY = 0; // Start at the bottom
        //int endX = Random.Range(0, width); // End X can also be anywhere within the width
        int endY = height; // End Y must be above startY

        // Generate path
        int currentX = startX;
        int currentY = startY;
        HashSet<Vector2Int> visitedCells = new HashSet<Vector2Int>(); // Track visited cells
        while (currentY < endY)
        {
            path.Add(new Vector2Int(currentX, currentY));
            visitedCells.Add(new Vector2Int(currentX, currentY));

            // Randomly decide whether to move left, right, or stay in the same column
            int direction = Random.Range(-1, 2);
            if (direction != 0) // Move horizontally
            {
                int nextX = currentX + direction;
                // Check if the next cell is already visited
                if (nextX >= 0 && nextX < width && !visitedCells.Contains(new Vector2Int(nextX, currentY)))
                {
                    currentX = nextX;
                }
                // if the next cell is already visited, try moving the other direction
                else if (currentX - direction >= 0 && currentX - direction < width)
                {
                    currentX = currentX - direction;
                }
                // move vertically if both directions are blocked
                else
                {
                    currentY++;
                }
            }
            else // Move vertically
            {
                currentY++;
            }
        }

        return path;
    }

    private List<Vector2Int> GenerateEntracePositions(List<Vector2Int> ogPath)
    {
        List<Vector2Int> entrancePos = new List<Vector2Int>();
        if (currentFloor == 0)
        {
            entrancePos.Add(new Vector2Int(-1, 0));
        }
        foreach (Vector2Int cell in ogPath)
        {
            bool hasLeft = ogPath.Contains(new Vector2Int(cell.x - 1, cell.y));
            bool hasRight = ogPath.Contains(new Vector2Int(cell.x + 1, cell.y));
            if (!hasLeft && cell.x > 0)
            {
                int random = Random.Range(0, 4);
                if (random < 3)
                {
                    entrancePos.Add(new Vector2Int(cell.x - 1, cell.y));
                }
            }
            if (!hasRight && cell.x < width - 1)
            {
                int random = Random.Range(0, 4);
                if (random < 3)
                {
                    entrancePos.Add(new Vector2Int(cell.x + 1, cell.y));
                }
            }
        }
        return entrancePos;
    }

    private void GenerateChunk(Tilemap chunk, int x, int y)
    {
        bground.SetTilesBlock(new BoundsInt(x, y, 0, chunkWidth, chunkHeight, 1), chunkBg.GetTilesBlock(new BoundsInt(0, 0, 0, chunkWidth, chunkHeight, 1)));
        tilemap.SetTilesBlock(new BoundsInt(x, y, 0, chunkWidth, chunkHeight, 1), chunk.GetTilesBlock(new BoundsInt(0, 0, 0, chunkWidth, chunkHeight, 1)));
    }

    private void GenerateEntrance(int direction, int x, int y)
    {
        if (direction == -1)
        {
            tilemap.SetTilesBlock(new BoundsInt(x+15, y, 0, 5, 5, 1), entranceL.GetTilesBlock(new BoundsInt(0, 0, 0, 5, 5, 1)));
        }
        else if (direction == 1)
        {
            tilemap.SetTilesBlock(new BoundsInt(x, y, 0, 5, 5, 1), entranceR.GetTilesBlock(new BoundsInt(0, 0, 0, 5, 5, 1)));
        }
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
    Tilemap GetChunkLTB(int index)
    {
        return chunks_ltb[index];
    }
    Tilemap GetChunkRTB(int index)
    {
        return chunks_rtb[index];
    }
    Tilemap GetChunkL(int index)
    {
        return chunks_l[index];
    }
    Tilemap GetChunkR(int index)
    {
        return chunks_r[index];
    }
}
