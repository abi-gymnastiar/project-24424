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
    private int chunkPosX = 0;
    private int chunkPosY = 0;


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
        int x = 0;
        int prevX = 0;

        // Loop through each cell of the tilemap and set the ground tile
        for (int y = 0; y < height; y++)
        {
            chunkPosY = y * chunkHeight;
            while (x < width || x >= 0)
            {
                chunkPosX = x * chunkWidth;
                int randomChunkIndex = Random.Range(0, chunks.Count);
                GenerateChunk(GetChunk(randomChunkIndex), chunkPosX, chunkPosY);
                Debug.Log("Chunk grid generated: " + x + ", " + y);
                // going up?
                int random = Random.Range(0, 3);
                Debug.Log("Random1: " + random);
                if (random == 1 || (prevX < x && x == width-1) || (prevX > x && x == 0))
                {
                    prevX = x;
                    break;
                }
                // going left or right?
                if (prevX == x)
                {
                    if (x > 0 && x < width - 1)
                    {
                        random = Random.Range(0, 2);
                        Debug.Log("Random2: " + random);
                        if (random == 1)
                        {
                            prevX = x;
                            x++;
                        }
                        else
                        {
                            prevX = x;
                            x--;
                        }
                    }
                    else if (x == 0)
                    {
                        prevX = x;
                        x++;
                    }
                    else if (x == width - 1)
                    {
                        prevX = x;
                        x--;
                    }
                }
                else if (prevX < x)
                {
                    prevX = x;
                    x++;
                }
                else if (prevX > x)
                {
                    prevX = x;
                    x--;
                }
            }
        }
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
                //Debug.Log("Tile position generated: " + (x + i) + ", " + (y + j));
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
