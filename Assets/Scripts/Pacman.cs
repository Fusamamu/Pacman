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

    public  float moveTime = 0.1f;
    private float inverseMoveTime;

    private Rigidbody2D rb2D;

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        dataFromTiles.Add(tileData.tile, tileData);

        inverseMoveTime = 1f / moveTime;

        rb2D = GetComponent<Rigidbody2D>();

        currentCell = referenceTileMap.WorldToCell(transform.position);
    }

    private void Update()
    {
        //Vector3 nextRightCellPos = wallCollision_flag.CellToWorld(new Vector3Int(currentCell.x + 1, currentCell.y, 0));

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.GetComponent<SpriteRenderer>().flipX = false;
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            currentCell = referenceTileMap.WorldToCell(transform.position);
            Vector3Int rightCell = new Vector3Int(currentCell.x + 1, currentCell.y, 0);
            Move(rightCell);
        }

        #region old_movement
        /*
        if (Input.GetKey(KeyCode.RightArrow))
        {
            float diff = Mathf.Abs(transform.position.y - wallCollision_flag.GetCellCenterWorld(currentCell).y);

            if (!Facing_RIGHTWALL() && diff < 0.15f)
            {
                transform.GetComponent<SpriteRenderer>().flipX = false;
                transform.position += Vector3.right * speed * Time.deltaTime;

            }
            else
            {
                Vector3 cellCenterWorldPos = wallCollision_flag.GetCellCenterWorld(currentCell);

                if(Vector3.Distance(transform.position, cellCenterWorldPos ) > 0f && transform.position.x < cellCenterWorldPos.x)
                {
                    transform.position += Vector3.right * speed * Time.deltaTime;
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            float diff = Mathf.Abs(transform.position.y - wallCollision_flag.GetCellCenterWorld(currentCell).y);

            if (!Facing_LEFTWALL() && diff < 0.15f)
            {
                transform.GetComponent<SpriteRenderer>().flipX = true;
                transform.position += Vector3.left * speed * Time.deltaTime;

            }
            else
            {
                Vector3 cellCenterWorldPos = wallCollision_flag.GetCellCenterWorld(currentCell);

                if (Vector3.Distance(transform.position, cellCenterWorldPos) > 0f && transform.position.x > cellCenterWorldPos.x)
                {
                    transform.position += Vector3.left * speed * Time.deltaTime;
                }
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            float diff = Mathf.Abs(transform.position.x - wallCollision_flag.GetCellCenterWorld(currentCell).x);

            if (!Facing_TOPWALL() && diff < 0.15f)
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
            }
            else
            {
                Vector3 cellCenterWorldPos = wallCollision_flag.GetCellCenterWorld(currentCell);

                if (Vector3.Distance(transform.position, cellCenterWorldPos) > 0f && transform.position.y < cellCenterWorldPos.y)
                {
                    transform.position += Vector3.up * speed * Time.deltaTime;
                }
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            float diff = Mathf.Abs(transform.position.x - wallCollision_flag.GetCellCenterWorld(currentCell).x);

            if (!Facing_BOTTOMWALL() && diff < 0.15f)
            {
                transform.position += Vector3.down * speed * Time.deltaTime;
            }
            else
            {
                Vector3 cellCenterWorldPos = wallCollision_flag.GetCellCenterWorld(currentCell);

                if (Vector3.Distance(transform.position, cellCenterWorldPos) > 0f && transform.position.y > cellCenterWorldPos.y)
                {
                    transform.position += Vector3.down * speed * Time.deltaTime;
                }
            }
        }
        */
        #endregion 
    }

    private void Move(Vector3Int targetCell)
    {
        Vector3 targetPos = referenceTileMap.GetCellCenterWorld(targetCell);
        currentCell = targetCell;
        StartCoroutine(SmoothMovement(targetPos));
    }

    IEnumerator SmoothMovement(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 1f)
        {
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, targetPos, inverseMoveTime * Time.deltaTime);

            rb2D.MovePosition(newPostion);

            yield return null;
        }

        Debug.Log("Quitting");
    }

    private bool Facing_RIGHTWALL()
    {
        Vector3Int rightCell = new Vector3Int(currentCell.x + 1, currentCell.y, 0);

        TileBase checkedTile = referenceTileMap.GetTile(rightCell);
       
        return (checkedTile == dataFromTiles[tileData.tile].tile) ? true : false;
    }

    private bool Facing_LEFTWALL()
    {
        Vector3Int leftCell = new Vector3Int(currentCell.x - 1, currentCell.y, 0);

        current_LEFTCELL = leftCell;

        TileBase checkedTile = referenceTileMap.GetTile(leftCell);

        return (checkedTile == dataFromTiles[tileData.tile].tile) ? true : false;
    }

    private bool Facing_BOTTOMWALL()
    {
        Vector3Int bottomCell = new Vector3Int(currentCell.x, currentCell.y - 1, 0);

        TileBase checkedTile = referenceTileMap.GetTile(bottomCell);

        return (checkedTile == dataFromTiles[tileData.tile].tile) ? true : false;
    }

    private bool Facing_TOPWALL()
    {
        Vector3Int topCell = new Vector3Int(currentCell.x, currentCell.y + 1, 0);

        TileBase checkedTile = referenceTileMap.GetTile(topCell);

        return (checkedTile == dataFromTiles[tileData.tile].tile) ? true : false;
    }
}
