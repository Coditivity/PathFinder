using UnityEngine;
using System.Collections;


using System;
using System.Collections.Generic;

public class MovementHandler : MonoBehaviour
{


    Animator animator;
    bool bTurnedToNode1;
    void Start()
    {
        animator = GetComponent<Animator>();
        bTurnedToNode1 = false;
        //colliderRadius = GetComponent<CapsuleCollider>().radius;
    }

    bool bTurnedTowardsActualTargetNode = false; //whether or not completed turning towards the final node, in case it was not reachable
    public bool HandleMovement(Vector3 targetPosMove, PathFindJob job) 
    {
        
        Vector3 tempVec = targetPosMove;
        tempVec.y = transform.position.y; //get the targetmove position with the same y offset at the player
        if ((tempVec - transform.position).magnitude != 0) {
            Vector3 turnDir = (tempVec - transform.position).normalized;
            Quaternion rotationNeeded = Quaternion.LookRotation(turnDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationNeeded, 500 * Time.deltaTime);
            if (Vector3.Dot(transform.forward, turnDir) > .6f)
            {
                bTurnedToNode1 = true;
            }
            else
            {
                bTurnedToNode1 = false;
            }

            if (bTurnedToNode1)
            {
                Vector3 tVec = transform.position;
                MyGameScripts.MoveVector(ref tVec, tempVec, 2.5f, Time.deltaTime);
                transform.position = tVec;
                animator.SetFloat("ForwardSpeed", .8f, .1f, Time.deltaTime);
                animator.SetFloat("Turn", 0, .1f, Time.deltaTime);

            }
            else
            {
                float rotAngle = Vector3.Angle(transform.forward, turnDir);
                Vector3 crossVec = Vector3.Cross(transform.forward, turnDir);
                int sign = crossVec.y < 0 ? -1 : 1;
                animator.SetFloat("ForwardSpeed", 0, .1f, Time.deltaTime);
                animator.SetFloat("Turn", 1f * sign, .1f, Time.deltaTime);
            }
        }

        if (!job.bEndNodeReachable)
        {
            Vector3 endNodeBestTransfY = job.endNodeBest.position;
            endNodeBestTransfY.y = transform.position.y;
            if ((transform.position - endNodeBestTransfY).magnitude == 0) //final position reached
            {

                bTurnedToNode1 = false;
                animator.SetFloat("ForwardSpeed", 0, .1f, Time.deltaTime);
                

                Vector3 tempVec2 = job.endNode.position;
                    tempVec2.y = transform.position.y; //get the targetmove position with the same y offset at the player
                    Vector3 turnDir2 = (tempVec2 - transform.position).normalized;
                    Quaternion rotationNeeded2 = Quaternion.LookRotation(turnDir2);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationNeeded2, 300 * Time.deltaTime);

                float rotAngle = Vector3.Angle(transform.forward, turnDir2);
                Vector3 crossVec = Vector3.Cross(transform.forward, turnDir2);
                int sign = crossVec.y < 0 ? -1 : 1;
                animator.SetFloat("Turn", 1f * sign, .1f, Time.deltaTime);
                Debug.LogError("turnign>>" + Vector3.Dot(transform.forward, turnDir2));
                if (Vector3.Dot(transform.forward, turnDir2) > .95f)
                {
                    animator.SetFloat("Turn", 0, 0f, Time.deltaTime);
                    Debug.LogError("returning");
                        
                    return true;
                }

            }
            
           
        }
        else {

            if ((tempVec - transform.position).magnitude == 0) //targetposmove reached
            {
                
                    bTurnedToNode1 = false;
                    animator.SetFloat("ForwardSpeed", 0, .1f, Time.deltaTime);
                    animator.SetFloat("Turn", 0, .1f, Time.deltaTime);
                    return false;
                
                                
            }
        }

        return false;
       
    }

    public void RunThroughPath(List<Node> path, float colliderRadius, PathFindJob job)
    {

       

        if (path.Count == 0)
        {
            animator.SetFloat("ForwardSpeed", 0, .1f, Time.deltaTime);
            return;
        }
        Vector3 rayDir = (path[0].position - transform.position).normalized;
        RaycastHit hitInfo;
        float rayDistance = (path[0].position - transform.position).magnitude;
        if (!Physics.SphereCast(transform.position, colliderRadius, rayDir, out hitInfo, rayDistance)) //if there are no obstacles in the straight path from character pos to destination
        {
          //  Debug.LogError("no collision");
            Vector3 movePos = path[0].position;
            bool nextNodeReached = HandleMovement(movePos, job);
            if (job.bEndNodeReachable || path.Count > 1)
            {
                Vector3 groundPos = Vector3.ProjectOnPlane(transform.position, Vector3.up);
                if ((groundPos - path[0].position).magnitude == 0.0000f)
                {
                    if (job.bEndNodeReachable)
                    {
                        path.Clear();// RemoveAt(path.Count - 1); 
                    }
                    else
                    {
                        path.RemoveRange(1, path.Count - 1);
                    }
                }
            }
            else
            {
                if (nextNodeReached)
                {
                   
                    path.RemoveAt(path.Count - 1);
                }
            }
        }
        else {
            Vector3 movePos = path[path.Count - 1].position;
            Vector3 turnPos = movePos;
            
            
            bool nextNodeReached = HandleMovement(movePos, job);
            if (job.bEndNodeReachable || path.Count > 1) {
                Vector3 groundPos = Vector3.ProjectOnPlane(transform.position, Vector3.up);
                if ((groundPos - path[path.Count - 1].position).magnitude == 0.0000f)
                {
                    Debug.LogError("removing at");
                    path.RemoveAt(path.Count - 1);
                }
            }
            else
            {
                if (nextNodeReached)
                   
                {
                   
                    path.RemoveAt(path.Count - 1);
                }
            }
            

        }
       if(path.Count == 0 )
        {
            Debug.LogError("path zero");
        }

    }


}
