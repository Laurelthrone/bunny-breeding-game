using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public Rigidbody2D player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        float xmov = Input.GetAxis("Horizontal") * speed * 100 * Time.deltaTime;
        float ymov = Input.GetAxis("Vertical") * speed * 100 * Time.deltaTime;

        //Prepare to check movement
        Vector2 movement;

        movement = new Vector2(xmov, ymov);

        //Apply movement
        player.AddForce(movement);
    }
}
