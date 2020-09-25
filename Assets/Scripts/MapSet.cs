using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MapSet
{
    public Tilemap map;
    public List<Tile> tiles;
    public List<Vector3Int> seeds;

    public MapSet(Tilemap a, List<Tile> b, List<Vector3Int> c)
    {
        map = a;
        tiles = b;
        seeds = c;
    }
}
