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

   // private float inverseMoveTime;

    //private Rigidbody2D rb2D;

    private Vector3 targetPos;

    public Vector2 direction = Vector2.zero;

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        dataFromTiles.Add(tileData.tile, tileData);

        //inverseMoveTime = 1f / moveTime;

        //rb2D = GetComponent<Rigidbody2D>();

        currentCell = referenceTileMap.WorldToCell(transform.position);
        targetPos   = transform.position;
    }

    private void Update()
    {
        //Vector3 nextRightCellPos = wallCollision_flag.CellToWorld(new Vector3Int(currentCell.x + 1, currentCell.y, 0));

        #region Movement_#2
        /*
        if (Input.GetKey(KeyCode.RightArrow))
        {
            StopAllCoroutines();
            transform.GetComponent<SpriteRenderer>().flipX = false;
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            currentCell = referenceTileMap.WorldToCell(transform.position);
            Vector3Int rightCell = new Vector3Int(currentCell.x + 1, currentCell.y, 0);
            Move(rightCell);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            StopAllCoroutines();
            transform.GetComponent<SpriteRenderer>().flipX = true;
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            currentCell = referenceTileMap.WorldToCell(transform.position);
            Vector3Int leftCell = new Vector3Int(currentCell.x - 1, currentCell.y, 0);
            Move(leftCell);
        }
        */
        #endregion

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


        #region Movement_#3

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

    /*
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
    }
    */

    private bool CheckWall(Vector3Int _cell)
    {
        TileBase checkedTile = referenceTileMap.GetTile(_cell);
        return (checkedTile == dataFromTiles[tileData.tile].tile) ? true : false;
    }

    /*
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
    */
}
