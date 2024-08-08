using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BuildingGeneration : MonoBehaviour
{
    public Grid grid;
    public Tilemap backgrounds;
    public Tilemap tm;
    public Tilemap platforms;
    public Tilemap envTm;

    public RuleTile groundRuleTile;

    public int width = 5;
    public int height = 10;

    public int roomWidth = 20;
    public int roomHeight = 20;

    public string seed;
    public bool useRandomSeed = true;

    //private int currentFloor = 0;

    public List<GameObject> buildingChunks;
    public List<GameObject> store;
    public GameObject placeHolderForRooms;
    // Start is called before the first frame update
/*    void Start()
    {
        GenerateBuildings();
    }*/

    public void GenerateBuildings(int height_start)
    {
        List<Vector2Int> path = GeneratePath(height_start);
        // loop the path list
        foreach (Vector2Int cell in path)
        {
            Debug.Log(cell);
        }
        foreach (Vector2Int cell in path)
        {
            // check previous and next cells
            bool hasLeft = path.Contains(new Vector2Int(cell.x - 1, cell.y));
            bool hasRight = path.Contains(new Vector2Int(cell.x + 1, cell.y));
            bool hasTop = path.Contains(new Vector2Int(cell.x, cell.y + 1));
            bool hasBottom = path.Contains(new Vector2Int(cell.x, cell.y - 1));
            Vector2Int roomPos = new Vector2Int(cell.x * roomWidth, cell.y * roomHeight);

            if (!hasBottom)
            {
                for (int y = roomPos.y; y > roomPos.y - 5; y--)
                {
                    int leftX = roomPos.x;
                    int rightX = roomPos.x + roomWidth;
                    if (!hasLeft)
                    {
                        leftX -= 5;
                    }
                    if (!hasRight)
                    {
                        rightX += 5;
                    }
                    for (int x = leftX; x < rightX; x++)
                    {
                        tm.SetTile(new Vector3Int(x, y, 0), groundRuleTile);
                    }
                }
            } 
            if (hasBottom || hasTop)
            {
                int index = Random.Range(0, buildingChunks.Count);
                GenerateTheRoom(roomPos, buildingChunks[index]);
            }
            else if (hasLeft)
            {
                int index = Random.Range(0, store.Count);
                GenerateTheRoom(roomPos, store[index]);
            }
        }
    }

    private List<Vector2Int> GeneratePath(int height_start)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        int startX = 0; // Start X can be anywhere within the width
        int startY = height_start; // Start at the bottom
        //int endX = 0; // End X can also be anywhere within the width
        int endY = height_start + height; // End Y must be above startY

        // Generate path
        int currentX = startX;
        int currentY = startY;
        HashSet<Vector2Int> visitedCells = new HashSet<Vector2Int>(); // Track visited cells
        while (currentY < endY)
        {
            path.Add(new Vector2Int(currentX, currentY));
            visitedCells.Add(new Vector2Int(currentX, currentY));

            // Randomly decide whether to move left, right, or stay in the same column
            int direction = Random.Range(0, 2);
            if (direction == 0)
            {
                if (currentX > 1)
                {
                    int length = Random.Range(2, currentX);
                    for (int i = 0; i < length; i++)
                    {
                        currentX--;
                        path.Add(new Vector2Int(currentX, currentY));
                        visitedCells.Add(new Vector2Int(currentX, currentY));
                    }
                }
                else
                {
                    int length = Random.Range(2, width - currentX);
                    for (int i = 0; i < length; i++)
                    {
                        currentX++;
                        path.Add(new Vector2Int(currentX, currentY));
                        visitedCells.Add(new Vector2Int(currentX, currentY));
                    }
                }
            }
            else if (direction == 1)
            {
                if (currentX < width - 1)
                {
                    int length = Random.Range(2, width - currentX);
                    for (int i = 0; i < length; i++)
                    {
                        currentX++;
                        path.Add(new Vector2Int(currentX, currentY));
                        visitedCells.Add(new Vector2Int(currentX, currentY));
                    }
                }
                else
                {
                    int length = Random.Range(2, currentX);
                    for (int i = 0; i < length; i++)
                    {
                        currentX--;
                        path.Add(new Vector2Int(currentX, currentY));
                        visitedCells.Add(new Vector2Int(currentX, currentY));
                    }
                }
            }
            int randomY = Random.Range(2, 5);
            if (currentY+randomY < endY)
            {
                for (int i = 0; i < randomY; i++)
                {
                    path.Add(new Vector2Int(currentX, currentY+i));
                    visitedCells.Add(new Vector2Int(currentX, currentY));
                }
            }
            int tempY = Random.Range(currentY+2, currentY+randomY);
            currentY = tempY;
        }

        return path;
    }

    private List<Vector2Int> GenerateRandomPath()
    {
        List<Vector2Int> path = new List<Vector2Int>();
        int startX = 0; // Start X can be anywhere within the width
        int startY = 0; // Start at the bottom
        //int endX = 0; // End X can also be anywhere within the width
        int endY = height; // End Y must be above startY

        // Generate path
        int currentX = startX;
        int currentY = startY;



        return path;
    }


    private void GenerateTheRoom(Vector2Int roomPos, GameObject room)
    {
        Tilemap roombgTm = room.GetComponentsInChildren<Tilemap>()[0];
        backgrounds.SetTilesBlock(new BoundsInt(roomPos.x, roomPos.y, 0, roomWidth, roomHeight, 1), roombgTm.GetTilesBlock(new BoundsInt(0, 0, 0, roomWidth, roomHeight, 1)));
        Tilemap concreteTm = room.GetComponentsInChildren<Tilemap>()[1];
        tm.SetTilesBlock(new BoundsInt(roomPos.x, roomPos.y, 0, roomWidth, roomHeight, 1), concreteTm.GetTilesBlock(new BoundsInt(0, 0, 0, roomWidth, roomHeight, 1)));
        Tilemap platformTm = room.GetComponentsInChildren<Tilemap>()[2];
        platforms.SetTilesBlock(new BoundsInt(roomPos.x, roomPos.y, 0, roomWidth, roomHeight, 1), platformTm.GetTilesBlock(new BoundsInt(0, 0, 0, roomWidth, roomHeight, 1)));
        List<SpawnerNPC> spawner = room.GetComponentsInChildren<SpawnerNPC>().ToList();
        foreach (SpawnerNPC s in spawner)
        {
            s.SpawnNPC(roomPos.x, roomPos.y);
        }
    }
}
