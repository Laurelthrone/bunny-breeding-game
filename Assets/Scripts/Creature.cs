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

    public Vector2 lastFood, lastWater, lastPrey, lastBun;
    public GameObject lastPredator;

    protected bool eating, drinking, hunting, sleeping, fleeing;

    protected float nextWaypointDistance = 3f;

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

    protected void initialize()
    {
        isAlive = true;
        motivators.Add("reproduction", 0);
        motivators.Add("food", 0);
        motivators.Add("hostile", 0);
        motivators.Add("water", 0);
        motivators.Add("sleep", 0);

        rigidBody = GetComponent<Rigidbody2D>();
        parentCollider = GetComponent<CircleCollider2D>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("findPath", 0f, 1f);
    }

    public void findPath()
    {
        if (seeker.IsDone() && isAlive) seeker.StartPath(rigidBody.position, findTarget(), OnPathComplete);
    }

    protected Vector2 findTarget()
    {
        return targetPos;
    }

    protected void followPath(Path p)
    {
        if (fleeing)
        {
            flee();
            return;
        }
        if (sleeping) return;

        if (currentWaypoint >= p.vectorPath.Count)
        {
            reachedEnd = true;
            return;
        }
        else reachedEnd = false;

        Vector2 direction = ((Vector2)p.vectorPath[currentWaypoint] - rigidBody.position).normalized;
        Vector2 force = direction * movementSpeed * 2 * Time.deltaTime;
        float distance = Vector2.Distance(rigidBody.position, path.vectorPath[currentWaypoint]);

        rigidBody.AddForce(force);

        if (distance < nextWaypointDistance || (Time.time - lastTime) > 1f)
        {
            lastTime = Time.time;
            currentWaypoint++;
        }
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

    protected void determineGoal()
    {
        Debug.Log(drinking);
        Debug.Log(eating);

        string topMotivator = motivators.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
        Vector2 defaultPos = new Vector2(0, 0);
        Debug.Log(topMotivator + " " + motivators[topMotivator]);
        if (motivators[topMotivator] < 40)
        {
            wander();
            return;
        }

        if (topMotivator != "sleep" && sleeping) sleeping = false;

        Debug.Log(sleeping);

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
                    wander();
                    return;
                }
                targetPos = lastPrey;
                break;
            case "sleep":
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

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        switch (collision.gameObject.tag)
        {
            case "Bunny":
                break;
            case "Water":
                drinking = true;
                break;
            case "Food":
                eating = true;
                break;
            case "Predator":
                break;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        switch (collision.gameObject.tag)
        {
            case "Bunny":
                break;
            case "Water":
                drinking = false;
                break;
            case "Food":
                eating = false;
                break;
            case "Predator":
                break;
        }
    }
}