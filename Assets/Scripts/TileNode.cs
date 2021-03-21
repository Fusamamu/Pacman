using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode
{
    public TileNode Parent;

    public int G_cost;
    public int H_cost;
    public int F_cost { get { return G_cost + H_cost; } }

    public bool isWall;

    public Vector3Int   Cell;
    public Vector3      CellWorldPos;

    public TileNode(Vector3Int _cell, int _g = 0, int _h = 0)
    {
        Cell    = _cell;
        G_cost  = _g;
        H_cost  = _h;
    }
}
