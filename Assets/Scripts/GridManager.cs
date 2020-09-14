using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    Grid grid;
    public GridDirection direction, none;
    public Vector3Int gridPos, worldPos;

    public GridManager(Grid a, Vector3Int b)
    {
        none = new GridDirection("", "");
        grid = a;
        gridPos = getGridCoords(b);
    }

    public GridManager(Grid a)
    {
        none = new GridDirection("", "");
        grid = a;
        gridPos = new Vector3Int(0,0,0);
    }

    public Vector3Int getWorldPos()
    {
        Vector3 returnValue = grid.CellToWorld(gridPos);
        return new Vector3Int((int)returnValue.x, (int)returnValue.y, (int)returnValue.z);
    }

    public Vector3Int getGridCoords(Vector3Int input)
    {
        return grid.WorldToCell(input);
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
