using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    private TileMapManager tilemapMG;

    private AI _AI;
   
    public enum GhostType
    {
        BLUE, YELLOW, ORANGE, GREEN
    }
    public GhostType selectedGhostType = GhostType.BLUE;

    [Range(3.0f, 10.0f)]
    public float speed = 3f;

    public Vector3Int startCell;
    public Vector3Int currentCell;
    public Vector3Int nextCell;

    public Vector3 nextPos;

    public Vector3 nextWayPoint;
    private Queue<Vector3> waypoints = new Queue<Vector3>();

    public enum    State { Wait, Init, Scatter, Chase, Run };
    public         State currentState;
    public enum     Direction { left, right, up, down }
    public          Direction currentDir = Direction.right;

    private float endWait;
    private float waitTime = 10.0f;

    private void Awake()
    {
        _AI         = GetComponent<AI>();
        InitializeGhost();
    }

    private void Start()
    {
        tilemapMG = TileMapManager.sharedInstance;

        if (tilemapMG == null)
            Debug.Log("tilemap mg null");

        currentCell = tilemapMG.GetCell(transform.position);
    }

    private void Update()
    {
         Move(currentDir);
         _AI.Move();
    }

    public void InitializeGhost()
    {
        startCell       = currentCell;
        nextWayPoint    = transform.position;

        currentState    = State.Wait;
        endWait         = Time.time + waitTime;
        //InitializeWaypoints(currentState);
    }

    private void InitializeWaypoints(State _state)
    {
        switch (selectedGhostType)
        {
            case GhostType.BLUE:
                waypoints.Enqueue(tilemapMG.GetCellWorldPos(0, 2));
                waypoints.Enqueue(tilemapMG.GetCellWorldPos(0, 5));
                waypoints.Enqueue(tilemapMG.GetCellWorldPos(3, 5));
                waypoints.Enqueue(tilemapMG.GetCellWorldPos(3, 2));
                break;
            case GhostType.ORANGE:
                break;
            case GhostType.YELLOW:
                break;
            case GhostType.GREEN:
                break;
        }
    }

    void MoveToWaypoint(bool loop = false)
    {
        nextWayPoint = waypoints.Peek();

        if(Vector3.Distance(transform.position, nextWayPoint) > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextWayPoint, speed * Time.deltaTime);
        }
        else
        {
            if (loop) waypoints.Enqueue(waypoints.Dequeue());
            else waypoints.Dequeue();
        }
    }

    public void Move(Direction _dir)
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

        if(Vector3.Distance(transform.position, tilemapMG.GetCellWorldPos(currentCell.x, currentCell.y)) > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, tilemapMG.GetCellWorldPos(currentCell.x, currentCell.y), speed * Time.deltaTime);
        }
        else
        {
            currentCell = tilemapMG.GetCell(nextPos);
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
    }
}
