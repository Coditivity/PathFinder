using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder {

    
    Node startNode;
    public Node endNode;
    Grid grid;
    List<Node> path;
    private static PathFinder pathFinder = null;
    public static PathFinder instance
    {
        get
        {            
            return pathFinder;
        }
    }

    public PathFinder()
    {
        startNode = null;
        endNode = null;
        grid = null;
       
    }

    public static void Init()
    {
        if(pathFinder == null)
        {
            pathFinder = new PathFinder();
        }
    }
    
    public static void SetAndStart(Grid grid, Vector3 start, Vector3 target, List<Node> outPath)
    {
        instance.grid = grid;
        instance.startNode = instance.GetNodeFromWorldPostion(start);
        instance.endNode = instance.GetNodeFromWorldPostion(target);
        instance.path = outPath;
        instance.grid.ClosedNodes.Add(instance.startNode);
        instance.FindPath(instance.startNode);

    }

    public static void SetAndStart(PathFindJob job)
    {
        instance.grid = job.grid;
        instance.startNode = instance.GetNodeFromWorldPostion(job.from);
        instance.endNode = instance.GetNodeFromWorldPostion(job.to);
        instance.path = job.intoPath;
        instance.grid.ClosedNodes.Add(instance.startNode);
        instance.FindPath(instance.startNode);
    }

    Node[] retNodes = new Node[8];
    public Node[] getAdjacentNodes(Node node)
    {

        int retNodeCount = 0;
        int rowIndex = node.rowIndex - 1;
        int colIndex = node.colIndex - 1;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (rowIndex >= 0
                    && colIndex >= 0
                    && rowIndex < grid.NumNodesX
                    && colIndex < grid.NumNodesY
                    && grid.Nodes[rowIndex, colIndex].isWalkable
                    && !(rowIndex == node.rowIndex && colIndex == node.colIndex)
                    && !IsInClosedNodes(grid.Nodes[rowIndex, colIndex]))
                {
                    retNodes[retNodeCount++] = grid.Nodes[rowIndex, colIndex];



                }
                else if (rowIndex != node.rowIndex || colIndex != node.colIndex)
                {

                    retNodes[retNodeCount++] = null;
                }



                colIndex++;
            }
            colIndex = node.colIndex - 1;
            rowIndex++;
        }



        return retNodes;

    }

    bool IsInClosedNodes(Node node)
    {
        return grid.ClosedNodes.Contains(node);
    }


    public Node GetNodeFromWorldPostion(Vector3 position)
    {
        Vector3 position0 = grid.Nodes[0, 0].position - new Vector3(grid.NodeDiameter / 2, 0, grid.NodeDiameter / 2);
        Vector3 deltaVec = position - position0;
        if (deltaVec.x < 0 || deltaVec.z < 0)
        {
            Debug.LogError("World position is not inside the grid. Try increasing gridsize");
            return new Node(new Vector3(-100000, -1000000, -1000000), -1, -1);
        }
        else
        {
            int rowOffset = (int)Mathf.Floor(deltaVec.x / grid.NodeDiameter);
            int colOffset = (int)Mathf.Floor(deltaVec.z / grid.NodeDiameter);
            if (rowOffset > grid.NumNodesX-1 || colOffset > grid.NumNodesY-1)
            {
                Debug.LogError("World position is not inside the grid. Try increasing gridsize");
                return new Node(new Vector3(-100000, -1000000, -1000000), -1, -1);
            }
            else
            {
                //Debug.Log(rowOffset + " " + colOffset);
                return grid.Nodes[rowOffset, colOffset];
            }
        }
    }

    
    
    void FindPath(Node n)
    {
        if (n == endNode)
        {
            TracePath();
            return;

            // yield return null;
        }
        if (!endNode.isWalkable)
        {
            return;
        }



        bool found = CalculateCost(n);
        if (!found)
        {
            return;
        }
        /*   float lowestNetCost = float.MaxValue;
           float lowestHCost = float.MaxValue;
           Node bestNode = null;
           foreach (Node node in openNodes)
           {

               if (node.NetCost < lowestNetCost)
               {
                   lowestNetCost = node.NetCost;
                   lowestHCost = node.hCost;
                   bestNode = node;
               }
               else if (node.NetCost == lowestNetCost)
               {
                   if (node.hCost < lowestHCost)
                   {
                       bestNode = node;
                       lowestHCost = node.hCost;
                   }
               }
           }

        

           openNodes.Remove(bestNode);
           closedNodes.Add(bestNode);*/
        Node node = grid.OpenNodes.Remove();
        grid.ClosedNodes.Add(node);
        FindPath(node);





        // yield return null;
    }




    bool CalculateCost(Node node)
    {
        
        if (endNode.rowIndex < 0)
        {
            return false;
        }

        Node[] adjNodes = getAdjacentNodes(node);

        for (int i = 0; i < adjNodes.Length; i++)
        {
            Node n = adjNodes[i];

            if (n != null)
            {

                bool valAlreadyCalculated = grid.OpenNodes.Contains(n);
                if (Mathf.Abs(node.rowIndex - n.rowIndex) == 1
                    && Mathf.Abs(node.colIndex - n.colIndex) == 1) //if diagonal node
                {

                    float val = node.gCost + Node.DIAGONAL_COST;
                    if (valAlreadyCalculated)
                    {
                        if (val < n.gCost)
                        {
                            //   if(n.NetCost > val + n.hCost)
                            {
                                n.prevNode = node;
                            }
                            n.gCost = val;
                            grid.OpenNodes.Update(n);

                        }
                    }
                    else
                    {
                        n.gCost = val;
                        n.prevNode = node;
                    }

                }
                else
                {
                    float val = node.gCost + Node.ORTHOGONAL_COST;
                    if (valAlreadyCalculated)
                    {
                        if (val < n.gCost)
                        {
                            //    if (n.NetCost > val + n.hCost)
                            {
                                n.prevNode = node;
                            }
                            n.gCost = val;
                            grid.OpenNodes.Update(n);
                        }
                    }
                    else
                    {
                        n.gCost = val;
                        n.prevNode = node;

                    }


                }


                if (!valAlreadyCalculated)
                {

                    float minOffset = Mathf.Abs(Mathf.Min(endNode.rowIndex - n.rowIndex,
                           endNode.colIndex - n.colIndex));
                    n.hCost = minOffset * Node.DIAGONAL_COST;
                    n.hCost += (Mathf.Abs(Mathf.Max(endNode.rowIndex - n.rowIndex
                        , endNode.colIndex - n.colIndex)) - minOffset)
                        * Node.ORTHOGONAL_COST;
                    grid.OpenNodes.Add(n);
                }




            }
        }
        return true;
    }

    void TracePath()
    {

        Node currentNode = endNode;


        float netCostSum = 0;
        while (currentNode != startNode)
        {

            netCostSum += currentNode.NetCost;
            path.Add(currentNode);
            currentNode = currentNode.prevNode;
        }
        //    Debug.Log(netCostSum);

        /*    Node bestNode = null;
            float bestCost = float.MaxValue;
            foreach(Node n in closedNodes)
            {
                if(n.NetCost < bestCost)
                {
                    bestNode = n;
                    bestCost = n.NetCost;
                }
            }
            */
    }


}
