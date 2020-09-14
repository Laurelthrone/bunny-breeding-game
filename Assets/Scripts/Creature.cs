using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Unity.Profiling;
using UnityEngine;

public class Creature : MonoBehaviour
{
    protected float lastTime;
    protected Path path;
    protected Seeker seeker;
    protected Rigidbody2D rigidBody;
    protected CircleCollider2D parentCollider;
    protected CheckContact contactChecker;
    [SerializeField] protected string topMotivator;

    public Vector2 lastFood, lastWater, lastPrey, lastBun;
    public GameObject lastPredator;

    public bool eating, drinking, hunting, sleeping, fleeing;

    protected float cd, cdTime;

    [SerializeField] protected Transform target;

    protected int currentWaypoint = 0;
    protected bool reachedEnd = false;

    protected Vector2 targetPos;

    public bool isAlive, isPregnant, canReproduce, isHostile;

    protected int currentHealth, maxHealth;
    [SerializeField] protected int movementSpeed, age;

    //Motivators for decision making
    protected Dictionary<string, float> motivators = new Dictionary<string, float>();

    //Determines the rate at which different motivators should change
    [SerializeField]
    protected float reproductionWeight, foodWeight,
                    hostileWeight, waterWeight,
                    sleepWeight;

    protected Vector3 gridPos, pos;
    public Grid grid;
    protected GridManager gridManager;

    protected void initialize()
    {
        if (grid == null) grid = GameObject.FindGameObjectWithTag("TerrainGrid").GetComponent<Grid>();
        Vector3Int currentPosition = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        gridManager = new GridManager(grid, currentPosition);
        isAlive = true;
        motivators.Add("reproduction", 0);
        motivators.Add("food", 0);
        motivators.Add("hostile", 0);
        motivators.Add("water", 0);
        motivators.Add("sleep", 0);

        rigidBody = GetComponent<Rigidbody2D>();
        parentCollider = GetComponent<CircleCollider2D>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("findPath", 0f, 8f);
    }

    public void findPath()
    {
        if (seeker.IsDone() && isAlive) seeker.StartPath(rigidBody.position, targetPos, OnPathComplete);
    }

    protected void followPath(Path p)
    {
        Vector3Int targetGridPos;
        if (fleeing)
        {
            flee();
            return;
        }

        do
        {
            if (sleeping) return;

            //Check if route complete
            if (currentWaypoint >= p.vectorPath.Count)
            {
                reachedEnd = true;
                return;
            }
            else reachedEnd = false;

            //
            Vector2 targetPos = p.vectorPath[currentWaypoint];
            targetGridPos = gridManager.getGridCoords(new Vector3Int((int)targetPos.x, (int)targetPos.y,0));

            string vertDir = "";
            string horizDir = "";

            if (targetGridPos.y > gridManager.gridPos.y) vertDir = "up";
            else if (targetGridPos.y < gridManager.gridPos.y) vertDir = "down";

            if (targetGridPos.x > gridManager.gridPos.x) horizDir = "right";
            else if (targetGridPos.x < gridManager.gridPos.x) horizDir = "left";

            gridManager.direction = new GridDirection(horizDir, vertDir);
            gridManager.move();

        } while (gridManager.gridPos != targetGridPos);
        lastTime = Time.time;
        currentWaypoint++;
    }

    protected void moveTowardsCell()
    {
        if (gridPos == null) return;
        pos = Vector3.Lerp(transform.position, gridPos, movementSpeed * Time.deltaTime);
        transform.position = pos;
    }

    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected void updateMotivation()
    {
        motivators["food"] += (eating || motivators["food"] < 0 ? -1 : 1) * foodWeight;
        motivators["water"] += (drinking || motivators["water"] < 0 ? -1 : 1) * waterWeight;
        motivators["hostile"] += (isHostile || motivators["hostile"] < 0 ? 1 : 0) * hostileWeight;
        motivators["sleep"] += (sleeping || motivators["sleep"] < 0 ? -1 : 1) * sleepWeight;
        motivators["reproduction"] += (canReproduce || motivators["reproduction"] < 0 ? 1 : 0) * reproductionWeight;

        determineGoal();
    }

    //Method for determining where to go
    protected void determineGoal()
    {
        //Get the key of the entry with the highest value. ngl idk how this works I ripped it from stackoverflow
        topMotivator = motivators.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
        Debug.Log("Goal Determining....");

        Vector2 defaultPos = new Vector2(0, 0);

        //Debug.Log(topMotivator + " " + motivators[topMotivator]);

        //Wake up if something is more pressing than sleep
        if (sleeping)
        {
            if (topMotivator != "sleep")
            {
                sleeping = false;
            }
            else return;
        }
        

        //Wander until a need meets a certain threshold
        if (motivators[topMotivator] < 2000)
        {
            Debug.Log("Error in threshold check");
            wander();
            return;
        }

        //Wander if no target has been detected, otherwise pathfind to target
        switch (topMotivator)
        {
            case "food":
                if (lastFood == defaultPos)
                {
                    wander();
                    return;
                }
                targetPos = lastFood;
                break;
            case "water":
                if (lastWater == defaultPos)
                {
                    wander();
                    return;
                }
                targetPos = lastWater;
                break;
            case "reproduction":
                if (lastBun == defaultPos)
                {
                    wander();
                    return;
                }
                targetPos = lastBun;
                break;
            case "hostile":
                if (lastPrey == null)
                {
                    Debug.Log("Error_h");
                    wander();
                    return;
                }
                targetPos = lastPrey;
                break;
            case "sleep":
                Debug.Log("Sleep");
                targetPos = gameObject.transform.position;
                sleeping = true;
                break;
            default:
                wander();
                break;
        }
        return;
    }

    protected void flee()
    { }

    protected void wander()
    {
        targetPos = new Vector2(Random.Range(gameObject.transform.position.x - 30, gameObject.transform.position.x + 30), Random.Range(gameObject.transform.position.x - 30, gameObject.transform.position.x + 30));
        return;
    }

    protected void goToTargetTile()
    {
        gridPos = gridManager.getWorldPos();
        gridPos.x += .5f;
        gridPos.y += .5f;
        pos = Vector3.Lerp(transform.position, gridPos, movementSpeed * Time.deltaTime);
        transform.position = pos;
    }
    
}