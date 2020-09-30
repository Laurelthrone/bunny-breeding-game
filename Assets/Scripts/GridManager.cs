using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    Grid grid, tileGrid;
    public GridDirection direction, none;
    public Vector3Int gridPos, worldPos;

    public GridManager(Grid a, Grid b, Vector3Int c)
    {
        none = new GridDirection("", "");
        grid = a;
        tileGrid = b;
        gridPos = getGridCoords(c);
    }

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

    public Vector3Int getTileCoords(Vector3Int input)
    {
        return tileGrid.WorldToCell(input);
    }

    public Vector3Int gridCoordsToTileCoords(Vector3Int input)
    {
        return tileGrid.WorldToCell(grid.CellToWorld(input));
    }


    public void moveDiagonal(string dir)
    {
        int oldx = gridPos.x, oldy = gridPos.y;
        gridPos.y += (dir == "upleft" || dir == "upright") ? 1 : -1;
        if (WorldGen.obstacles != null && tileGrid != null && WorldGen.obstacles.Contains(gridCoordsToTileCoords(new Vector3Int(gridPos.x, gridPos.y, 0)))) gridPos.y = oldy;
        gridPos.x += (dir == "upright" || dir == "downright") ? 1 : -1;
        if (WorldGen.obstacles != null && tileGrid != null && WorldGen.obstacles.Contains(gridCoordsToTileCoords(new Vector3Int(gridPos.x, gridPos.y, 0)))) gridPos.x = oldx;
    }

    public void moveVert(string dir)
    {
        int old = gridPos.y;
        gridPos.y += dir == "up" ? 1 : -1;
        if (WorldGen.obstacles != null && tileGrid != null && WorldGen.obstacles.Contains(gridCoordsToTileCoords(new Vector3Int(gridPos.x, gridPos.y, 0)))) gridPos.y = old;
    }

    public void moveHoriz(string dir)
    {
        int old = gridPos.x;
        gridPos.x += dir == "right" ? 1 : -1;
        if (WorldGen.obstacles != null && tileGrid != null && WorldGen.obstacles.Contains(gridCoordsToTileCoords(new Vector3Int(gridPos.x, gridPos.y, 0)))) gridPos.x = old;
    }

    public void move()
    {
        //bool alternator = false;
        //int breaker = 0;
        //Vector3Int originalPosition = new Vector3Int(gridPos.x, gridPos.y, gridPos.z);
        if (direction.x != "" && direction.y == "") moveHoriz(direction.x);
        else if (direction.y != "" && direction.x == "") moveVert(direction.y);
        else if (direction.x != "" && direction.y != "") moveDiagonal(direction.y + direction.x);
    }
}
