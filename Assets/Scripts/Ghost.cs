using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    private TileMapManager tilemapMG;

    private AI _AI;
    private GameObject PACMAN;
   
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
    private Queue<Vector3> waypoints;

    public enum    State { WAIT, INIT, SCATTER, CHASING_PLAYER, AVOIDING_PLAYER };
    public         State currentState;
    public enum    Direction { left, right, up, down }
    public         Direction currentDir = Direction.right;

    private float endWait;
    private float waitTime = 2f;

    public float chasingDistance = 8f;

    private void Awake()
    {
        _AI         = GetComponent<AI>();
        
    }

    private void Start()
    {
        tilemapMG = TileMapManager.sharedInstance;

        if (tilemapMG == null)
            Debug.Log("tilemap mg null");

        PACMAN = GameObject.FindWithTag("Player");
        InitializeGhost();

        currentCell = tilemapMG.GetCell(transform.position);
    }

    private void Update()
    {
        //if(Time.time > endWait && currentState != State.INIT && currentState != State.SCATTER)
        //{
        //    endWait = Time.time + waitTime;
            
        //    currentState = State.INIT;
        //    InitializeWaypoints(currentState);
        //}

        switch (currentState)
        {
            case State.WAIT:

                if(Time.time < endWait)
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
                    currentState = State.SCATTER;

                break;

            case State.SCATTER:

                UpdateMove(currentDir);
                _AI.ScatterLogic();

                currentState = Vector3.Distance(transform.position, PACMAN.transform.position) < chasingDistance ? State.CHASING_PLAYER : State.SCATTER;

                break;

            case State.CHASING_PLAYER:

                UpdateMove(currentDir);
                _AI.ChasingLogic();

                currentState = Vector3.Distance(transform.position, PACMAN.transform.position) > chasingDistance ? State.SCATTER : State.CHASING_PLAYER;

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

    public void InitializeGhost()
    {
        startCell       = currentCell;
        nextWayPoint    = transform.position;

        currentState    = State.WAIT;
        endWait         = Time.time + waitTime;

        InitializeWaypoints(currentState);
    }

    private void InitializeWaypoints(State _state)
    {
        waypoints = new Queue<Vector3>();

        //switch (selectedGhostType)
        //{
        //    case GhostType.BLUE:
        //        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 5));
        //        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 3));
        //        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
        //        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
        //        break;
        //    case GhostType.ORANGE:
        //        break;
        //    case GhostType.YELLOW:
        //        break;
        //    case GhostType.GREEN:
        //        break;
        //}

        switch (_state)
        {
            case State.WAIT:
                waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 5));
                waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 3));
                waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
           
                break;
            case State.INIT:
                //waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 5));
                //waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 3));
                waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
                waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 5));
                waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 7));
                waypoints.Enqueue(tilemapMG.GetCellWorldPos(4, 7));
                waypoints.Enqueue(tilemapMG.GetCellWorldPos(4, 1));
                break;
        }
    }

    void UpdateMoveByWayPoints_Looping(bool loop = false)
    {
        nextWayPoint = waypoints.Peek();
        currentCell = tilemapMG.GetCell(nextWayPoint);

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
            //transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    public void AvoidPlayer()
    {
        // currentState = State.AVOIDING_PLAYER;
        Debug.Log("Now ghost try to avoid player");
       
    }

   
}
