using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MapSet
{
    public Tilemap map;
    public RuleTile tile;
    public List<Vector3Int> seeds;
    public string name;

    public MapSet(Tilemap a, RuleTile b, List<Vector3Int> c, string d)
    {
        map = a;
        tile = b;
        seeds = c;
        name = d;
    }
}
