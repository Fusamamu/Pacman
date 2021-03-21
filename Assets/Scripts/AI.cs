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

    /*
    public bool SwitchDir(Vector3Int _currentCell, Ghost.Direction _direction)
    {
        bool FoundWay = false;

        if(tilemapMG.isWall(GetNextCell(_currentCell, _direction)))
        {
            switch (_direction)
            {
                case Ghost.Direction.right:
                    GHOST.currentDir = Ghost.Direction.left;
                    break;
                case Ghost.Direction.left:
                    GHOST.currentDir = Ghost.Direction.right;
                    break;
                case Ghost.Direction.up:
                    GHOST.currentDir = Ghost.Direction.down;
                    break;
                case Ghost.Direction.down:
                    GHOST.currentDir = Ghost.Direction.up;
                    break;
            }
        }

        if (tilemapMG.isWall(GetNextCell(_currentCell, GHOST.currentDir)))
            SwitchDir(_currentCell, GHOST.currentDir);
        else
            FoundWay = true;

        return FoundWay;
    }

    public void SwitchDir2(Vector3Int _currentCell, Ghost.Direction _direction)
    {
       
        if (tilemapMG.isWall(GetNextCell(_currentCell, _direction)))
        {
            switch (_direction)
            {
                case Ghost.Direction.right:
                    GHOST.currentDir = Ghost.Direction.left;
                    break;
                case Ghost.Direction.left:
                    GHOST.currentDir = Ghost.Direction.right;
                    break;
                case Ghost.Direction.up:
                    GHOST.currentDir = Ghost.Direction.down;
                    break;
                case Ghost.Direction.down:
                    GHOST.currentDir = Ghost.Direction.up;
                    break;
            }
        }
    }
    */
}
