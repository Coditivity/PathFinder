using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GameScript : MonoBehaviour {

    public LayerMask unwalkableLayerMask;
    public Transform characterTransform;
    public MovementHandler characterMovementHandler;
    List<Node> path;
    Grid grid;
    public GameObject nodePrefab;
    private float colliderRadius = .1f;

    
	// Use this for initialization
	void Start () {

        path = new List<Node>();
        grid = new Grid(Vector2.zero, 20, 20, .75f, unwalkableLayerMask, true, nodePrefab);
       
        // Debug.Log("start " + MyGameScripts.NormalizeAngleRad(-13.043828f));
        Debug.Log("dfdf" + null + ">>>>");
	}

    Vector3 target;
    bool calcTurn = true;
    PathFindJob job = null;
    // Update is called once per frame
    void Update () {
        
        float turnAmount = 0;
        
        if (Input.GetMouseButtonDown(0))
        {

            calcTurn = true;
            
            Ray targetRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(targetRay, out hit);
            path.Clear();
            
            job = new PathFindJob(grid, characterTransform.position
                , hit.point, path,false, colliderRadius);
           
            PathManager.AddForPathFinding(job);
           
            PathManager.StartPathFinding();
            //characterTransform.position += path[path.Count - 1].position.normalized;
            
           
        }
      //  if (path.Count > 0)
        {

            //  MovementHandler.HandleMovement(characterTransform
            //     , PathFinder.instance.endNode.position, characterAnimator);

            characterMovementHandler.RunThroughPath(path, colliderRadius, job);
           
            /*characterAnimator.SetFloat("ForwardSpeed", moveAmount, .1f, Time.deltaTime);
            characterAnimator.SetFloat("Turn"//, 0, .1f, Time.deltaTime)
                , turnAmount);//, .1f, Time.deltaTime);*/
        }
          
    
}

#if UNITY_EDITOR
    void OnGUI()
    {
      /*  UnityEditor.Handles.color = Color.black;
        for (int i = 0; i < grid.NumNodesX; i++)
        {
            for (int j = 0; j < grid.NumNodesY; j++)
            {
                
                Vector3 position = new Vector3(-3, 1, 1);
                UnityEditor.Handles.Label(grid.Nodes[i, j].position, ((int)(grid.Nodes[i, j].gCost/1f)).ToString());
            }
        }*/
    }
#endif

}
