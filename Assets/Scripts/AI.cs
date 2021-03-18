using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AI : MonoBehaviour
{
   // public static AI sharedInstance { get; set; }

    //public Tilemap  referenceTileMap;
    //public TileData tileData;
    //public Dictionary<TileBase, TileData> dataFromTiles;

    private TileMapManager tilemapMG;

    public Ghost ghost;


    public Vector3Int targetCell;

    private void Awake()
    {
      //  sharedInstance = this;
        
        ghost = GetComponent<Ghost>();
    }

    private void Start()
    {
        tilemapMG = TileMapManager.sharedInstance;
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        Vector3Int _checkedCell = new Vector3Int();
        Vector3Int _currentCell = ghost.currentCell;

        switch (ghost.currentDir)
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

        if (tilemapMG.isWall(_checkedCell))
        {
            int randomInt = Random.Range(0, 2);

            switch (ghost.currentDir)
            {
                case Ghost.Direction.right:
                    if (randomInt == 0)
                        ghost.currentDir = Ghost.Direction.up;
                    else
                        ghost.currentDir = Ghost.Direction.down;
                    break;
                case Ghost.Direction.left:
                    if (randomInt == 0)
                        ghost.currentDir = Ghost.Direction.up;
                    else
                        ghost.currentDir = Ghost.Direction.down;
                    break;
                case Ghost.Direction.up:
                    if (randomInt == 0)
                        ghost.currentDir = Ghost.Direction.left;
                    else
                        ghost.currentDir = Ghost.Direction.right;
                    break;
                case Ghost.Direction.down:
                    if(randomInt == 0)
                        ghost.currentDir = Ghost.Direction.left;
                    else
                        ghost.currentDir = Ghost.Direction.right;
                    break;
            }
        }
      
    }
}
