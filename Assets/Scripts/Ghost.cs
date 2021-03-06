using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    private StateManager stateManager;
    private TileMapManager tilemapMG;

    private AI          _AI;
    private PathFinder  _PATHFINDER;
    private GameObject  _PACMAN;
   
    public enum GhostType { BLUE, YELLOW, ORANGE, GREEN }
    public GhostType selectedGhostType = GhostType.BLUE;

    [Range(3.0f, 10.0f)]
    public float speed = 3f;

    public Vector3Int currentCell;

    public Vector3Int rightCell     { get { return new Vector3Int(currentCell.x + 1, currentCell.y, currentCell.z); } }
    public Vector3Int leftCell      { get { return new Vector3Int(currentCell.x - 1, currentCell.y, currentCell.z); } }
    public Vector3Int topCell       { get { return new Vector3Int(currentCell.x, currentCell.y + 1, currentCell.z); } }
    public Vector3Int bottomCell    { get { return new Vector3Int(currentCell.x, currentCell.y - 1, currentCell.z); } }

    public enum    State { WAIT, INIT, SCATTER, CHASING_PLAYER, AVOIDING_PLAYER, RETURN_BASE };
    public         State currentState;
    public enum    Direction { left, right, up, down }
    public         Direction currentDir = Direction.right;

    private const float WAIT_TIME       = 1f;
    private const float SCATTER_TIME    = 8f;
    private const float CHASE_TIME      = 10f;
    private const float SCARE_TIME      = 4f;

    private float SWITCH_STATE_TIME;
    private float NEXT_PATHCALCULATION_TIME = 0;

    [Range(4f, 30f)]
    public float chasingDistance = 8f;

    [Range(0.0f, 1.0f)]
    public float gizmoAlphaValue = 0.5f;

    private void Awake()
    {
        _AI = GetComponent<AI>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(currentState == State.AVOIDING_PLAYER)
            {
                GetComponent<FlickerAnimation>().SetAnimation(false);
                GetComponent<SpriteRenderer>().color = Color.red;
                GetComponent<Collider2D>().enabled = false;
                currentState = State.RETURN_BASE;
            }
        }
    }

    private void Start()
    {
        stateManager    = StateManager.sharedInstance;
        tilemapMG       = TileMapManager.sharedInstance;

        _PATHFINDER     = GetComponent<PathFinder>();
        _PACMAN         = GameObject.FindWithTag("Player");

        currentCell     = tilemapMG.GetCell(transform.position);

        currentState = State.WAIT;
        _AI.InitializeWaypoints(currentState);

        SWITCH_STATE_TIME = Time.time + WAIT_TIME;
    }

    private void Update()
    {
        switch (stateManager.currentGameState)
        {
            case StateManager.GAMESTATE.PLAYING:
                UpdateCurrentGhostState();
                break;
            case StateManager.GAMESTATE.GAMEOVER:

                break;
        }
    }

    private void UpdateCurrentGhostState()
    {
        switch (currentState)
        {
            case State.WAIT:

                speed = 4.0f;

                if (Time.time < SWITCH_STATE_TIME)
                {
                    UpdateMove(_AI.waypoints, true);
                }
                else
                {
                    /*Reset Color to white after returing to wating room */
                    GetComponent<SpriteRenderer>().color = Color.white;

                    UpdateMove(_AI.waypoints, false);

                    if (_AI.waypoints.Count == 0)
                    {
                        currentState = State.INIT;
                        _AI.InitializeWaypoints(currentState);
                    }
                }
                break;

            case State.INIT:

                UpdateMove(_AI.waypoints, false);

                if (_AI.waypoints.Count == 0)
                {
                    currentState = State.SCATTER;
                    _AI.InitializeWaypoints(currentState);

                    SWITCH_STATE_TIME = Time.time + SCATTER_TIME;
                }
                break;

            case State.SCATTER:

                if (Time.time < SWITCH_STATE_TIME)
                {
                    UpdateMove(_AI.waypoints, true);
                }
                else
                {
                    _AI.waypoints.Clear();
                    currentCell  = tilemapMG.GetCell(transform.position);
                    currentState = State.CHASING_PLAYER;

                    SWITCH_STATE_TIME = Time.time + CHASE_TIME;
                }
                break;

            case State.CHASING_PLAYER:

                speed = 4.0f;

                if (Time.time < SWITCH_STATE_TIME)
                {
                    if (Time.time > NEXT_PATHCALCULATION_TIME)
                    {
                        _PATHFINDER.StartFindPath();
                    }

                    if (_PATHFINDER.PathIsFound)
                    {
                        _AI.waypoints.Clear();

                        foreach (TileNode node in _PATHFINDER.FinalPath)
                        {
                            _AI.waypoints.Enqueue(tilemapMG.GetCellWorldPos(node.Cell.x, node.Cell.y));
                        }
                        _PATHFINDER.PathIsFound = false;
                        _PATHFINDER.DrawFinalPathEnabled = true;
                        NEXT_PATHCALCULATION_TIME = Time.time + 3f;
                    }

                    if (_AI.waypoints.Count != 0)
                    {
                        UpdateMove(_AI.waypoints, false);
                    }
                    else
                    {
                        _AI.ScatterLogic();
                        UpdateMove(currentDir);
                    }
                }
                else
                {
                    if (Time.time > NEXT_PATHCALCULATION_TIME)
                    {
                        _PATHFINDER.StartFindPath(_AI.GetBaseCell(selectedGhostType));

                        if (_PATHFINDER.PathIsFound)
                        {
                            _AI.waypoints.Clear();

                            foreach (TileNode node in _PATHFINDER.FinalPath)
                            {
                                _AI.waypoints.Enqueue(tilemapMG.GetCellWorldPos(node.Cell.x, node.Cell.y));
                            }

                            _PATHFINDER.DrawFinalPathEnabled = true;
                            NEXT_PATHCALCULATION_TIME = Time.time + 3f;
                        }
                    }

                    if (_AI.waypoints.Count != 0)
                    {
                        UpdateMove(_AI.waypoints, false);

                        if (_AI.waypoints.Count == 0 && _PATHFINDER.PathIsFound)
                        {
                            currentState = State.SCATTER;
                            _AI.InitializeWaypoints(currentState);

                            SWITCH_STATE_TIME = Time.time + SCATTER_TIME;
                        }
                    }
                    else
                    {
                        _AI.ScatterLogic();
                        UpdateMove(currentDir);
                    }
                }
                break;

            case State.AVOIDING_PLAYER:

                speed = 2.5f;

                _AI.ScatterLogic();
                UpdateMove(currentDir);

                if (Time.time > SWITCH_STATE_TIME)
                {
                    currentState = State.CHASING_PLAYER;
                    SWITCH_STATE_TIME = Time.time + CHASE_TIME;
                }

                break;

            case State.RETURN_BASE:

                speed = 6.0f;

                if (Time.time > NEXT_PATHCALCULATION_TIME)
                {
                    _PATHFINDER.StartFindPath(_AI.GetWaitRoom());

                    if (_PATHFINDER.PathIsFound)
                    {
                        _AI.waypoints.Clear();

                        foreach (TileNode node in _PATHFINDER.FinalPath)
                        {
                            _AI.waypoints.Enqueue(tilemapMG.GetCellWorldPos(node.Cell.x, node.Cell.y));
                        }

                        _PATHFINDER.DrawFinalPathEnabled = true;
                        NEXT_PATHCALCULATION_TIME = Time.time + 3f;
                    }
                }

                if (_AI.waypoints.Count != 0)
                {
                    UpdateMove(_AI.waypoints, false);

                    if (_AI.waypoints.Count == 0 && _PATHFINDER.PathIsFound)
                    {
                        currentState = State.WAIT;
                        _AI.InitializeWaypoints(currentState);

                        SWITCH_STATE_TIME = Time.time + WAIT_TIME;
                    }
                }
                else
                {
                    _AI.ScatterLogic();
                    UpdateMove(currentDir);
                }

                break;
        }
    }

    public void UpdateMove(Queue<Vector3> _waypoints, bool loop = false)
    {
        Vector3 _nextpoint  = _waypoints.Peek();
        currentCell         = tilemapMG.GetCell(_nextpoint);

        if (Vector3.Distance(transform.position, _nextpoint) > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, _nextpoint, speed * Time.deltaTime);
        }
        else
        {
            if (loop) _waypoints.Enqueue(_waypoints.Dequeue());
            else _waypoints.Dequeue();
        }
    }

    public void UpdateMove(Direction _dir)
    {
        Vector3 nextPos = Vector3.zero;

        switch (_dir)
        {
            case Direction.right:
                nextPos = tilemapMG.GetCellWorldPos(currentCell.x + 1, currentCell.y);
                break;
            case Direction.left:
                nextPos = tilemapMG.GetCellWorldPos(currentCell.x - 1, currentCell.y);
                break;
            case Direction.up:
                nextPos = tilemapMG.GetCellWorldPos(currentCell.x, currentCell.y + 1);
                break;
            case Direction.down:
                nextPos = tilemapMG.GetCellWorldPos(currentCell.x, currentCell.y - 1);
                break;
        }

        Vector3 currentCellPos = tilemapMG.GetCellWorldPos(currentCell.x, currentCell.y);

        if(Vector3.Distance(transform.position, currentCellPos) > float.Epsilon)
            transform.position = Vector3.MoveTowards(transform.position, currentCellPos, speed * Time.deltaTime);
        else
            currentCell = tilemapMG.GetCell(nextPos);
    }

    public void AvoidPlayer()
    {
        if (currentState == State.WAIT || currentState == State.INIT || currentState == State.RETURN_BASE)
            return;

        GetComponent<FlickerAnimation>().SetAnimation(true);
        
        currentState = State.AVOIDING_PLAYER;
        SWITCH_STATE_TIME = Time.time + SCARE_TIME;
    }

    private void OnDrawGizmos()
    {
        Color fadeYellow    = Color.yellow;
        fadeYellow.a        = gizmoAlphaValue;
        Gizmos.color        = fadeYellow;
        Gizmos.DrawWireSphere(transform.position, chasingDistance);
    }
}
