using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.Profiling;
using UnityEngine;

public class Creature : MonoBehaviour
{

    protected Path path;
    protected Seeker seeker;
    protected Rigidbody2D rigidBody;

    protected float nextWaypointDistance = 3f;

    [SerializeField] protected Transform target;

    protected int currentWaypoint = 0;
    protected bool reachedEnd = false;

    protected bool isAlive, isPregnant, canReproduce;

    protected int currentHealth, maxHealth;
    [SerializeField] protected int movementSpeed, age;

    //Motivators for decision making
    protected int reproductionMotivator, foodMotivator,
                  hostileMotivator, waterMotivator,
                  sleepMotivator;

    //Determines the rate at which different motivators should change
    [SerializeField]
    protected float reproductionWeight, foodWeight,
                     hostileWeight, waterWeight,
                     sleepWeight;

    protected void initialize()
    {
        reproductionMotivator = 0;
        foodMotivator = 0;
        hostileMotivator = 0;
        waterMotivator = 0;
        sleepMotivator = 0;

        rigidBody = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("findPath", 0f, .5f);   
    }

    public void findPath()
    {
        if (seeker.IsDone()) seeker.StartPath(rigidBody.position, findTarget(), OnPathComplete);
    }

    protected Vector2 findTarget()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    protected void followPath(Path p)
    {
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

        if (distance < nextWaypointDistance)
        {
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
}