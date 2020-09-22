using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGen : MonoBehaviour
{

    public Grid tileGrid, moveGrid;
    GridManager tileGMgr, moveGMgr;
    public Tile ground, grass1, grass2, grass3, grass4, grass5, grass6, grass7, grass8, grass9;
    public Tilemap groundMap, grassMap, obstacleMap;
    const int SIZE = 20, MAX_STEPS = 6;
    Vector3Int currentPos;
    List<Vector3Int> grassSeeds;

    // Start is called before the first frame update
    void Start()
    {
        grassSeeds = new List<Vector3Int>();
        tileGMgr = new GridManager(tileGrid);
        moveGMgr = new GridManager(moveGrid);

        //Generate base tiles
        for(int i = -SIZE; i < SIZE; i++)
        {
            for(int j = -SIZE; j < SIZE; j++)
            {
                groundMap.SetTile(new Vector3Int(i, j, 0), ground);
            }
        }

        //Generate grass seeds
        for (int i = -SIZE; i < SIZE; i++)
        {
            for (int j = -SIZE; j < SIZE; j++)
            {
                if ((!checkAdjacents(new Vector2Int(i, j), 20) && Chance.Percent(1) && !grassMap.HasTile(new Vector3Int(i, j, 0))))
                {
                    grassMap.SetTile(new Vector3Int(i, j, 0), grass5);
                    grassSeeds.Add(new Vector3Int(i, j, 0));
                }
            }
        }

        //Create grass branches
        foreach(Vector3Int seedPos in grassSeeds)
        {
            int numSteps;
            int xDir = 0;
            int yDir = 0;
            bool lastWasDiag = false;
            Vector3Int movMod;

            for (int i = 0; i < 3; i++)
            {
                switch(i)
                {
                    case 0:
                        xDir = -1;
                        yDir = 0;
                        break;
                    case 1:
                        xDir = 0;
                        yDir = 1;
                        break;
                    case 2:
                        xDir = 1;
                        yDir = 0;
                        break;
                    case 3:
                        xDir = 0;
                        yDir = -1;
                        break;
                }
                numSteps = Random.Range(1, MAX_STEPS);
                movMod = new Vector3Int(xDir, yDir, 0);
                currentPos = seedPos + movMod;
                for (int j = 0; j < numSteps; j++)
                {
                    grassMap.SetTile(currentPos, grass5);
                    currentPos += movMod;
                    if (!lastWasDiag && Chance.Percent(45))
                    {
                        if (xDir != 0) currentPos.y += Random.Range(-1, 1);
                        else currentPos.x += Random.Range(-1, 1);
                        lastWasDiag = true;
                    }
                    else lastWasDiag = false;
                }

            }

        }

        //Fill in patches
        foreach (Vector3Int seedPos in grassSeeds)
        {
            for (int i = 0; i < 1; i++)
            {
                for (int x = MAX_STEPS*-2; x < MAX_STEPS*2; x++)
                {
                    for (int y = MAX_STEPS*-2; y < MAX_STEPS*2; y++)
                    {
                        Debug.Log("x " + (seedPos.x + x) + "y " + (seedPos.y + y));
                        currentPos = new Vector3Int(seedPos.x + x, seedPos.y - y, 0);
                        if (numAdjacents(currentPos) >= 3)
                        {
                            grassMap.SetTile(currentPos, grass5);
                        }
                    }
                }
            }
        }
    }
    int numAdjacents(Vector3Int pos)
    {
        int counter = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3Int tspot = new Vector3Int(pos.x + i, pos.y + j, 0);
                Debug.Log("i " + (pos.x + i) + "j " + (pos.y + j) + "pos " + tspot + "cond " + grassMap.HasTile(tspot));
                if (grassMap.HasTile(tspot)) counter++;
            }
        }
        return counter;
    }

    bool checkAdjacents(Vector2Int pos, int width)
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < width; j++)
            {
                Vector3Int t = new Vector3Int(pos.x - (width-2) + i, pos.y - (width - 2) + j, 0);
                if (grassMap.HasTile(t)) return true;
            }
        }
        return false;
    }
}
