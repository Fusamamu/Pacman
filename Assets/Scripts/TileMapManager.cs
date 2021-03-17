using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    public static TileMapManager sharedInstance { get; set; }

    public TileData tileData;
    public Dictionary<TileBase, TileData> dataFromTiles;

    public Grid         mainGrid;
    public Tilemap      referencedTileMap;

    public GameObject   BigCoin_prefab;
    public GameObject   Coin_prefab;

    private void Awake()
    {
        sharedInstance = this;

        dataFromTiles = new Dictionary<TileBase, TileData>();
        dataFromTiles.Add(tileData.tile, tileData);
    }

    public Vector3 GetCellWorldPos(int x, int y)
    {
        return referencedTileMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
    }

    public Vector3Int GetCell(Vector3 pos)
    {
        return referencedTileMap.WorldToCell(pos);
    }

    public bool isWall(Vector3Int _checkedCell)
    {
        TileBase checkedTile = referencedTileMap.GetTile(_checkedCell);
        return (checkedTile == dataFromTiles[tileData.tile].tile) ? true : false;
    }
}
