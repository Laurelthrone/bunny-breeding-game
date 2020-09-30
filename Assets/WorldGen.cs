using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGen : MonoBehaviour
{
    public Grid tileGrid, moveGrid;
    GridManager tileGMgr, moveGMgr;
    public Tile ground;
    public Tilemap groundMap, grassMap, waterMap, activeMap;
    public RuleTile grassTile, waterTile, activeTile;
    public int SIZE = 2, MAX_STEPS = 10;
    public float ODDS = .02f;
    Vector3Int currentPos;
    public static List<Vector3Int> grassSeeds, waterSeeds, activeSeeds, allOccupied, obstacles;
    bool initialized = false;
    bool running = false;
    int currentStep = 0;

    MapSet grass, water;

    // Start is called before the first frame update
    void Start()
    {
        grassSeeds = new List<Vector3Int>();
        waterSeeds = new List<Vector3Int>();
        allOccupied = new List<Vector3Int>();
        tileGMgr = new GridManager(tileGrid);
        moveGMgr = new GridManager(moveGrid);
        obstacles = new List<Vector3Int>();
        grass = new MapSet(grassMap, grassTile, grassSeeds, "Grass");
        water = new MapSet(waterMap, waterTile, waterSeeds, "Water");

        //Generate base tiles
        for (int i = -SIZE; i < SIZE; i++)
        {
            for (int j = -SIZE; j < SIZE; j++)
            {
                groundMap.SetTile(new Vector3Int(i, j, 0), ground);
            }
        }

        waterMap.SetTile(new Vector3Int(10, 10, 0), waterTile);
        waterSeeds.Add(new Vector3Int(10, 10, 0));
        allOccupied.Add(new Vector3Int(10, 10, 0));
        obstacles.Add(new Vector3Int(10, 10, 0));

        StartCoroutine("checker");
    }

    IEnumerator checker()
    {
        running = true;
        StartCoroutine("doPatchGen", grass);
        while (running) yield return new WaitForFixedUpdate();
        running = true;
        StartCoroutine("doPatchGen", water);
        while (running) yield return new WaitForFixedUpdate();
    }

    IEnumerator doPatchGen(MapSet set)
    {
        activeTile = set.tile;
        activeMap = set.map;
        activeSeeds = set.seeds;

        //Generate grass seeds
        for (int i = -SIZE; i < SIZE; i++)
        {
            for (int j = -SIZE; j < SIZE; j += Random.Range(1, 5))
            {
                if (Chance.Percent(ODDS) && spacing(new Vector3Int(i, j, 0), 10) && !activeMap.HasTile(new Vector3Int(i, j, 0)))
                {
                    activeMap.SetTile(new Vector3Int(i, j, 0), activeTile);
                    activeSeeds.Add(new Vector3Int(i, j, 0));
                    allOccupied.Add(new Vector3Int(i, j, 0));
                    if (activeMap.name == "Water") obstacles.Add(new Vector3Int(i, j, 0));
                }
            }
            
            yield return new WaitForEndOfFrame();
        }

        //Generate random patches
        for (int i = -SIZE; i < SIZE; i++)
        {
            for (int j = -SIZE; j < SIZE; j += Random.Range(1, 5))
            {
                if (Chance.Percent(ODDS*5) && spacing(new Vector3Int(i, j, 0), 10) && !activeMap.HasTile(new Vector3Int(i, j, 0)))
                {
                    activeMap.SetTile(new Vector3Int(i, j, 0), activeTile);
                    allOccupied.Add(new Vector3Int(i, j, 0));
                    if (activeMap.name == "Water") obstacles.Add(new Vector3Int(i, j, 0));
                }
            }

            yield return new WaitForEndOfFrame();
        }

        //Create grass branches
        foreach (Vector3Int seedPos in activeSeeds)
        {
            int numSteps;
            int xDir = 0;
            int yDir = 0;
            bool lastWasDiag = false;
            Vector3Int movMod;

            for (int i = 0; i <= 3; i++)
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
                    activeMap.SetTile(currentPos, activeTile);
                    allOccupied.Add(currentPos);
                    if (activeMap.name == "Water") obstacles.Add(currentPos);
                    currentPos += movMod;
                    if (!lastWasDiag && Chance.Percent(90))
                    {
                        if (xDir != 0) currentPos.y += Random.Range(-1, 1);
                        else currentPos.x += Random.Range(-1, 1);
                        lastWasDiag = true;
                    }
                    else lastWasDiag = false;
                }
                yield return new WaitForEndOfFrame();

            }

        }

        Debug.Log("Kakababatutu");


        foreach (Vector3Int seedPos in activeSeeds)
        {
            bool invertVert = Chance.Percent(50);
            bool invertHoriz = Chance.Percent(50);

            for (int i = 0; i < 1; i++)
            {
                for (int x = MAX_STEPS * -2; x < MAX_STEPS * 2; x++)
                {
                    for (int y = MAX_STEPS * -2; y < MAX_STEPS * 2; y++)
                    {
                        currentPos = new Vector3Int(seedPos.x + (x * (invertHoriz ? -1 : 1)), seedPos.y - (y * (invertVert ? -1 : 1)), 0);

                        //Fill empty tiles that have at least 3 adjacent tiles
                        if (numAdjacents(currentPos) >= 3 && Chance.Percent(90))
                        {
                            activeMap.SetTile(currentPos, activeTile);
                            allOccupied.Add(currentPos);
                            if (activeMap.name == "Water") obstacles.Add(currentPos);
                        }
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }

        foreach (Vector3Int seedPos in activeSeeds)
        {
            bool invertVert = Chance.Percent(50);
            bool invertHoriz = Chance.Percent(50);

            for (int i = 0; i < 1; i++)
            {
                for (int x = MAX_STEPS * -2; x < MAX_STEPS * 2; x++)
                {
                    for (int y = MAX_STEPS * -2; y < MAX_STEPS * 2; y++)
                    {
                        currentPos = new Vector3Int(seedPos.x + (x * (invertHoriz ? -1 : 1)), seedPos.y - (y * (invertVert ? -1 : 1)), 0);

                        //Fill empty tiles that have at least 3 adjacent tiles
                        if (numAdjacents(currentPos) == 8)
                        {
                            activeMap.SetTile(currentPos, activeTile);
                            allOccupied.Add(currentPos);
                            if (activeMap.name == "Water") obstacles.Add(currentPos);
                        }
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Kakababatutu2");

        running = false;
    }

    int numAdjacents(Vector3Int pos)
    {
        int counter = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3Int tspot = new Vector3Int(pos.x + i, pos.y + j, 0);
               // Debug.Log("i " + (pos.x + i) + "j " + (pos.y + j) + "pos " + tspot + "cond " + activeMap.HasTile(tspot));
               if(activeMap.HasTile(tspot))
                {
                    counter++;
                }
            }
        }
        return counter;
    }

    bool spacing(Vector3Int pos, int distance)
    {
        foreach (Vector3Int position in allOccupied)
        {
            if (Vector3Int.Distance(pos, position) < distance) return false;
        }
        return true;
    }

}

