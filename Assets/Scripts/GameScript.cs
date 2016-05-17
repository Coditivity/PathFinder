using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class GameScript : MonoBehaviour {

    public LayerMask unwalkableLayerMask;
    public Transform characterTransform;
    public MovementHandler characterMovementHandler;
    List<Node> path;
    Grid grid;
    public GameObject nodePrefab;

    
	// Use this for initialization
	void Start () {

        path = new List<Node>();
        grid = new Grid(Vector2.zero, 20, 20, .5f, unwalkableLayerMask, false, nodePrefab);
        Debug.Log("start " + MyGameScripts.NormalizeAngleRad(-13.043828f));

	}

    Vector3 target;
    bool calcTurn = true;
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
            PathManager.AddForPathFinding(grid, characterTransform.position, hit.point, path, true);
            PathManager.StartPathFinding();
            //characterTransform.position += path[path.Count - 1].position.normalized;
            
           
        }
      //  if (path.Count > 0)
        {

            //  MovementHandler.HandleMovement(characterTransform
            //     , PathFinder.instance.endNode.position, characterAnimator);

            characterMovementHandler.RunThroughPath(path);
           
            /*characterAnimator.SetFloat("ForwardSpeed", moveAmount, .1f, Time.deltaTime);
            characterAnimator.SetFloat("Turn"//, 0, .1f, Time.deltaTime)
                , turnAmount);//, .1f, Time.deltaTime);*/
        }
	}

   
        
}
