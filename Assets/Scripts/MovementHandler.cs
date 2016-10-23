using UnityEngine;
using System.Collections;


using System;
using System.Collections.Generic;

public class MovementHandler : MonoBehaviour
{


    Animator animator;
    bool bTurnedToNode1;
    float colliderRadius = .1f;
    void Start()
    {
        animator = GetComponent<Animator>();
        bTurnedToNode1 = false;
        //colliderRadius = GetComponent<CapsuleCollider>().radius;
    }

    
    public void HandleMovement(Vector3 targetPosMove
        , Vector3 targetPosTu) //targetPosTurn
    {
        
        Vector3 tempVec = targetPosMove;
        tempVec.y = transform.position.y; //get the targetmove position with the same y offset at the player
        Vector3 turnDir = (tempVec - transform.position).normalized;
        Quaternion rotationNeeded = Quaternion.LookRotation(turnDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationNeeded, 500 * Time.deltaTime);
        if (Vector3.Dot(transform.forward, turnDir) > .5f)
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

      

        if ((tempVec - transform.position).magnitude == 0)
        {

            bTurnedToNode1 = false;
            animator.SetFloat("ForwardSpeed", 0, .1f, Time.deltaTime);
            animator.SetFloat("Turn", 0, .1f, Time.deltaTime);
            return;
        }
    }

    public void RunThroughPath(List<Node> path)
    {


        if (path.Count == 0)
        {
            animator.SetFloat("ForwardSpeed", 0, .1f, Time.deltaTime);
            return;
        }
        Vector3 rayDir = (path[0].position - transform.position).normalized;
        RaycastHit hitInfo;
        float rayDistance = (path[0].position - transform.position).magnitude;
        if (!Physics.SphereCast(transform.position, colliderRadius, rayDir, out hitInfo)) //if there are no obstacles in the straight path from character pos to destination
        {
          //  Debug.LogError("no collision");
            Vector3 movePos = path[0].position;
            Vector3 turnPos = movePos;
            HandleMovement(movePos, turnPos);
            Vector3 groundPos = Vector3.ProjectOnPlane(transform.position, Vector3.up);
            if ((groundPos - path[0].position).magnitude == 0.0000f)
            {
                path.Clear();// RemoveAt(path.Count - 1); 
            }
        }
        else {
            Vector3 movePos = path[path.Count - 1].position;
            Vector3 turnPos = movePos;
            if (path.Count >= 2)
            {
                turnPos = path[path.Count - 2].position;
            }
            HandleMovement(movePos, turnPos);
            Vector3 groundPos = Vector3.ProjectOnPlane(transform.position, Vector3.up);
            if ((groundPos - path[path.Count - 1].position).magnitude == 0.0000f)
            {
                path.RemoveAt(path.Count - 1);
            }

        }

    }


}
