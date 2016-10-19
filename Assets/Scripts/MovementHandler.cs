using UnityEngine;
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
        , Vector3 targetPosTu) //targetPosTurn
    {
       
        /*  Vector3 tVec = transform.position;

          Vector3 dir = targetPosMove - tVec;// transform.position;       
          dir.Normalize();
          dir = transform.InverseTransformDirection(dir);
          dir = Vector3.ProjectOnPlane(dir, Vector3.up);
          float angle = Mathf.Atan2(dir.x, dir.z);
          float rotateAmount =  MyGameScripts.rotateAngleToRad(0, angle, 10, Time.deltaTime);
          if (Mathf.Abs( rotateAmount - angle) <= .00001f)
          {            
              bTurnedToNode1 = true;
          }

              transform.Rotate(Vector3.up * rotateAmount*Mathf.Rad2Deg ); // angle/Time.deltaTime);*/
        // Vector3 dirVec = (targetPosTurn - transform.position).normalized;

        Vector3 tempVec = targetPosMove;
        tempVec.y = transform.position.y; //get the targetmove position with the same y offset at the player
        Vector3 turnDir = (tempVec - transform.position).normalized;
        Quaternion rotationNeeded = Quaternion.LookRotation(turnDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationNeeded, 200 * Time.deltaTime);
        Debug.LogError("w>>" + Quaternion.LookRotation(turnDir));
        if (Vector3.Dot(transform.forward, turnDir) >= .98f)
        {
            bTurnedToNode1 = true;
        }

        if(bTurnedToNode1)
        {
            //transform.position += dirVec * 2 * Time.deltaTime;
            Vector3 tVec = transform.position;
            MyGameScripts.MoveVector(ref tVec, tempVec, 2.5f, Time.deltaTime);            
            transform.position = tVec;
            animator.SetFloat("ForwardSpeed", .8f, .1f, Time.deltaTime);
            animator.SetFloat("Turn", 0, .1f, Time.deltaTime);

        }
        else
        {

           // int sign = rotateAmount > Mathf.PI ? -1 : 1;
            animator.SetFloat("ForwardSpeed", 0, .1f, Time.deltaTime);
         //   animator.SetFloat("Turn", 1f * sign, .1f, Time.deltaTime);
        }

    //    Debug.LogError((Vector3.ProjectOnPlane(tVec, Vector3.up) - targetPosMove).magnitude + " tvec>>" + tVec + " targetposmove>>" + targetPosMove);

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
