using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AI : MonoBehaviour
{
    private TileMapManager tilemapMG;

    public  Ghost       GHOST;
    private GameObject  PACMAN;

    public Vector3Int targetCell;

    public enum TargetOrientation
    {
        onLeft, onRight, onTop, onBottom
    }

    public TargetOrientation currentTargetOrien;


    public Vector3 nextWaypoint;
    public Queue<Vector3> waypoints;

    private void Awake()
    {
        GHOST = GetComponent<Ghost>();
    }

    private void Start()
    {
        tilemapMG   = TileMapManager.sharedInstance;
        PACMAN      = GameObject.FindWithTag("Player");
    }

    public void ScatterLogic()
    {
        if (tilemapMG.isWall(GetNextCell(GHOST.currentCell, GHOST.currentDir)))
        {
            int randomInt = Random.Range(0, 2);

            switch (GHOST.currentDir)
            {
                case Ghost.Direction.right:
                    if (randomInt == 0)
                        GHOST.currentDir = Ghost.Direction.up;
                    else
                        GHOST.currentDir = Ghost.Direction.down;
                    break;
                case Ghost.Direction.left:
                    if (randomInt == 0)
                        GHOST.currentDir = Ghost.Direction.up;
                    else
                        GHOST.currentDir = Ghost.Direction.down;
                    break;
                case Ghost.Direction.up:
                    if (randomInt == 0)
                        GHOST.currentDir = Ghost.Direction.left;
                    else
                        GHOST.currentDir = Ghost.Direction.right;
                    break;
                case Ghost.Direction.down:
                    if(randomInt == 0)
                        GHOST.currentDir = Ghost.Direction.left;
                    else
                        GHOST.currentDir = Ghost.Direction.right;
                    break;
            }
        }
    }

    public void ChasingLogic()
    {
        float Gx = GHOST.transform.position.x;
        float Gy = GHOST.transform.position.y;

        float Px = PACMAN.transform.position.x;
        float Py = PACMAN.transform.position.y;

        float diff_x = Mathf.Abs(Gx - Px);
        float diff_y = Mathf.Abs(Gy - Py);


        if (diff_x > diff_y)
            currentTargetOrien = Gx < Px ? TargetOrientation.onRight : TargetOrientation.onLeft;
        else
            currentTargetOrien = Gy < Py ? TargetOrientation.onTop : TargetOrientation.onBottom;

        TargetOrientation horizontal = Gx < Px ? TargetOrientation.onRight : TargetOrientation.onLeft;
        TargetOrientation vertical   = Gy < Py ? TargetOrientation.onTop : TargetOrientation.onBottom;

        #region ChasingLogic_1
        //if (horizontal == TargetOrientation.onRight && vertical == TargetOrientation.onTop)
        //{
        //    GHOST.currentDir = Ghost.Direction.right;

        //    if (tilemapMG.isWall(GHOST.rightCell))
        //        GHOST.currentDir = Ghost.Direction.up;

        //    if (tilemapMG.isWall(GHOST.topCell))
        //        GHOST.currentDir = Ghost.Direction.right;

        //    if (tilemapMG.isWall(GHOST.rightCell) && tilemapMG.isWall(GHOST.topCell))
        //        GHOST.currentDir = Ghost.Direction.left;
        //}

        //if (horizontal == TargetOrientation.onLeft && vertical == TargetOrientation.onTop)
        //{
        //    GHOST.currentDir = Ghost.Direction.left;

        //    if (tilemapMG.isWall(GHOST.leftCell))
        //        GHOST.currentDir = Ghost.Direction.up;

        //    if (tilemapMG.isWall(GHOST.topCell))
        //        GHOST.currentDir = Ghost.Direction.left;

        //    if (tilemapMG.isWall(GHOST.leftCell) && tilemapMG.isWall(GHOST.topCell))
        //        GHOST.currentDir = Ghost.Direction.right;
        //}
        #endregion

        #region ChasingLocgic_2
        switch (currentTargetOrien)
        {
            case TargetOrientation.onRight:

                if (!tilemapMG.isWall(GHOST.rightCell))
                {
                    GHOST.currentDir = Ghost.Direction.right;
                }
                else
                {
                    if (GHOST.currentDir == Ghost.Direction.up || GHOST.currentDir == Ghost.Direction.down)
                        return;

                    if (!tilemapMG.isWall(GHOST.topCell))
                    {
                        GHOST.currentDir = Ghost.Direction.up;
                    }
                    else
                    {
                        GHOST.currentDir = Ghost.Direction.down;
                    }
                }
                break;

            case TargetOrientation.onLeft:

                if (!tilemapMG.isWall(GHOST.leftCell))
                {
                    GHOST.currentDir = Ghost.Direction.left;
                }
                else
                {
                    if (!tilemapMG.isWall(GHOST.topCell))
                    {
                        GHOST.currentDir = Ghost.Direction.up;

                    }
                    else
                    {
                        GHOST.currentDir = Ghost.Direction.down;
                    }
                }
                break;
            case TargetOrientation.onTop:

                if (!tilemapMG.isWall(GHOST.topCell))
                {
                    GHOST.currentDir = Ghost.Direction.up;
                }
                else
                {
                    if (!tilemapMG.isWall(GHOST.leftCell))
                    {
                        GHOST.currentDir = Ghost.Direction.left;

                    }
                    else
                    {
                        GHOST.currentDir = Ghost.Direction.right;
                    }
                }
                break;
            case TargetOrientation.onBottom:

                if (!tilemapMG.isWall(GHOST.bottomCell))
                {
                    GHOST.currentDir = Ghost.Direction.down;
                }
                else
                {
                    if (!tilemapMG.isWall(GHOST.leftCell))
                    {
                        GHOST.currentDir = Ghost.Direction.left;
                    }
                    else
                    {
                        GHOST.currentDir = Ghost.Direction.right;
                    }
                }
                break;
        }
        #endregion

        #region ChasingLogic_3(Attempt to slove horizontal movement first then vertical)
        //if (diff_x > diff_y)
        //{
        //    if (horizontal == TargetOrientation.onRight)
        //    {

        //        if (!tilemapMG.isWall(GHOST.rightCell))
        //        {
        //            GHOST.currentDir = Ghost.Direction.right;
        //        }
        //        else
        //        {
        //            if (!tilemapMG.isWall(GHOST.topCell))
        //            {
        //                GHOST.currentDir = Ghost.Direction.up;
        //            }

        //        }
        //    }

        //    if (horizontal == TargetOrientation.onLeft)
        //    {
        //        if (!tilemapMG.isWall(GHOST.leftCell))
        //        {
        //            GHOST.currentDir = Ghost.Direction.left;
        //        }
        //        else
        //        {
        //            if (!tilemapMG.isWall(GHOST.topCell))
        //            {
        //                GHOST.currentDir = Ghost.Direction.up;

        //            }

        //        }
        //    }
        //}

        //if(diff_y > diff_x)
        //{
        //    if (vertical == TargetOrientation.onTop)
        //    {
        //        if (!tilemapMG.isWall(GHOST.topCell))
        //        {
        //            GHOST.currentDir = Ghost.Direction.up;
        //        }
        //        else
        //        {
        //            if (!tilemapMG.isWall(GHOST.leftCell))
        //                GHOST.currentDir = Ghost.Direction.left;
        //        }
        //    }

        //    if (vertical == TargetOrientation.onBottom)
        //    {
        //        if (!tilemapMG.isWall(GHOST.bottomCell))
        //        {
        //            GHOST.currentDir = Ghost.Direction.down;
        //        }
        //    }
        //}
        #endregion
    }

    public Vector3Int GetNextCell(Vector3Int _currentCell, Ghost.Direction _direction)
    {
        Vector3Int _checkedCell = new Vector3Int();

        switch (_direction)
        {
            case Ghost.Direction.right:
                _checkedCell = new Vector3Int(_currentCell.x + 1, _currentCell.y, 0);
                break;
            case Ghost.Direction.left:
                _checkedCell = new Vector3Int(_currentCell.x - 1, _currentCell.y, 0);
                break;
            case Ghost.Direction.up:
                _checkedCell = new Vector3Int(_currentCell.x, _currentCell.y + 1, 0);
                break;
            case Ghost.Direction.down:
                _checkedCell = new Vector3Int(_currentCell.x, _currentCell.y - 1, 0);
                break;
        }

        return _checkedCell;
    }

    public void InitializeWaypoints(Ghost.State _state)
    {
        waypoints = new Queue<Vector3>();

        switch (GHOST.selectedGhostType)
        {
            case Ghost.GhostType.BLUE:
                switch (_state)
                {
                    case Ghost.State.WAIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));

                        break;
                    case Ghost.State.INIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 7));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(4, 7));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(4, -2));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(1, -2));
                        break;
                    case Ghost.State.SCATTER:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(1, -5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(4, -5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(4, -8));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(7, -8));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(7, -11));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(-4, -11));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(-4, -8));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(1, -8));
                        break;
                }
                break;

            case Ghost.GhostType.ORANGE:
                switch (_state)
                {
                    case Ghost.State.WAIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
                        break;

                    case Ghost.State.INIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 7));

                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(10, 7));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(10, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(13, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(13, 13));
                        break;

                    case Ghost.State.SCATTER:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(10, 13));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(10, 17));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(21, 17));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(21, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(16, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(16, 13));
                        break;
                }
                break;

            case Ghost.GhostType.YELLOW:
                switch (_state)
                {
                    case Ghost.State.WAIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));

                        break;
                    case Ghost.State.INIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 7));

                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(7, 7));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(7, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(4, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(4, 13));
                        break;

                    case Ghost.State.SCATTER:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(7, 13));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(7, 17));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(-4, 17));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(-4, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(1, 10));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(1, 13));
                        break;
                }
                break;

            case Ghost.GhostType.GREEN:
                switch (_state)
                {
                    case Ghost.State.WAIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(6, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
                        break;

                    case Ghost.State.INIT:
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 3));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(11, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 5));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(8, 7));

                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(13, 7));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(13, -2));
                        waypoints.Enqueue(tilemapMG.GetCellWorldPos(16, -2));
                        break;

                    case Ghost.State.SCATTER:
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

    public Vector3Int GetBaseCell(Ghost.GhostType _ghostType)
    {
        switch (_ghostType)
        {
            case Ghost.GhostType.BLUE:
                return new Vector3Int(1, -5, 0);
            case Ghost.GhostType.ORANGE:
                return new Vector3Int(10, 13, 0);
            case Ghost.GhostType.YELLOW:
                return new Vector3Int(7, 13, 0);
            case Ghost.GhostType.GREEN:
                return new Vector3Int(16, -5, 0);
        }
        return Vector3Int.zero;
    }
}
