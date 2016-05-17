﻿using UnityEngine;
using System.Collections;


using System;
using System.Collections.Generic;

public class MovementHandler :MonoBehaviour{

   
    Animator animator;
    bool bTurnedToNode1;

    void Start()
    {
        animator = GetComponent<Animator>();
        bTurnedToNode1 = false;
    }
        
   

    public void HandleMovement( Vector3 targetPosMove
        , Vector3 targetPosTu)
    {

        Vector3 tVec = new Vector3(transform.position.x, targetPosMove.y, transform.position.z);
        
        Vector3 dir = targetPosMove - transform.position;       
        dir.Normalize();
        dir = transform.InverseTransformDirection(dir);
        dir = Vector3.ProjectOnPlane(dir, Vector3.up);
        float angle = Mathf.Atan2(dir.x, dir.z);
        float rotateAmount = MyGameScripts.rotateAngleToRad(0, angle, 5, Time.deltaTime);
        if (Mathf.Abs(angle - rotateAmount) <= .00001f)
        {
            bTurnedToNode1 = true;
        }

            transform.Rotate(Vector3.up * rotateAmount*Mathf.Rad2Deg ); // angle/Time.deltaTime);
       // Vector3 dirVec = (targetPosTurn - transform.position).normalized;
        if(bTurnedToNode1)
        {
            //transform.position += dirVec * 2 * Time.deltaTime;
            
            MyGameScripts.MoveVector(ref tVec, targetPosMove, 2.5f, Time.deltaTime);
            tVec.y = transform.position.y;
            transform.position = tVec;
            animator.SetFloat("ForwardSpeed", .8f, .1f, Time.deltaTime);
            animator.SetFloat("Turn", 0, .1f, Time.deltaTime);

        }
        else
        {

            int sign = rotateAmount > Mathf.PI ? -1 : 1;
            animator.SetFloat("ForwardSpeed", 0, .1f, Time.deltaTime);
            animator.SetFloat("Turn", 1f * sign, .1f, Time.deltaTime);
        }
        if ((tVec - targetPosMove).magnitude == 0)
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
        Vector3 movePos = path[path.Count - 1].position;
        Vector3 turnPos = movePos;
        if (path.Count >= 2)
        {
            turnPos = path[path.Count - 2].position;
        }
        HandleMovement(movePos, turnPos);
        Vector3 groundPos = Vector3.ProjectOnPlane(transform.position, Vector3.up);
        if((groundPos - path[path.Count-1].position).magnitude == 0.0000f)
        {           
            path.RemoveAt(path.Count-1);            
        }
        

    }

    
}
