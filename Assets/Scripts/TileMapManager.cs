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

    public int Get_H_Cost(Vector3Int _nodeA, Vector3Int _nodeB)
    {
        int x_diff = Mathf.Abs(_nodeA.x - _nodeB.x);
        int y_diff = Mathf.Abs(_nodeA.y - _nodeB.y);

        return x_diff + y_diff - 2;
    }

    public List<TileNode> GetNeighborNodes(Vector3Int _node)
    {
        List<TileNode> _neighbors = new List<TileNode>();

        TileNode rightNode      = new TileNode(new Vector3Int(_node.x + 1, _node.y, _node.z));
        TileNode leftNode       = new TileNode(new Vector3Int(_node.x - 1, _node.y, _node.z));
        TileNode topNode        = new TileNode(new Vector3Int(_node.x, _node.y + 1, _node.z));
        TileNode bottomNode     = new TileNode(new Vector3Int(_node.x, _node.y - 1, _node.z));

        rightNode.isWall        = isWall(rightNode.Cell);
        leftNode.isWall         = isWall(leftNode.Cell);
        topNode.isWall          = isWall(topNode.Cell);
        bottomNode.isWall       = isWall(bottomNode.Cell);

        _neighbors.Add(rightNode);
        _neighbors.Add(leftNode);
        _neighbors.Add(topNode);
        _neighbors.Add(bottomNode);

        return _neighbors;
    }
}
