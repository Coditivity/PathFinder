using UnityEngine;
using System.Collections;

using Assets.Scripts;
using System;
using System.Collections.Generic;

public class MovementHandler{
   

    public static void HandleMovement(Transform transform, Vector3 targetPos, Animator animator)
    {

        Vector3 tVec = new Vector3(transform.position.x, targetPos.y, transform.position.z);
        if((tVec - targetPos).magnitude == 0)
        {
            animator.SetFloat("ForwardSpeed", 0, .1f, Time.deltaTime);
            animator.SetFloat("Turn", 0, .1f, Time.deltaTime);
            return;
        }
        Vector3 dir = targetPos - transform.position;       
        dir.Normalize();
        dir = transform.InverseTransformDirection(dir);
        dir = Vector3.ProjectOnPlane(dir, Vector3.up);
        float angle = Mathf.Atan2(dir.x, dir.z);
        float rotateAmount = MyGameScripts.rotateAngleToRad(0, angle, 5, Time.deltaTime);
        
       
        transform.Rotate(Vector3.up * rotateAmount*Mathf.Rad2Deg ); // angle/Time.deltaTime);
        Vector3 dirVec = (targetPos - transform.position).normalized;
        if(Mathf.Abs(angle - rotateAmount) <= .00001f)
        {
            //transform.position += dirVec * 2 * Time.deltaTime;
            
            MyGameScripts.MoveVector(ref tVec, targetPos, 2.5f, Time.deltaTime);
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

    }

    public static void RunThroughPath(List<Node> path, Transform transform, Animator animator)
    {
        if (path.Count == 0)
        {
            return;
        }
        HandleMovement(transform, path[path.Count - 1].position, animator);
        Vector3 groundPos = Vector3.ProjectOnPlane(transform.position, Vector3.up);
        if((groundPos - path[path.Count-1].position).magnitude <= 0.0001f)
        {           
            path.RemoveAt(path.Count-1);            
        }
        

    }

    
}
