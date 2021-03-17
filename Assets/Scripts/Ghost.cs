using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tilemap wallCollision_flag;

    public TileData tileData;
    public Dictionary<TileBase, TileData> dataFromTiles;

    [Range(5.0f, 10.0f)]
    public float speed = 5.0f;

    public Vector3Int currentCell;

    private enum Direction
    {
        left, right, up, down
    }

    private Direction currentDir = Direction.right;

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        dataFromTiles.Add(tileData.tile, tileData);
    }

    private void Update()
    {
        currentCell = wallCollision_flag.WorldToCell(transform.position);

        switch (currentDir)
        {
            case Direction.right:
                transform.position += Vector3.right * speed * Time.deltaTime;
                break;
            case Direction.left:
                transform.position += Vector3.left * speed * Time.deltaTime;
                break;
            case Direction.up:
                transform.position += Vector3.up * speed * Time.deltaTime;
                break;
            case Direction.down:
                transform.position += Vector3.down * speed * Time.deltaTime;
                break;
        }

        if (Facing_RIGHTWALL())
            currentDir = Direction.left;

        if (Facing_LEFTWALL())
            currentDir = Direction.right;
    }

    private bool Facing_RIGHTWALL()
    {
        Vector3Int rightCell = new Vector3Int(currentCell.x + 1, currentCell.y, 0);

        TileBase checkedTile = wallCollision_flag.GetTile(rightCell);

        return (checkedTile == dataFromTiles[tileData.tile].tile) ? true : false;
    }

    private bool Facing_LEFTWALL()
    {
        Vector3Int leftCell = new Vector3Int(currentCell.x - 1, currentCell.y, 0);

        TileBase checkedTile = wallCollision_flag.GetTile(leftCell);

        return (checkedTile == dataFromTiles[tileData.tile].tile) ? true : false;
    }

    private bool Facing_BOTTOMWALL()
    {
        Vector3Int bottomCell = new Vector3Int(currentCell.x, currentCell.y - 1, 0);

        TileBase checkedTile = wallCollision_flag.GetTile(bottomCell);

        return (checkedTile == dataFromTiles[tileData.tile].tile) ? true : false;
    }

    private bool Facing_TOPWALL()
    {
        Vector3Int topCell = new Vector3Int(currentCell.x, currentCell.y + 1, 0);

        TileBase checkedTile = wallCollision_flag.GetTile(topCell);

        return (checkedTile == dataFromTiles[tileData.tile].tile) ? true : false;
    }
}
