using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Grid
{
    public LayerMask unwalkableObjectLayer;
    public Vector3 centerPosition;
    public float gridSizeX;
    public float gridSizeY;
    public float NodeDiameter { get; private set; }
    public Node[,] Nodes { get; private set; }

    public int NumNodesX { get; private set; }
    public int NumNodesY { get; private set; }


    public Heap<Node> OpenNodes { get; private set; }
    public Heap<Node> ClosedNodes { get; private set; }

    Layer_WeightPair[] layerWeights;
    GameObject[] nodeObjects;
    GameObject nodeObjectParent;
    public Grid(Vector3 position, float sizeX, float sizeY, float nodeDiameter
        , LayerMask unwalkableLayerMask, bool drawGrid, GameObject nodePrefab)
    {
        this.centerPosition = position;
        this.gridSizeX = sizeX;
        this.gridSizeY = sizeY;
        this.NodeDiameter = nodeDiameter;
        this.unwalkableObjectLayer = unwalkableLayerMask;

        if (NodeDiameter == 0)
        {
            Debug.LogError("nodeDiameter is zero. Defaulting to 1");
            NodeDiameter = 1;
        }

        NumNodesX = Mathf.RoundToInt(gridSizeX / NodeDiameter);
        NumNodesY = Mathf.RoundToInt(gridSizeY / NodeDiameter);
        Nodes = new Node[NumNodesX, NumNodesY];
        OpenNodes = new Heap<Node>(NumNodesX * NumNodesY);
        ClosedNodes = new Heap<Node>(NumNodesX * NumNodesY);


        Vector3 tNodePos;
        Vector3 nodeSizeX = new Vector3(NodeDiameter, 0, 0);
        Vector3 nodeSizeZ = new Vector3(0, 0, NodeDiameter);
        this.layerWeights = PathManager.instance.layerWeights;

        float prefabExtentsX;
        float scaleFactor = 1;
        if (drawGrid)
        {
            nodeObjects = new GameObject[NumNodesX*NumNodesY];
            prefabExtentsX = nodePrefab.GetComponent<Renderer>().bounds.size.x;
            scaleFactor = (NodeDiameter - NodeDiameter*.2f) / prefabExtentsX;
            nodeObjectParent = new GameObject();
            nodeObjectParent.AddComponent<MeshFilter>();
            nodeObjectParent.AddComponent<MeshRenderer>();
            nodeObjectParent.GetComponent<MeshRenderer>().material
                = nodePrefab.GetComponent<MeshRenderer>().sharedMaterial;
        }

        int index = 0;

        int unwalkcount = 0;
        for (int i = 0; i < NumNodesX; i++)
        {
            for (int j = 0; j < NumNodesY; j++)
            {

                tNodePos = centerPosition - Vector3.forward * gridSizeY / 2 + nodeSizeZ / 2f + j * nodeSizeZ
                    - Vector3.right * gridSizeX / 2 + i * nodeSizeX + nodeSizeX / 2;
                Nodes[i, j] = new Node(tNodePos, i, j);
                                

                if (Physics.CheckSphere(Nodes[i, j].position, NodeDiameter / 2f, unwalkableObjectLayer))

                {
                    Nodes[i, j].isWalkable = false;
                    unwalkcount++;
                }

               
                foreach (Layer_WeightPair l in layerWeights) {
                    if (Physics.CheckSphere(Nodes[i, j].position, NodeDiameter / 2f, l.layerMask))
                    {
                        
                        Nodes[i, j].weight = l.weight;
                    }
                }
                if(drawGrid)
                {
                    
                    nodeObjects[index++] = Object.Instantiate(nodePrefab);
                    nodeObjects[index - 1].transform.position = Nodes[i, j].position;
                    nodeObjects[index - 1].transform.localScale *= scaleFactor;
                    nodeObjects[index - 1].transform.parent = nodeObjectParent.transform;
                }

            }
        }
        Debug.Log("unwalkcount " + unwalkcount);

        MeshFilter[] meshFilters = nodeObjectParent.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        for(int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }
        nodeObjectParent.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        nodeObjectParent.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        nodeObjectParent.transform.gameObject.SetActive(true);

    }

    public void Reset()
    {
        OpenNodes.Clear();
        ClosedNodes.Clear();
    }
    

    public void SetLayerWeights(Layer_WeightPair[] layerWeights)
    {
        this.layerWeights = layerWeights;
    }
    [System.Serializable]
    public struct Layer_WeightPair
    {
        public LayerMask layerMask;
        public float weight;
    }

}
