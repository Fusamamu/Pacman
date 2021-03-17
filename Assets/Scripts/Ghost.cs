using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    private TileMapManager tilemapMG;

    //public Tilemap referencedTileMap;
   
    public enum GhostType
    {
        BLUE, YELLOW, ORANGE, GREEN
    }
    public GhostType selectedGhostType = GhostType.BLUE;

    [Range(5.0f, 10.0f)]
    public float speed = 5.0f;

    public Vector3Int startCell;
    public Vector3Int currentCell;
    public Vector3Int targetCell;

    public Vector3 targetWaypoint;
    private Queue<Vector3> waypoints = new Queue<Vector3>();

    private enum    State { Wait, Init, Scatter, Chase, Run };
    private         State currentState;
    private enum    Direction { left, right, up, down }
    private         Direction currentDir = Direction.right;

    private float endWait;
    private float waitTime = 10.0f;

    private void Awake()
    {
        tilemapMG = TileMapManager.sharedInstance;
        InitializeGhost();
    }

    private void Update()
    {
        // currentCell = referencedTileMap.WorldToCell(transform.position);
        currentCell = tilemapMG.GetCell(transform.position);
        MoveToWaypoint(true);
    }

    public void InitializeGhost()
    {
        startCell       = new Vector3Int(0, 0, 0);
        targetWaypoint  = transform.position; 
        currentState    = State.Wait;
        endWait         = Time.time + waitTime;
        InitializeWaypoints(currentState);
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
        targetWaypoint = waypoints.Peek();

        if(Vector3.Distance(transform.position, targetWaypoint) > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
        }
        else
        {
            if (loop) waypoints.Enqueue(waypoints.Dequeue());
            else waypoints.Dequeue();
        }
    }
}
