using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGen : MonoBehaviour
{

    public Grid tileGrid, moveGrid;
    GridManager tileGMgr, moveGMgr;
    public Tile ground,
                ////////////////////////////////////////
                grassTLC, grassT, grassTRC,
                grassL, grassM, grassR,
                grassBLC, grassB, grassBRC,

                grassIBL, grassIBR, grassITL, grassITR,

                grass2CL, grass2CR, grass2CU, grass2CD,
                grass1L, grass1R, grass1U, grass1D,
                ////////////////////////////////////////
                waterTLC, waterT, waterTRC,
                waterL, waterM, waterR,
                waterBLC, waterB, waterBRC,

                waterIBL, waterIBR, waterITL, waterITR,

                water2CL, water2CR, water2CU, water2CD,
                water1L, water1R, water1U, water1D;
                ////////////////////////////////////////

    public Tilemap groundMap, grassMap, obstacleMap, waterMap, activeMap;
    const int SIZE = 20, MAX_STEPS = 6;
    Vector3Int currentPos;  
    public static List<Vector3Int> grassSeeds, waterSeeds, activeSeeds, allOccupied;
    public static List<Tile> grassTiles, waterTiles, activeTiles;

    public static bool firstRun = true;

    // Start is called before the first frame update
    void Start()
    {
        grassSeeds = new List<Vector3Int>();
        waterSeeds = new List<Vector3Int>();
        allOccupied = new List<Vector3Int>();
        grassTiles = new List<Tile>();
        waterTiles = new List<Tile>();
        tileGMgr = new GridManager(tileGrid);
        moveGMgr = new GridManager(moveGrid);

        grassTiles.Add(grassTLC);
        grassTiles.Add(grassT);
        grassTiles.Add(grassTRC);
        grassTiles.Add(grassL);
        grassTiles.Add(grassM);
        grassTiles.Add(grassR);
        grassTiles.Add(grassBLC);
        grassTiles.Add(grassB);
        grassTiles.Add(grassBRC);
        grassTiles.Add(grassIBL);
        grassTiles.Add(grassIBR);
        grassTiles.Add(grassITL);
        grassTiles.Add(grassITR);
        grassTiles.Add(grass2CL);
        grassTiles.Add(grass2CR);
        grassTiles.Add(grass2CU);
        grassTiles.Add(grass2CD);
        grassTiles.Add(grass1L);
        grassTiles.Add(grass1R);
        grassTiles.Add(grass1U);
        grassTiles.Add(grass1D);

        waterTiles.Add(waterTLC);
        waterTiles.Add(waterT);
        waterTiles.Add(waterTRC);
        waterTiles.Add(waterL);
        waterTiles.Add(waterM);
        waterTiles.Add(waterR);
        waterTiles.Add(waterBLC);
        waterTiles.Add(waterB);
        waterTiles.Add(waterBRC);
        waterTiles.Add(waterIBL);
        waterTiles.Add(waterIBR);
        waterTiles.Add(waterITL);
        waterTiles.Add(waterITR);
        waterTiles.Add(water2CL);
        waterTiles.Add(water2CR);
        waterTiles.Add(water2CU);
        waterTiles.Add(water2CD);
        waterTiles.Add(water1L);
        waterTiles.Add(water1R);
        waterTiles.Add(water1U);
        waterTiles.Add(water1D);

        //Generate base tiles
        for (int i = -SIZE; i < SIZE; i++)
        {
            for (int j = -SIZE; j < SIZE; j++)
            {
                groundMap.SetTile(new Vector3Int(i, j, 0), ground);
            }
        }

        doPatchGen(grassMap, grassTiles, grassSeeds);
        doPatchGen(waterMap, waterTiles, waterSeeds);

    }

    void doPatchGen(Tilemap a, List<Tile> b, List<Vector3Int> c)
    {
        activeMap = a;
        Debug.Log("Map: " + activeMap.name);
        activeTiles = b;
        activeSeeds = c;

        //Generate grass seeds
        for (int i = -SIZE; i < SIZE; i++)
        {
            for (int j = -SIZE; j < SIZE; j++)
            {
                if ((spacing(new Vector2Int(i, j), 20) && Chance.Percent(1) && !activeMap.HasTile(new Vector3Int(i, j, 0))))
                {
                    activeMap.SetTile(new Vector3Int(i, j, 0), activeTiles[4]);
                    activeSeeds.Add(new Vector3Int(i, j, 0));
                    allOccupied.Add(new Vector3Int(i, j, 0));
                }
            }
        }

        //Create grass branches
        foreach (Vector3Int seedPos in activeSeeds)
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
                    activeMap.SetTile(currentPos, activeTiles[4]);
                    allOccupied.Add(currentPos);
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
        foreach (Vector3Int seedPos in activeSeeds)
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
                            activeMap.SetTile(currentPos, activeTiles[4]);
                            allOccupied.Add(currentPos);
                        }
                    }
                }
            }
        }

        //Cleanup first pass (outline)
        foreach (Vector3Int seedPos in activeSeeds)
        {
            for (int x = MAX_STEPS * -2; x < MAX_STEPS * 2; x++)
            {
                for (int y = MAX_STEPS * -2; y < MAX_STEPS * 2; y++)
                {
                    determineOutline(new Vector3Int(x + seedPos.x, y + seedPos.y, 0));
                }
            }
        }

        //Cleanup second pass (inner corners and 1x1s)
        foreach (Vector3Int seedPos in activeSeeds)
        {
            for (int x = MAX_STEPS * -2; x < MAX_STEPS * 2; x++)
            {
                for (int y = MAX_STEPS * -2; y < MAX_STEPS * 2; y++)
                {
                    innersAnd1x1s(new Vector3Int(x + seedPos.x, y + seedPos.y, 0));
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
                Debug.Log("i " + (pos.x + i) + "j " + (pos.y + j) + "pos " + tspot + "cond " + activeMap.HasTile(tspot));
                if (activeMap.HasTile(tspot) && tspot != pos) counter++;
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
                if (activeMap.HasTile(t) || allOccupied.Contains(t)) return false;
            }
        }
        return true;
    }

    bool checkTile(Vector3Int a, string dir, string diag)
    {
        int yOffset;
        if (diag == "up") yOffset = 1;
        else if (diag == "down") yOffset = -1;
        else yOffset = 0;

        switch (dir)
        {
            case "up":
                return activeMap.HasTile(new Vector3Int(a.x, a.y + 1, 0));
            case "down":
                return activeMap.HasTile(new Vector3Int(a.x, a.y - 1, 0));
            case "left":
                return activeMap.HasTile(new Vector3Int(a.x - 1, a.y + yOffset, 0));
            case "right":
                return activeMap.HasTile(new Vector3Int(a.x + 1, a.y + yOffset, 0));
        }
        return false;
    }

    bool checkTile(Vector3Int a, string dir)
    {
        switch (dir)
        {
            case "up":
                return activeMap.HasTile(new Vector3Int(a.x, a.y + 1, 0));
            case "down":
                return activeMap.HasTile(new Vector3Int(a.x, a.y - 1, 0));
            case "left":
                return activeMap.HasTile(new Vector3Int(a.x - 1, a.y, 0));
            case "right":
                return activeMap.HasTile(new Vector3Int(a.x + 1, a.y, 0));
        }
        return false;
    }

    void determineOutline(Vector3Int a)
    {
        int adjacents = numAdjacents(a);
        if (!activeMap.HasTile(a) || adjacents == 8) return;

        //Checks sides
        if (twoVerts(a) && !twoHoriz(a))
        {
            activeMap.SetTile(a, checkTile(a, "left") ? activeTiles[5] : activeTiles[3]);
            allOccupied.Add(a);
            return;
        }
        if (twoHoriz(a) && !twoVerts(a))
        {
            activeMap.SetTile(a, checkTile(a, "up") ? activeTiles[7] : activeTiles[1]);
            allOccupied.Add(a);
            return;
        }

        //Checks corners
        if (!twoHoriz(a) && !twoVerts(a))
        {
            if (checkTile(a, "down"))
            {
                if (checkTile(a, "left")) activeMap.SetTile(a, activeTiles[2]);
                else activeMap.SetTile(a, activeTiles[0]);
                allOccupied.Add(a);
                return;
            }
            else
            {
                if (checkTile(a, "left")) activeMap.SetTile(a, activeTiles[8]);
                else activeMap.SetTile(a, activeTiles[6]);
                allOccupied.Add(a);
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

    //Returns the number of tiles in cardinal directions
    int numCardinals(Vector3Int a)
    {
        int counter = 0;
        if (checkTile(a, "up")) counter++;
        if (checkTile(a, "down")) counter++;
        if (checkTile(a, "left")) counter++;
        if (checkTile(a, "right")) counter++;
        return counter;
    }

    int numDiagonals(Vector3Int a)
    {
        int counter = 0;
        if (checkTile(a, "left", "up")) counter++;
        if (checkTile(a, "left", "down")) counter++;
        if (checkTile(a, "right", "up")) counter++;
        if (checkTile(a, "right", "down")) counter++;
        return counter;
    }

    void innersAnd1x1s(Vector3Int a)
    {
        //If tile exists, is not completely surrounded, and has all four cardinal directions filled, it is an inner corner piece
        if (activeMap.HasTile(a) && numAdjacents(a) != 8 && numCardinals(a) == 4)
        {
            if (numDiagonals(a) == 2) twoCorners(a);
            else if (numDiagonals(a) == 3) oneCorner(a);
        }
        else if (activeMap.HasTile(a) && numCardinals(a) == 1)
        {
            if (!checkTile(a, "left")) activeMap.SetTile(a, activeTiles[17]);
            else if (!checkTile(a, "right")) activeMap.SetTile(a, activeTiles[18]);
            else if (!checkTile(a, "up")) activeMap.SetTile(a, activeTiles[19]);
            else if (!checkTile(a, "down")) activeMap.SetTile(a, activeTiles[20]);
            allOccupied.Add(a);
        }
    }

    void twoCorners(Vector3Int a)
    {
        if (!checkTile(a, "left", "down") && !checkTile(a, "left", "up")) activeMap.SetTile(a, activeTiles[13]);
        else if (!checkTile(a, "left", "down") && !checkTile(a, "right", "down")) activeMap.SetTile(a, activeTiles[16]);
        else if (!checkTile(a, "left", "up") && !checkTile(a, "right", "up")) activeMap.SetTile(a, activeTiles[15]);
        else if (!checkTile(a, "right", "down") && !checkTile(a, "right", "up")) activeMap.SetTile(a, activeTiles[14]);
        allOccupied.Add(a);
    }

    void oneCorner(Vector3Int a)
    {
        if (!checkTile(a, "left", "down")) activeMap.SetTile(a, activeTiles[9]);
        else if (!checkTile(a, "left", "up")) activeMap.SetTile(a, activeTiles[11]);
        else if (!checkTile(a, "right", "up")) activeMap.SetTile(a, activeTiles[12]);
        else if (!checkTile(a, "right", "down")) activeMap.SetTile(a, activeTiles[10]);
        allOccupied.Add(a);
    }
}