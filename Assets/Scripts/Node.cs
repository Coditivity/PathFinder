using UnityEngine;
using System.Collections;
using System;

public class Node:IHeap<Node> {
    public const float DIAGONAL_COST = 14.1421356237f;
    public const float ORTHOGONAL_COST = 10;
    public bool isWalkable;
    public Vector3 position;
    public float gCost;
    public float hCost;
    public float weight;
    public Node prevNode;
    public float NetCost
    {
        get
        {
            return gCost + hCost;// + weight;
        }
    }
    public int rowIndex { get; private set; }
    public int colIndex { get; private set; }

    public int HeapIndex
    {
        get;
        set;
    }

    public Node(Vector3 position, int rowIndex, int colIndex)
    {
        prevNode = null;
        hCost = -.5f;
        gCost = -.5f;
        weight = 0;
        this.rowIndex = rowIndex;
        this.colIndex = colIndex;
        this.position = position;
        isWalkable = true;
    }

    public string GetString()
    {
        return "("+rowIndex + "," + colIndex+")"+" ";
    }

    public int CompareTo(Node other)
    {
        if(other.NetCost == NetCost)
        {
            return hCost.CompareTo(other.hCost) * -1;
        }
        return NetCost.CompareTo(other.NetCost) * -1;
    }
}
