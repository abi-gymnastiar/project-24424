using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomsGeneration : MonoBehaviour
{
    public Grid grid;
    public Tilemap tilemap;
    public Tilemap backGround;
    public Tilemap environment;
    public Tilemap platform;

    List<GameObject> rooms = new List<GameObject>();
    public Tilemap bground;
    public Tilemap roomBg;
    public Tilemap entranceL;
    public Tilemap entranceR;

    public List<GameObject> rooms_lr = new List<GameObject>();
    public List<GameObject> rooms_lt = new List<GameObject>();
    public List<GameObject> rooms_rt = new List<GameObject>();
    public List<GameObject> rooms_lrt = new List<GameObject>();
    public List<GameObject> rooms_lrtb = new List<GameObject>();
    public List<GameObject> rooms_lb = new List<GameObject>();
    public List<GameObject> rooms_rb = new List<GameObject>();
    public List<GameObject> rooms_lrb = new List<GameObject>();
    public List<GameObject> rooms_tb = new List<GameObject>();
    public List<GameObject> rooms_ltb = new List<GameObject>();
    public List<GameObject> rooms_rtb = new List<GameObject>();
    public List<GameObject> rooms_l = new List<GameObject>();
    public List<GameObject> rooms_r = new List<GameObject>();
    public List<GameObject> rooms_wide = new List<GameObject>();

    public int width = 10;
    public int height = 3;

    public int roomWidth = 10;
    public int roomHeight = 10;

    public string seed;
    public bool useRandomSeed = true;

    private int currentFloor = 0;

    public GameObject testRoom;

    // Start is called before the first frame update
    void Start()
    {
        //test room
        //GenerateTheRoom(new Vector2Int(0, 0), testRoom, new Vector2Int(0, 0));

        GenerateRooms();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Generate Path and Rooms"))
        {
            GenerateRooms();
        }
    }

    private void GenerateRooms()
    {
        List<Vector2Int> path = GeneratePath();
        // debug log path positions
        foreach (Vector2Int cell in path)
        {
            Debug.Log(cell);
        }
        List<Vector2Int> entrances = GenerateEntracePositions(path);
        // loop through path and create rooms
        foreach (Vector2Int cell in path)
        {
            // check previous and next cells
            bool hasLeft = path.Contains(new Vector2Int(cell.x - 1, cell.y)) || entrances.Contains(new Vector2Int(cell.x - 1, cell.y));
            bool hasRight = path.Contains(new Vector2Int(cell.x + 1, cell.y)) || entrances.Contains(new Vector2Int(cell.x + 1, cell.y));
            bool hasTop = path.Contains(new Vector2Int(cell.x, cell.y + 1));
            bool hasBottom = path.Contains(new Vector2Int(cell.x, cell.y - 1));
            Vector2Int roomPos = new Vector2Int(cell.x * roomWidth, cell.y * roomHeight);
            // generate chunk based on the surrounding cells
            if (hasLeft && hasRight && hasTop && hasBottom)
            {
                int roomIndex = Random.Range(0, rooms_lrtb.Count);
                GenerateTheRoom(cell, rooms_lrtb[roomIndex], roomPos);
            }
            else if (hasLeft && hasRight && hasTop)
            {
                int roomIndex = Random.Range(0, rooms_lrt.Count);
                GenerateTheRoom(cell, rooms_lrt[roomIndex], roomPos);
            }
            else if (hasRight && hasTop && hasBottom)
            {
                int roomIndex = Random.Range(0, rooms_rtb.Count);
                GenerateTheRoom(cell, rooms_rtb[roomIndex], roomPos);
            }
            else if (hasLeft && hasTop && hasBottom)
            {
                int roomIndex = Random.Range(0, rooms_ltb.Count);
                GenerateTheRoom(cell, rooms_ltb[roomIndex], roomPos);
            }
            else if (hasLeft && hasRight && hasBottom)
            {
                int roomIndex = Random.Range(0, rooms_lrb.Count);
                GenerateTheRoom(cell, rooms_lrb[roomIndex], roomPos);
            }
            else if (hasLeft && hasRight)
            {
                int roomIndex = Random.Range(0, rooms_lr.Count);
                GenerateTheRoom(cell, rooms_lr[roomIndex], roomPos);
            }
            else if (hasLeft && hasTop)
            {
                int roomIndex = Random.Range(0, rooms_lt.Count);
                GenerateTheRoom(cell, rooms_lt[roomIndex], roomPos);
            }
            else if (hasRight && hasTop)
            {
                int roomIndex = Random.Range(0, rooms_rt.Count);
                GenerateTheRoom(cell, rooms_rt[roomIndex], roomPos);
            }
            else if (hasTop && hasBottom)
            {
                int roomIndex = Random.Range(0, rooms_tb.Count);
                GenerateTheRoom(cell, rooms_tb[roomIndex], roomPos);
            }
            else if (hasLeft && hasBottom)
            {
                int roomIndex = Random.Range(0, rooms_lb.Count);
                GenerateTheRoom(cell, rooms_lb[roomIndex], roomPos);
            }
            else if (hasRight && hasBottom)
            {
                int roomIndex = Random.Range(0, rooms_rb.Count);
                GenerateTheRoom(cell, rooms_rb[roomIndex], roomPos);
            }
            else if (hasRight)
            {
                int roomIndex = Random.Range(0, rooms_r.Count);
                GenerateTheRoom(cell, rooms_rb[roomIndex], roomPos);
            }
            else if (hasLeft)
            {
                int roomIndex = Random.Range(0, rooms_l.Count);
                GenerateTheRoom(cell, rooms_l[roomIndex], roomPos);
            }
            else
            {
                Debug.Log("Room has no surrounding cells ????");
                int roomIndex = Random.Range(0, rooms.Count);
                GenerateTheRoom(cell, rooms[roomIndex], roomPos);
            }
        }
    }

    private List<Vector2Int> GeneratePath()
    {
        List<Vector2Int> path = new List<Vector2Int>();
        int startX = 0; // Start X can be anywhere within the width
        int startY = 0; // Start at the bottom
        //int endX = 0; // End X can also be anywhere within the width
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

    private void GenerateTheRoom(Vector2Int cell, GameObject room, Vector2Int roomPos)
    {
        Tilemap roomBgTilemap = room.GetComponentsInChildren<Tilemap>()[0];
        backGround.SetTilesBlock(new BoundsInt(roomPos.x, roomPos.y, 0, roomWidth, roomHeight, 1), roomBgTilemap.GetTilesBlock(new BoundsInt(0, 0, 0, roomWidth, roomHeight, 1)));
        Tilemap tm = room.GetComponentsInChildren<Tilemap>()[1];
        tilemap.SetTilesBlock(new BoundsInt(roomPos.x, roomPos.y, 0, roomWidth, roomHeight, 1), tm.GetTilesBlock(new BoundsInt(0, 0, 0, roomWidth, roomHeight, 1)));
        // check if room has 3rd tilemap for platform
        if (room.GetComponentsInChildren<Tilemap>().Length > 2)
        {
            Tilemap platTm = room.GetComponentsInChildren<Tilemap>()[2];
            platform.SetTilesBlock(new BoundsInt(roomPos.x, roomPos.y, 0, roomWidth, roomHeight, 1), platTm.GetTilesBlock(new BoundsInt(0, 0, 0, roomWidth, roomHeight, 1)));
        }
        List<SpawnerNPC> spawner = room.GetComponentsInChildren<SpawnerNPC>().ToList();
        foreach (SpawnerNPC s in spawner)
        {
            s.SpawnNPC(roomPos.x, roomPos.y);
        }
    }
    private void GenerateTheRoom(Vector2Int cell, GameObject room, Vector2Int roomPos, int width, int height)
    {
        Tilemap roomBgTilemap = room.GetComponentsInChildren<Tilemap>()[0];
        backGround.SetTilesBlock(new BoundsInt(roomPos.x, roomPos.y, 0, width, height, 1), roomBgTilemap.GetTilesBlock(new BoundsInt(0, 0, 0, width, height, 1)));
        Tilemap tm = room.GetComponentsInChildren<Tilemap>()[1];
        tilemap.SetTilesBlock(new BoundsInt(roomPos.x, roomPos.y, 0, width, height, 1), tm.GetTilesBlock(new BoundsInt(0, 0, 0, width, height, 1)));
    }
}
