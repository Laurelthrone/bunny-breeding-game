using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckContact : MonoBehaviour
{

    Creature bunny;
    CircleCollider2D detector;
    bool state;

    // Start is called before the first frame update
    void Start()
    {
        bunny = GetComponentInParent<Bunny>();
        Debug.Log(bunny.lastWater);
        detector = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        detector.radius = state ? 1.05f : 20;
        state = !state;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //I genuinely don't remember why I was doing this but I'm scared if I change it it'll break
        if (bunny == null)
        {
            bunny = gameObject.GetComponentInParent<Bunny>();
            return;
        }

        if (state) //If radius is set to large, record the points of collisions
        {
            switch (collision.gameObject.tag) 
            {
                case "Bunny":
                    bunny.lastBun = collision.gameObject.transform.position;
                    break;
                case "Water":
                    bunny.lastWater = collision.gameObject.transform.position;
                    break;
                case "Food":
                    bunny.lastFood = collision.gameObject.transform.position;
                    break;
                case "Predator":
                    bunny.lastPredator = collision.gameObject;
                    break;
            }
        }
        else switch (collision.gameObject.tag)  //Otherwise, update state according to collision
            {
            case "Bunny":
                break;
            case "Water":
                bunny.drinking = true;
                break;
            case "Food":
                bunny.eating = true;
                break;
            case "Predator":
                break;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!state) return;

        switch (collision.gameObject.tag)
        {
            case "Bunny":
                break;
            case "Water":
                bunny.drinking = false;
                break;
            case "Food":
                bunny.eating = false;
                break;
            case "Predator":
                break;
        }
    }
}
