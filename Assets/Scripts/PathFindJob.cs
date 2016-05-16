﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct PathFindJob
{
    public Grid grid;
    public Vector3 from;
    public Vector3 to;
    public List<Node> intoPath;
    public bool drawGrid;
    public PathFindJob(Grid grid, Vector3 from, Vector3 to, List<Node> intoPath, bool drawGrid)
    {
        this.drawGrid = drawGrid;
        this.grid = grid;
        this.from = from;
        this.to = to;
        this.intoPath = intoPath;

    }
}