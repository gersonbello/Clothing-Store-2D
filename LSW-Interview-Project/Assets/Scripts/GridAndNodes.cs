using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Generated based on A* Pathfinding from Sebastion Lague - https://www.youtube.com/watch?v=-L-WgKMFuhE
public class GridAndNodes : MonoBehaviour
{
    // The base world grid
    public Node[,] grid { get; private set; }

    [Header("Grid Configuration")]
    [Tooltip("Grid node diameter")]
    [SerializeField]
    private float nodeDiameter;
    [Tooltip("Grid XY scale")]
    [SerializeField]
    private Vector2 gridWorldScale;
    [Tooltip("Obstacles Layer")]
    [SerializeField]
    private LayerMask obstaclesLayers;
    [Tooltip("Consider diagonal nodes")]
    [SerializeField]
    private bool diagonalParents = true;

    // Last Path
    List<Node> lastPath = new List<Node>();

    /// <summary>
    /// The grid node XY count
    /// </summary>
    private int gridSizeX, gridSizeY;

    void Start()
    {
        float nodeRadius = nodeDiameter / 2;
        gridSizeX = (int)(gridWorldScale.x / nodeDiameter);
        gridSizeY = (int)(gridWorldScale.y / nodeDiameter);
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 bottomLeftGridPosition = (Vector2)transform.position + Vector2.left * gridWorldScale.x / 2 + Vector2.down * gridWorldScale.y / 2;
        for(int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 nodePosition = bottomLeftGridPosition + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics2D.OverlapBox(nodePosition, new Vector2(nodeRadius, nodeRadius) , 0, obstaclesLayers);
                grid[x, y] = new Node(new Vector2(x, y), nodePosition, walkable);
            }
        }
    }

    /// <summary>
    /// Return the equivalent node in the world position
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public Node FromWorldPointToNode(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldScale.x / 2) / gridWorldScale.x;
        float percentY = (worldPosition.y + gridWorldScale.y / 2) / gridWorldScale.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }
    public Vector3 FromWorldPointToNodePosition(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldScale.x / 2) / gridWorldScale.x;
        float percentY = (worldPosition.y + gridWorldScale.y / 2) / gridWorldScale.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y].nodedWorldPosition;
    }

    /// <summary>
    /// Get Nodes around the selected node
    /// </summary>
    /// <param name="node">Selected node</param>
    /// <returns></returns>
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0 || Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1 && !diagonalParents) continue;

                int checkX = (int)node.nodeIndex.x + x;
                int checkY = (int)node.nodeIndex.y + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    if(Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1 && !diagonalParents)
                        grid[checkX, checkY].walkable = !Physics2D.OverlapBox(grid[checkX, checkY].nodedWorldPosition, new Vector2(nodeDiameter, nodeDiameter) * 2, 0, obstaclesLayers);
                    else
                        grid[checkX, checkY].walkable = !Physics2D.OverlapBox(grid[checkX, checkY].nodedWorldPosition, new Vector2(nodeDiameter, nodeDiameter) / 2, 0, obstaclesLayers);
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    /// <summary>
    /// Gets a path list with nodes that dont colide
    /// </summary>
    /// <param name="startPos">Start position</param>
    /// <param name="target">Target Position</param>
    /// <param name="path">Path list reference</param>
    public void FindPath(Vector3 startPos, Vector3 target, ref List<Node> path)
    {
        Node startNode = FromWorldPointToNode(startPos);
        Node targetNode = FromWorldPointToNode(target);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].gCost <= currentNode.gCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                    currentNode.walkable = !Physics2D.OverlapBox(currentNode.nodedWorldPosition, new Vector2(nodeDiameter, nodeDiameter) / 2, 0, obstaclesLayers);
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if(currentNode == targetNode)
            {
                RetracePath(startNode, targetNode, ref path);
                return;
            }

            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                { continue; }

                int newMovementCostNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode, ref List<Node> path)
    {
        List<Node> newPath = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            newPath.Add(currentNode);
            currentNode = currentNode.parent;
        }
        newPath.Reverse();

        lastPath = newPath;
        path = newPath;
    }

    /// <summary>
    /// Get the distance in nodes from two nodes
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>
    /// <returns></returns>
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs((int)(nodeA.nodeIndex.x - nodeB.nodeIndex.x));
        int dstY = Mathf.Abs((int)(nodeA.nodeIndex.y - nodeB.nodeIndex.y));

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    #if UNITY_EDITOR
    [Tooltip("Show/Hide the node gizmo")]
    public bool showGrid;
    /// <summary>
    /// Show gizmos on editor
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!showGrid) return;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldScale.x, gridWorldScale.y, 1));
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = n.walkable ? Color.blue : Color.red;
                if (lastPath != null && lastPath.Contains(n)) Gizmos.color = Color.black;
                Gizmos.DrawCube(n.nodedWorldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
    #endif


}

/// <summary>
/// Node informations for create a grid
/// </summary>
public class Node
{
    [Tooltip("Node X and Y based on grid size")]
    public Vector2 nodeIndex;
    [Tooltip("Node world position")]
    public Vector2 nodedWorldPosition;
    [Tooltip("Inform if the node has no object inside")]
    public bool walkable;

    [Tooltip("The cost for move into this node")]
    public int gCost, hCost;
    [Tooltip("Node parent for pathfinding")]
    public Node parent;

    public Node(Vector2 _nodeIndex, Vector2 _nodedWorldPosition, bool _walkable)
    {
        nodeIndex = _nodeIndex;
        nodedWorldPosition = _nodedWorldPosition;
        walkable = _walkable;
    }
}
