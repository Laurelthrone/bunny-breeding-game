using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGen : MonoBehaviour
{

    public Grid tileGrid, moveGrid;
    GridManager tileGMgr, moveGMgr;
    public Tile ground,
                grassTLC, grassT, grassTRC,
                grassL, grassM, grassR,
                grassBLC, grassB, grassBRC,
                grassIBL, grassIBR, grassITL, grassITR;

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
        for (int i = -SIZE; i < SIZE; i++)
        {
            for (int j = -SIZE; j < SIZE; j++)
            {
                groundMap.SetTile(new Vector3Int(i, j, 0), ground);
            }
        }

        //Generate grass seeds
        for (int i = -SIZE; i < SIZE; i++)
        {
            for (int j = -SIZE; j < SIZE; j++)
            {
                if ((!spacing(new Vector2Int(i, j), 20) && Chance.Percent(1) && !grassMap.HasTile(new Vector3Int(i, j, 0))))
                {
                    grassMap.SetTile(new Vector3Int(i, j, 0), grassM);
                    grassSeeds.Add(new Vector3Int(i, j, 0));
                }
            }
        }

        //Create grass branches
        foreach (Vector3Int seedPos in grassSeeds)
        {
            int numSteps;
            int xDir = 0;
            int yDir = 0;
            bool lastWasDiag = false;
            Vector3Int movMod;

            for (int i = 0; i < 3; i++)
            {
                switch (i)
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
                    grassMap.SetTile(currentPos, grassM);
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
                for (int x = MAX_STEPS * -2; x < MAX_STEPS * 2; x++)
                {
                    for (int y = MAX_STEPS * -2; y < MAX_STEPS * 2; y++)
                    {
                        Debug.Log("x " + (seedPos.x + x) + "y " + (seedPos.y + y));
                        currentPos = new Vector3Int(seedPos.x + x, seedPos.y - y, 0);

                        //Fill empty tiles that have at least 3 adjacent tiles
                        if (numAdjacents(currentPos) >= 3)
                        {
                            grassMap.SetTile(currentPos, grassM);
                        }
                    }
                }
            }
        }

        //Cleanup first pass (outline)
        foreach (Vector3Int seedPos in grassSeeds)
        {
            for (int x = MAX_STEPS * -2; x < MAX_STEPS * 2; x++)
            {
                for (int y = MAX_STEPS * -2; y < MAX_STEPS * 2; y++)
                {
                    determineOutline(new Vector3Int(x + seedPos.x, y + seedPos.y, 0));
                }
            }
        }

        //Cleanup second pass (inner corner)
        foreach (Vector3Int seedPos in grassSeeds)
        {
            for (int x = MAX_STEPS * -2; x < MAX_STEPS * 2; x++)
            {
                for (int y = MAX_STEPS * -2; y < MAX_STEPS * 2; y++)
                {
                    findInnerCorners(new Vector3Int(x + seedPos.x, y + seedPos.y, 0));
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
                if (grassMap.HasTile(tspot) && tspot != pos) counter++;
            }
        }
        return counter;
    }

    bool spacing(Vector2Int pos, int width)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector3Int t = new Vector3Int(pos.x - (width - 2) + i, pos.y - (width - 2) + j, 0);
                if (grassMap.HasTile(t)) return true;
            }
        }
        return false;
    }

    bool checkTile(Vector3Int a, string dir, string diag)
    {
        int yOffset;
        if (diag == "up") yOffset = 1;
        else yOffset = 0;

        switch (dir)
        {
            case "up":
                return grassMap.HasTile(new Vector3Int(a.x, a.y + 1, 0));
            case "down":
                return grassMap.HasTile(new Vector3Int(a.x, a.y - 1, 0));
            case "left":
                return grassMap.HasTile(new Vector3Int(a.x - 1, a.y + yOffset, 0));
            case "right":
                return grassMap.HasTile(new Vector3Int(a.x + 1, a.y + yOffset, 0));
        }
        return false;
    }

    bool checkTile(Vector3Int a, string dir)
    {
        switch (dir)
        {
            case "up":
                return grassMap.HasTile(new Vector3Int(a.x, a.y + 1, 0));
            case "down":
                return grassMap.HasTile(new Vector3Int(a.x, a.y - 1, 0));
            case "left":
                return grassMap.HasTile(new Vector3Int(a.x - 1, a.y, 0));
            case "right":
                return grassMap.HasTile(new Vector3Int(a.x + 1, a.y, 0));
        }
        return false;
    }

    void determineOutline(Vector3Int a)
    {
        int adjacents = numAdjacents(a);
        if (!grassMap.HasTile(a) || adjacents == 8) return;

        //Checks sides
        if (twoVerts(a) && !twoHoriz(a))
        {
            grassMap.SetTile(a, checkTile(a, "left") ? grassR : grassL);
            return;
        }
        if (twoHoriz(a) && !twoVerts(a))
        {
            grassMap.SetTile(a, checkTile(a, "up") ? grassB : grassT);
            return;
        }

        //Checks corners
        if (!twoHoriz(a) && !twoVerts(a))
        {
            if (checkTile(a, "down"))
            {
                if (checkTile(a, "left")) grassMap.SetTile(a, grassTRC);
                else grassMap.SetTile(a, grassTLC);
                return;
            }
            else
            {
                if (checkTile(a, "left")) grassMap.SetTile(a, grassBRC);
                else grassMap.SetTile(a, grassBLC);
                return;
            }
        }
    }

    bool twoVerts(Vector3Int a)
    {
        if (checkTile(a, "up") && checkTile(a, "down")) return true;
        else return false;
    }
    bool twoHoriz(Vector3Int a)
    {
        if (checkTile(a, "left") && checkTile(a, "right")) return true;
        else return false;
    }

    void findInnerCorners(Vector3Int a)
    {
    }
}
