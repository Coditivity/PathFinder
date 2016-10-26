using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFindJob
{
    public Grid grid;
    public Vector3 from;
    public Vector3 to;
    public List<Node> intoPath;
    public bool drawGrid;
    public Node endNode;
    public Node endNodeBest; //if the targetNode is not reachable, this will be the new targetNode
    public bool bEndNodeReachable; //whether or not the target node was reachable
    public float colliderRadius;
    public PathFindJob(Grid grid, Vector3 from, Vector3 to, List<Node> intoPath, bool drawGrid, float colliderRadius)
    {
        this.drawGrid = drawGrid;
        this.grid = grid;
        this.from = from;
        this.to = to;
        this.intoPath = intoPath;
        this.colliderRadius = colliderRadius;
        this.endNode = null;
        this.endNodeBest = null;
        bEndNodeReachable = true;
    }

    public void SetNodesAndStatus(Node endNode, Node endNodeBest, bool bEndNodeReachable)
    {
        this.endNode = endNode;
        this.endNodeBest = endNodeBest;
        this.bEndNodeReachable = bEndNodeReachable;
    }
}