using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector3 gridPos, pos;
    public float speed;
    public Rigidbody2D player;
    float cdTime;   
    public float cd, movementSpeed;
    public Grid grid, grid2;
    GridManager gridManager, gridManager2;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = new GridManager(grid);
        gridManager2 = new GridManager(grid2);
        player = GetComponent<Rigidbody2D>();
        gridPos = new Vector3Int(0, 0, 0);
        cdTime = 0;
    }
        
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey) doInputs();
        else gridManager.direction = gridManager.none;
        gridPos = gridManager.getWorldPos();
        gridPos.x += .5f;
        gridPos.y += .5f;
        pos = Vector3.Lerp(transform.position, gridPos, movementSpeed * Time.deltaTime);
        transform.position = pos;
    }

    void doInputs()
    {
        if (Time.time <= cdTime) return;

        string vertDir = "";
        string horizDir = "";

        if (Input.GetAxisRaw("Vertical") > 0) vertDir = "up";
        if (Input.GetAxisRaw("Vertical") < 0) vertDir = "down";
        if (Input.GetAxisRaw("Horizontal") > 0) horizDir = "right";
        if (Input.GetAxisRaw("Horizontal") < 0) horizDir = "left";

        cdTime = Time.time + ((vertDir != "" && horizDir != "") ? cd*1.1f : cd);

        gridManager.direction = new GridDirection(horizDir, vertDir);
        gridManager.move();
    }
    
}
