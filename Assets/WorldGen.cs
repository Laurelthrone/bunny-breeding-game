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

                grassIBL, grassIBR, grassITL, grassITR,

                grass2CL, grass2CR, grass2CU, grass2CD,
                grass1L, grass1R, grass1U, grass1D;

    public Tilemap groundMap, grassMap, obstacleMap;
    const int SIZE = 20, MAX_STEPS = 6;
    Vector3Int currentPos;  
    public static List<Vector3Int> grassSeeds;
    public static List<Vector3Int> grassBranches;
    public static bool firstRun = true;

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
        else if (diag == "down") yOffset = -1;
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
        if(grassMap.HasTile(a) && numAdjacents(a) != 8 && numCardinals(a) == 4)
        {
            if (numDiagonals(a) == 2) twoCorners(a);
            else if (numDiagonals(a) == 3) oneCorner(a);
        }
        else if(grassMap.HasTile(a) && numCardinals(a) == 1)
        {
            if (!checkTile(a, "left")) grassMap.SetTile(a, grass1L);
            else if (!checkTile(a, "right")) grassMap.SetTile(a, grass1R);
            else if (!checkTile(a, "up")) grassMap.SetTile(a, grass1U);
            else if (!checkTile(a, "down")) grassMap.SetTile(a, grass1D);
        }
    }

    void twoCorners(Vector3Int a)
    {
        if (!checkTile(a, "left", "down") && !checkTile(a, "left", "up")) grassMap.SetTile(a, grass2CL);
        else if (!checkTile(a, "left", "down") && !checkTile(a, "right", "down")) grassMap.SetTile(a, grass2CD);
        else if (!checkTile(a, "left", "up") && !checkTile(a, "right", "up")) grassMap.SetTile(a, grass2CU);
        else if (!checkTile(a, "right", "down") && !checkTile(a, "right", "up")) grassMap.SetTile(a, grass2CR);
    }

    void oneCorner(Vector3Int a)
    {
        if (!checkTile(a, "left", "down")) grassMap.SetTile(a, grassIBL);
        else if (!checkTile(a, "left", "up")) grassMap.SetTile(a, grassITL);
        else if (!checkTile(a, "right", "up")) grassMap.SetTile(a, grassITR);
        else if (!checkTile(a, "right", "down")) grassMap.SetTile(a, grassIBR);
    }
}
