using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AI : MonoBehaviour
{
    public static AI sharedInstance { get; set; }

    public Tilemap  referenceTileMap;
    public TileData tileData;
    public Dictionary<TileBase, TileData> dataFromTiles;


    public Ghost ghost;


    public Vector3Int targetCell;

    private void Awake()
    {
        sharedInstance = this;
    }
}
