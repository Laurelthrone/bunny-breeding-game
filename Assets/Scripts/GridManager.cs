using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    Grid grid;
    public GridDirection direction, none;
    Vector3Int gridPos;

    public GridManager(Grid a)
    {
        none = new GridDirection("", "");
        grid = a;
        gridPos = new Vector3Int(0, 0, 0);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public Vector3 getWorldPos()
    {
        return grid.CellToWorld(gridPos);
    }

    public void moveDiagonal(string dir)
    {
        gridPos.y += (dir == "upleft" || dir == "upright") ? 1 : -1;
        gridPos.x += (dir == "upright" || dir == "downright") ? 1 : -1;
    }

    public void moveVert(string dir)
    {
        gridPos.y += dir == "up" ? 1 : -1;
    }

    public void moveHoriz(string dir)
    {
        gridPos.x += dir == "right" ? 1 : -1;
    }

    public void move()
    {
        if (direction.x != "" && direction.y == "") moveHoriz(direction.x);
        if (direction.y != "" && direction.x == "") moveVert(direction.y);
        if (direction.x != "" && direction.y != "") moveDiagonal(direction.y + direction.x);
    }
}
