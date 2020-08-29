using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLogger : MonoBehaviour
{

    Bunny bunny;

    // Start is called before the first frame update
    void Start()
    {
        bunny = gameObject.GetComponentInParent<Bunny>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bunny == null) return;
        switch(collision.gameObject.tag)
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
}
