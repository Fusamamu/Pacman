using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class PathFinder : MonoBehaviour
{
    private TileMapManager tileMapMG;

    public TileNode         current_Q_NODE;
    public List<TileNode>   openList;
    public List<TileNode>   closeList;

    public GameObject GHOST;
    public GameObject Pacman;

    public TileNode START_node;
    public TileNode TARGET_node;

    public List<TileNode> FinalPath;
    public bool PathIsFound = false;
    public bool DrawFinalPathEnabled = false;

    public int MAX_ITERATION = 70;

    public bool TURNONGIZMO = true;

    private void Start()
    {
        tileMapMG = TileMapManager.sharedInstance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StopAllCoroutines();
           
            Vector3Int startnode    = GHOST.GetComponent<Ghost>().currentCell;
            Vector3Int targetnode   = Pacman.GetComponent<Pacman>().currentCell;

            StartCoroutine(FindPath(startnode, targetnode));
        }
    }

    public void StartFindPath()
    {
        StopAllCoroutines();

        GameObject _pacman = GameObject.FindWithTag("Player");

        Vector3Int startnode  = GHOST.GetComponent<Ghost>().currentCell;
        Vector3Int targetnode = _pacman.GetComponent<Pacman>().currentCell;

        StartCoroutine(FindPath(startnode, targetnode));
    }

    public void StartFindPath(Vector3Int _targetnode)
    {
        StopAllCoroutines();
        Vector3Int _startnode = GHOST.GetComponent<Ghost>().currentCell;
        StartCoroutine(FindPath(_startnode, _targetnode));
    }

    public IEnumerator FindPath(Vector3Int startNode, Vector3Int targetNode)
    {
        PathIsFound = false;

        openList    = new List<TileNode>();
        closeList   = new List<TileNode>();

        START_node  = new TileNode(startNode);
        TARGET_node = new TileNode(targetNode);

        openList.Add(START_node);

        int iter = 0;

        while(openList.Count > 0 && !PathIsFound && iter < MAX_ITERATION)
        {
            iter++;

            TileNode q_node = openList[0];                           /* Q-NODE: Current selected tileNode */

            for (int i = 1; i < openList.Count; i++)                 /* Loop through OpenList, Try to Find the lowest F_cost in OpenList and set it to Q-NODE */
            {
                if(openList[i].F_cost <= q_node.F_cost)
                {
                    if (openList[i].H_cost < q_node.H_cost)
                        q_node = openList[i];

                    current_Q_NODE = q_node;
                }
                if (i%10 == 0)
                    yield return new WaitForSeconds(0.25f);
            }

            openList.Remove(q_node);
            closeList.Add(q_node);

            /*If path found, Retrace and return path*/
            if(q_node.Cell == targetNode)
            {
                TARGET_node = q_node;

                RetracePath(START_node, TARGET_node);
                StopAllCoroutines();

                PathIsFound = true;
            }

            /*Add neighbors to OpenList (4-Direction from Q_NODE)*/
            foreach(TileNode neighbor in tileMapMG.GetNeighborNodes(q_node.Cell))
            {
                /*Don't add to Openlist if it's wall or contains in Closelist ( continue keyword - skip to next foreach loop ) */
                if (neighbor.isWall || closeList.Exists(x => x.Cell == neighbor.Cell))
                    continue;

                int NEW_GCOST = q_node.G_cost + 1;

                //if (NEW_GCOST < neighbor.G_cost || !openList.Contains(neighbor))
                if (!openList.Contains(neighbor))
                {
                    neighbor.G_cost = NEW_GCOST;
                    neighbor.H_cost = tileMapMG.Get_H_Cost(neighbor.Cell, targetNode);
                    neighbor.Parent = q_node;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                };
            }
        }
    }

    public void RetracePath(TileNode startNode, TileNode targetNode)
    {
        List<TileNode> path = new List<TileNode>();

        TileNode currentNode = targetNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();

        FinalPath = path;
    }

    //Draws visual representation of grid
    void OnDrawGizmos()
    {
        if (TURNONGIZMO)
        {
            DrawOpenList();
            DrawCloseList();
            DrawFinalPath();
            Draw_Qnode();
        }
    }

    private void Draw_Qnode()
    {
        if (current_Q_NODE != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(tileMapMG.GetCellWorldPos(current_Q_NODE.Cell.x, current_Q_NODE.Cell.y), 0.3f);
        }
    }

    private void DrawCloseList()
    {
        if (closeList != null)
        {
            if (closeList.Count != 0)
            {
                foreach (TileNode tileNode in closeList)
                {
                    if (tileNode != null)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(tileMapMG.GetCellWorldPos(tileNode.Cell.x, tileNode.Cell.y), 0.25f);
                    }
                }
            }
        }
    }

    private void DrawOpenList()
    {
        if (openList != null)
        {
            if (openList.Count != 0)
            {
                foreach (TileNode tileNode in openList)
                {
                    if (tileNode != null)
                    {
                        Color c = Color.blue;
                        c.a = 0.5f;
                        Gizmos.color = c;
                        Gizmos.DrawSphere(tileMapMG.GetCellWorldPos(tileNode.Cell.x, tileNode.Cell.y), 0.25f);
                    }
                }
            }
        }
    }

    private void DrawFinalPath()
    {
        if (DrawFinalPathEnabled)
        {
            foreach (TileNode tileNode in FinalPath)
            {
                switch (GHOST.GetComponent<Ghost>().selectedGhostType)
                {
                    case Ghost.GhostType.BLUE:
                        Gizmos.color = Color.cyan;
                        break;
                    case Ghost.GhostType.ORANGE:
                        Gizmos.color = Color.magenta;
                        break;
                    case Ghost.GhostType.YELLOW:
                        Gizmos.color = Color.yellow;
                        break;
                    case Ghost.GhostType.GREEN:
                        Gizmos.color = Color.green;
                        break;
                }
                //Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(tileMapMG.GetCellWorldPos(tileNode.Cell.x, tileNode.Cell.y), 0.3f);
            }
        }
    }
}
