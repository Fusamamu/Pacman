using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    private TileMapManager tilemapMG;

    private AI          _AI;
    private PathFinder  _PATHFINDER;
    private GameObject  _PACMAN;
   
    public enum GhostType
    {
        BLUE, YELLOW, ORANGE, GREEN
    }
    public GhostType selectedGhostType = GhostType.BLUE;

    [Range(3.0f, 10.0f)]
    public float speed = 3f;

    public Vector3Int startCell;
    public Vector3Int currentCell;

    public Vector3Int rightCell     { get { return new Vector3Int(currentCell.x + 1, currentCell.y, currentCell.z); } }
    public Vector3Int leftCell      { get { return new Vector3Int(currentCell.x - 1, currentCell.y, currentCell.z); } }
    public Vector3Int topCell       { get { return new Vector3Int(currentCell.x, currentCell.y + 1, currentCell.z); } }
    public Vector3Int bottomCell    { get { return new Vector3Int(currentCell.x, currentCell.y - 1, currentCell.z); } }

    public Vector3Int nextCell;

    public Vector3 nextPos;

    public Vector3 nextWayPoint;
    public Queue<Vector3> waypoints;

    public enum    State { WAIT, INIT, SCATTER, CHASING_PLAYER, AVOIDING_PLAYER };
    public         State currentState;
    public enum    Direction { left, right, up, down }
    public         Direction currentDir = Direction.right;

    private float SWITCH_STATE_TIME;

    private float wait_TIME      = 2f;
    private float scatter_TIME   = 4f;
    private float chasing_TIME   = 4f;
    private float nextPathCalcuation_TIME;

    [Range(4f, 30f)]
    public float chasingDistance = 8f;

    [Range(0.0f, 1.0f)]
    public float gizmoAlphaValue = 0.5f;

    private void Awake()
    {
        _AI = GetComponent<AI>();
        
    }

    private void Start()
    {
        tilemapMG       = TileMapManager.sharedInstance;
        _PATHFINDER     = GetComponent<PathFinder>();
        _PACMAN         = GameObject.FindWithTag("Player");
        currentCell     = tilemapMG.GetCell(transform.position);
        InitializeGhost();


        nextPathCalcuation_TIME = Time.time;
    }

    public void InitializeGhost()
    {
        //startCell       = currentCell;
        nextWayPoint    = transform.position;

        SWITCH_STATE_TIME         = Time.time + wait_TIME;
        currentState    = State.WAIT;
        InitializeWaypoints(currentState);
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.WAIT:

                if(Time.time < SWITCH_STATE_TIME)
                {
                    UpdateMoveByWayPoints_Looping(true);
                }
                else
                {
                    UpdateMoveByWayPoints_Looping(false);

                    if(waypoints.Count == 0)
                    {
                        currentState = State.INIT;
                        InitializeWaypoints(currentState);
                    }
                }
                break;

            case State.INIT:
                
                UpdateMoveByWayPoints_Looping(false);

                if (waypoints.Count == 0)
                {
                    currentState = State.SCATTER;
                    InitializeWaypoints(currentState);
                    SWITCH_STATE_TIME = Time.time + scatter_TIME;
                }

                break;

            case State.SCATTER:

               // UpdateMoveByWayPoints_Looping(true);

                if (Time.time < SWITCH_STATE_TIME)
                {
                    UpdateMoveByWayPoints_Looping(true);
                }
                else
                {
                    waypoints.Clear();
                    currentState = State.CHASING_PLAYER;
                    SWITCH_STATE_TIME = Time.time + chasing_TIME;
                }


                //UpdateMove(currentDir);
                //_AI.ScatterLogic();

                //currentState = Vector3.Distance(transform.position, PACMAN.transform.position) < chasingDistance ? State.CHASING_PLAYER : State.SCATTER;

                break;

            case State.CHASING_PLAYER:

                // StartCoroutine(_PATHFINDER.StartFindPath());
                
                if(Time.time > nextPathCalcuation_TIME)
                {
                    Debug.Log("Start Calculate new paht");
                    _PATHFINDER.StartFindPath();
                    nextPathCalcuation_TIME = Time.time + 20f;
                }

                

                if (_PATHFINDER.PathIsFound && Time.time < nextPathCalcuation_TIME)
                {
                    //waypoints.Clear();

                    foreach (TileNode node in _PATHFINDER.FinalPath)
                    {
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(node.Cell.x, node.Cell.y));
                    }

                }
                else
                {
                    
                    _AI.ScatterLogic();
                    UpdateMove(currentDir);
                }

                
                    UpdateMoveByWayPoints_Looping(false);

                //UpdateMove(currentDir);
                //_AI.ChasingLogic();

                //currentState = Vector3.Distance(transform.position, PACMAN.transform.position) > chasingDistance ? State.SCATTER : State.CHASING_PLAYER;

                break;
            case State.AVOIDING_PLAYER:
                break;
        }


        //Checking Distance to player
        //if close enought
        //state to chase
        //if far enoght
        //state to scatter
    }

    private void InitializeWaypoints(State _state)
    {
        waypoints = new Queue<Vector3>();

        switch (selectedGhostType)
        {
            case GhostType.BLUE:
                switch (_state)
                {
                    case State.WAIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));

                        break;
                    case State.INIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 7));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(4, 7));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(4, -2));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(1, -2));
                        break;
                    case State.SCATTER:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(1, -5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(4, -5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(4, -8));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(7, -8));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(7, -11));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(-4,-11));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(-4, -8));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos( 1, -8));
                        break;
                }
                break;

            case GhostType.ORANGE:
                switch (_state)
                {
                    case State.WAIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
                        break;

                    case State.INIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 7));

                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(10, 7));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(10, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(13, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(13, 13));
                        break;

                    case State.SCATTER:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(10, 13));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(10, 17));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(21, 17));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(21, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(16, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(16, 13));
                        break;
                }
                break;
          
            case GhostType.YELLOW:
                switch (_state)
                {
                    case State.WAIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));

                        break;
                    case State.INIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 7));

                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(7, 7));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(7, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(4, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(4, 13));
                        break;

                    case State.SCATTER:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(7, 13));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(7, 17));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(-4, 17));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(-4, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(1, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(1, 13));
                        break;
                }
                break;

            case GhostType.GREEN:
                switch (_state)
                {
                    case State.WAIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
                        break;

                    case State.INIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 7));

                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(13, 7));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(13, -2));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(16, -2));
                        break;

                    case State.SCATTER:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(16, -5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(13, -5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(13, -8));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(10, -8));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(10, -11));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(21, -11));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(21, -8));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(16, -8));
                        break;
                }
                break;
        }
    }

    void UpdateMoveByWayPoints_Looping(bool loop = false)
    {
        nextWayPoint = waypoints.Peek();
        currentCell  = tilemapMG.GetCell(nextWayPoint);

        if (Vector3.Distance(transform.position, nextWayPoint) > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextWayPoint, speed * Time.deltaTime);
        }
        else
        {
            if (loop) waypoints.Enqueue(waypoints.Dequeue());
            else waypoints.Dequeue();
        }
    }

    public void UpdateMove(Direction _dir)
    {
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
        Debug.Log("Now ghost try to avoid player");
    }

    private void OnDrawGizmos()
    {
        Color fadeYellow    = Color.yellow;
        fadeYellow.a        = gizmoAlphaValue;
        Gizmos.color        = fadeYellow;
        Gizmos.DrawWireSphere(transform.position, chasingDistance);
    }
}
