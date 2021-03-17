using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pacman : MonoBehaviour
{
    public Tilemap referenceTileMap;

    public TileData tileData;
    public Dictionary<TileBase, TileData> dataFromTiles;

    [Range(5f, 100f)]
    public float speed = 5f;

    public Vector3Int currentCell;

    public Vector3Int current_RIGHTCELL;
    public Vector3Int current_LEFTCELL;

    public  float moveTime = 0.05f;

    private Vector3 targetPos;

    public Vector2 direction = Vector2.zero;

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        dataFromTiles.Add(tileData.tile, tileData);

        currentCell = referenceTileMap.WorldToCell(transform.position);
        targetPos   = transform.position;
    }

    private void Update()
    {
        #region Controller

        currentCell = referenceTileMap.WorldToCell(transform.position);

        if (Input.GetKey(KeyCode.RightArrow) && direction.y == 0)
        {
            direction = Vector2.right;

            Vector3Int rightCell = new Vector3Int(currentCell.x + 1, currentCell.y, 0);

            if(!CheckWall(rightCell))
                targetPos   = referenceTileMap.GetCellCenterWorld(rightCell);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && direction.y == 0)
        {
            direction = Vector2.left;

            Vector3Int leftCell = new Vector3Int(currentCell.x - 1, currentCell.y, 0);

            if(!CheckWall(leftCell))
                targetPos = referenceTileMap.GetCellCenterWorld(leftCell);
        }

        if (Input.GetKey(KeyCode.UpArrow) && direction.x == 0)
        {
            direction = Vector2.up;

            Vector3Int upCell = new Vector3Int(currentCell.x, currentCell.y + 1, 0);

            if(!CheckWall(upCell))
                targetPos = referenceTileMap.GetCellCenterWorld(upCell);
        }

        if (Input.GetKey(KeyCode.DownArrow) && direction.x == 0)
        {
            direction = Vector2.down;

            Vector3Int downCell = new Vector3Int(currentCell.x, currentCell.y - 1, 0);

            if(!CheckWall(downCell))
                targetPos = referenceTileMap.GetCellCenterWorld(downCell);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow))
            direction = Vector2.zero;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        #endregion
    }

    private bool CheckWall(Vector3Int _cell)
    {
        TileBase checkedTile = referenceTileMap.GetTile(_cell);
        return (checkedTile == dataFromTiles[tileData.tile].tile) ? true : false;
    }
}
