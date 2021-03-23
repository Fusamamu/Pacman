using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class Pacman : MonoBehaviour
{
    private StateManager    stateManager;
    private TileMapManager  tilemapMG;

    public Tilemap referenceTileMap;

    [Range(5f, 100f)]
    public float speed = 5f;

    public Vector3Int currentCell;

    public Vector3Int current_RIGHTCELL;
    public Vector3Int current_LEFTCELL;

    public  float moveTime = 0.05f;

    private Vector3 targetPos;
    public  Vector2 direction = Vector2.zero;

    public UnityEvent UpdateScore;
    public UnityEvent OnPacmanDead;


    private void Awake()
    {
    }

    private void Start()
    {
        stateManager        = StateManager.sharedInstance;
        tilemapMG           = TileMapManager.sharedInstance;

        referenceTileMap    = tilemapMG.referencedTileMap;
        currentCell         = tilemapMG.GetCell(transform.position);
        targetPos           = transform.position;
    }

    private void Update()
    {
        switch (stateManager.currentGameState)
        {
            case StateManager.GAMESTATE.PLAYING:
                UpdateInput();
                UpdateMove();
                break;
        }

        if (direction != Vector2.zero)
            UpdateScore.Invoke();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ghost")
        {
            OnPacmanDead.Invoke();

            if (ScoreManager.sharedInstance.live > 0)
            {
                Pacman newPacman = Instantiate(this, tilemapMG.GetCellWorldPos(8, 1), Quaternion.identity, transform.parent);
                newPacman.GetComponent<FlickerAnimation>().SetAnimation(true);
            }
            else
            {
                stateManager.currentGameState = StateManager.GAMESTATE.GAMEOVER;
            }

            Destroy(gameObject);
        }
    }

    private void UpdateInput()
    {
        if (Input.GetKey(KeyCode.RightArrow) && direction.y == 0)
        {
            direction = Vector2.right;

            Vector3Int rightCell = new Vector3Int(currentCell.x + 1, currentCell.y, 0);

            if (!tilemapMG.isWall(rightCell))
                targetPos = referenceTileMap.GetCellCenterWorld(rightCell);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && direction.y == 0)
        {
            direction = Vector2.left;

            Vector3Int leftCell = new Vector3Int(currentCell.x - 1, currentCell.y, 0);

            if (!tilemapMG.isWall(leftCell))
                targetPos = referenceTileMap.GetCellCenterWorld(leftCell);
        }

        if (Input.GetKey(KeyCode.UpArrow) && direction.x == 0)
        {
            direction = Vector2.up;

            Vector3Int upCell = new Vector3Int(currentCell.x, currentCell.y + 1, 0);

            if (!tilemapMG.isWall(upCell))
                targetPos = referenceTileMap.GetCellCenterWorld(upCell);
        }

        if (Input.GetKey(KeyCode.DownArrow) && direction.x == 0)
        {
            direction = Vector2.down;

            Vector3Int downCell = new Vector3Int(currentCell.x, currentCell.y - 1, 0);

            if (!tilemapMG.isWall(downCell))
                targetPos = referenceTileMap.GetCellCenterWorld(downCell);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow))
            direction = Vector2.zero;
    }

    /* Check distance to current target cell position.
       While not reaching the target, keep moving toward.
       Having reached the target, set currentCell to next target. */

    private void UpdateMove()
    {
        Vector3 currentCellPos = tilemapMG.GetCellWorldPos(currentCell.x, currentCell.y);

        if (Vector3.Distance(transform.position, currentCellPos) > float.Epsilon)
            transform.position = Vector3.MoveTowards(transform.position, currentCellPos, speed * Time.deltaTime);
        else
            currentCell = tilemapMG.GetCell(targetPos);
    }
}
