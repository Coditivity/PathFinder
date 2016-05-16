using UnityEngine;
using System.Collections;



public class MyGameScripts
{
    static Vector3 t = Vector3.zero;
    public const float PI2 = Mathf.PI * 2;

    //returns a rand variable whose value is around val. 
    //Returned value is in between (val+surroundwidth, val-surroundwidth) 
    //or (val, val+survalw) or(val, val-survalw) depending on dir
    //if sorroundvalwidth is zero returned value will be same as val. 
    //Farther a value is from val, the less likely that it is the returned value
    public static float random(float val, float sorroundValWidth, int dir) //dir = 0 means +-, 1 means +val+surval, -1 means -val-surval 
    {
        int numDecPlaces = getNumDecPlaces(sorroundValWidth, 3);
        float sorValWNoDec = sorroundValWidth 
            * Mathf.Pow(10, numDecPlaces);
        float sum = (int)((sorValWNoDec) * (sorValWNoDec + 1+1) / 2); // sum of n natural numbers
        float rand = Random.Range(0, sum);
       
        float dif = sum - rand + 1;
        float D = Mathf.Ceil(Mathf.Sqrt(1 + 4 * 2 * dif));
        int n = (int)(Mathf.Ceil((-1 + D) / 2f));
        float c = sorValWNoDec + 1 - n ;

        val *= Mathf.Pow(10, numDecPlaces);

        if (dir == 0)
        {
            return (val + c * randomSign()) / Mathf.Pow(10, numDecPlaces);
        }
        else
        {
            return (val + c * Mathf.Sign(dir)) / Mathf.Pow(10, numDecPlaces);
        }

    }

    public static int randomSign()
    {
        return (Random.value < .5 ? -1 : 1);
    }
    public static float getAngleSumDegree(float angle1, float angle2)
    {
        angle1 += angle2;
        while (angle1 >= 360)
        {
            angle1 -= 360;
        }
        while (angle1 < 0)
        {
            angle1 += 360;

        }
        return angle1;
    }
    public static float getAngleSumRad(float angle1, float angle2)
    {
        angle1 += angle2;
        while (angle1 >= PI2)
        {
            angle1 -= PI2;
        }
        while (angle1 < 0)
        {
            angle1 += PI2;

        }
        return angle1;
    }
    public static float SignedAngleZ(Vector3 dirVecA, Vector3 dirVecb)
    {
        //Debug.Log(Vector3.Cross(a, b));
        float angle = Vector3.Angle(dirVecA, dirVecb); // calculate angle
        // assume the sign of the cross product's Y component:
        if (Mathf.Sign(Vector3.Cross(dirVecA, dirVecb).z) < 0)
        {
            //  return 180 + (180-angle);
            angle = angle * -1 + 360;
        }

        return 360 - angle;
    }

    /*public static float canvasToScreen(RectTransform canvRectTransfrom, float canvasCoordX )
    {
        float canvasToScreenXConv = Screen.width / canvRectTransfrom.rect.width;
        return canvasCoordX * canvasToScreenXConv;
    }*/

  /*  public static float addWorldX_CanvasX2Screen(Vector3 worldPos, float canvasCoordX, Camera cam
        , float canvasToScreenConvX) //add worldspace x with canvas space x and then conv the result to screen space
    {
        return cam.WorldToScreenPoint(worldPos).x + canvasCoordX * canvasToScreenConvX;
    }*/

    public static float addWorldX_CanvasX2Screen(Vector3 worldPos, float canvasCoordX, Camera cam
        , RectTransform canvasRectTransform) //add worldspace x with canvas space x and then conv the result to screen space
    {
        float canvasToScreenConvX = 1;
        canvasToScreenConvX = Screen.width / canvasRectTransform.rect.width;
       
        return cam.WorldToScreenPoint(worldPos).x + canvasCoordX * canvasToScreenConvX;
        
    }

    

    public static float addWorldY_CanvasY2Screen(Vector3 worldPos, float canvasCoordY, Camera cam
        , RectTransform canvasRectTransform) //add worldspace x with canvas space x and then conv the result to screen space
    {
        float canvasToScreenConvY = 1;
        canvasToScreenConvY = Screen.height / canvasRectTransform.rect.height;
        return cam.WorldToScreenPoint(worldPos).y + canvasCoordY * canvasToScreenConvY;

    }

    static Vector3 screenCoords = Vector3.zero;
    static Vector3 retVecGetURMIW = Vector3.zero;
    public static Vector3 getUIRectSizeInWorld(Vector3 worldPos, float uiRectWidth, float uiRectHeight, RectTransform canvasRectTransform, Camera cam )
    {
        float maxPXScreen = MyGameScripts.addWorldX_CanvasX2Screen(worldPos
            , uiRectWidth/2
            , cam
            , canvasRectTransform);
        float maxPYScreen = MyGameScripts.addWorldY_CanvasY2Screen(worldPos
            , uiRectHeight/2, cam
            , canvasRectTransform);
        screenCoords.Set(maxPXScreen, maxPYScreen, 1);
        retVecGetURMIW = cam.ScreenToWorldPoint(screenCoords);
        return retVecGetURMIW - worldPos;

    }

    public static void accelerateValue(ref float value, float by, float limit)
    {
        value += by * Time.deltaTime;
        if (value > limit)
        {
            value = limit;
        }

    }

    //accelerates a velocity vector at the rate of 'accel' till its magnitude reaches 'velMagLimit'
    public static void accelerateVec(ref Vector3 vecToBeAcceled, Vector3 accel, float vecMagLimit)
    {
        vecToBeAcceled += accel * Time.deltaTime; //increment the velocity vector by accel
        float vecMag = vecToBeAcceled.magnitude; //get the magnitude of the velocity vector

        if (vecMag > vecMagLimit) //if velocity magnitude greater than its limit
        {

            vecMag = vecMagLimit; //reset the vector velocity to its limit
            vecToBeAcceled = vecToBeAcceled.normalized * vecMag; //apply the new magnitude to the velocity vector
           
        }

        
    }

    //Damps a given vector at a rate of dampMag
    public static void dampVector(ref Vector3 vectorToBeDamped, float dampMag)
    {
        float vecMag = vectorToBeDamped.magnitude; //get the vector magnitude
        vecMag -= dampMag * Time.deltaTime; //damp the magnitude by dampMag * dt
        if (vecMag < 0) //if dampMag went below zero
        {
            vecMag = 0; //set dampMag to zero
        }
        vectorToBeDamped = vectorToBeDamped.normalized * vecMag; //apply the new damped magnitude to the vector
    }

    public static float SignedAngleX(Vector3 dirVecA, Vector3 dirVecb)
    {
        //Debug.Log(Vector3.Cross(a, b));
        float angle = Vector3.Angle(dirVecA, dirVecb); // calculate angle
        // assume the sign of the cross product's Y component:
        if (Mathf.Sign(Vector3.Cross(dirVecA, dirVecb).x) < 0)
        {
            //  return 180 + (180-angle);
            angle = angle * -1 + 360;
        }

        return 360 - angle;
    }
    public static float SignedAngleY(Vector3 dirVecA, Vector3 dirVecb)
    {
        //Debug.Log(Vector3.Cross(a, b));
        float angle = Vector3.Angle(dirVecA, dirVecb); // calculate angle
        // assume the sign of the cross product's Y component:
        if (Mathf.Sign(Vector3.Cross(dirVecA, dirVecb).y) < 0)
        {
            //  return 180 + (180-angle);
            angle = angle * -1 + 360;
        }

        return 360 - angle;
    }

    public static float getAngleZ(Vector3 fromObjPos, Vector3 toObjPos)
    {
        return SignedAngleZ(toObjPos - fromObjPos,
                Vector3.right);


    }


    public static float getObjRotationZ(Transform objTransform)
    {
        return SignedAngleZ(objTransform.right, Vector3.right); 
    }
    public static float getObjRotationX(Transform objTransform)
    {
        return SignedAngleX(objTransform.forward, Vector3.forward);
    }
    public static float getObjRotationY(Transform objTransform)
    {
        return SignedAngleY(objTransform.right, Vector3.right);
    }

    static Vector3 tv2;

    public static void getNonRotatedPointFromCenter(GameObject sourceObj, GameObject targetobject, float distance, float angleDeg)
    {  //point needs to allocated by caller


        tv2.x = sourceObj.transform.position.x + distance * Mathf.Cos(angleDeg * Mathf.Deg2Rad);
        tv2.y = sourceObj.transform.position.y + distance * Mathf.Sin(angleDeg * Mathf.Deg2Rad);
        tv2.z = targetobject.transform.position.z;

        targetobject.transform.position = tv2;
    }

    public static void getNonRotatedPointFromCenter(Transform sourceTransform, Transform targetTransform, float distance, float angleDeg)
    {  //point needs to allocated by caller


        tv2.x = sourceTransform.position.x + distance * Mathf.Cos(angleDeg * Mathf.Deg2Rad);
        tv2.y = sourceTransform.position.y + distance * Mathf.Sin(angleDeg * Mathf.Deg2Rad);
        tv2.z = targetTransform.position.z;

        targetTransform.position = tv2;
    }

    static Vector3 tv3;
    static float sourceRot;
    public static void getRotatedPointFromCenterZ(GameObject sourceObj, GameObject targetobject, float distance, float angleDeg)
    {  //point needs to allocated by caller
        sourceRot = SignedAngleZ(sourceObj.transform.right, Vector3.right);

        tv3.x = sourceObj.transform.position.x + distance * Mathf.Cos(getAngleSumDegree(angleDeg, sourceRot) * Mathf.Deg2Rad);
        tv3.y = sourceObj.transform.position.y + distance * Mathf.Sin(getAngleSumDegree(angleDeg, sourceRot) * Mathf.Deg2Rad);
        tv3.z = targetobject.transform.position.z;

        targetobject.transform.position = tv3;
    }


    static Vector3 GRV2DPVec = Vector3.zero;
    public static Vector3 getRotatedVec2DPlaneZ(Vector3 dirVec, float angDeg)
    {
        sourceRot = SignedAngleZ(dirVec, Vector3.right);
        float tx = dirVec.magnitude * Mathf.Cos(getAngleSumDegree(angDeg, sourceRot) * Mathf.Deg2Rad);
        float ty = dirVec.magnitude * Mathf.Sin(getAngleSumDegree(angDeg, sourceRot) * Mathf.Deg2Rad);
        GRV2DPVec.Set(tx, ty, dirVec.z);
        return GRV2DPVec;
    }

    public static void moveTransform(Transform transform, Vector3 to, Vector3 vel)
    {

        if (Vector3.Dot(vel.normalized
                       , (to - (transform.position + vel)).normalized) <= 0)
        {
            transform.position = to;
        }
        else
        {

            transform.position += vel;
        }
    }

    static Vector3 mtVelVec = Vector3.zero;
    public static void moveTransform(Transform transform, Vector3 to, float speed)
    {
        if (Vector3.Distance(to, transform.position) == 0)
        {
            return;
        }
        mtVelVec = (to - transform.position).normalized * speed * Time.deltaTime ;

        transform.position += mtVelVec;
        if (Vector3.Dot(mtVelVec.normalized
                       , (to - (transform.position)).normalized) <= 0)
        {
            transform.position = to;
        }

    }

    static Vector3 mVVelVec = Vector3.zero;
    public static void MoveVector(ref Vector3 from, Vector3 to, float speed, float deltaTime)
    {
        if (Vector3.Distance(to, from) == 0)
        {
            return;
        }
        mtVelVec = (to - from).normalized * speed * deltaTime;

        from += mtVelVec;
        if (Vector3.Dot(mtVelVec.normalized
                       , (to - (from)).normalized) <= 0)
        {
            from = to;
        }

    }

    public static float incrementValue(float curVal, float toVal, float step)
    {
        if (toVal < curVal)
        {
            curVal -= Mathf.Abs(step);
            if (curVal < toVal)
            {
                curVal = toVal;
            }
        }
        else
        {
            curVal += Mathf.Abs(step);
            if (curVal > toVal)
            {
                curVal = toVal;
            }
        }

        return curVal;
        

    }

    public static int getNumDecPlaces(float num, int precision)
    {
        //decimal d = (decimal)num;
        //int count = System.BitConverter.GetBytes(decimal.GetBits(d)[3])[2];
        //return count;
        //float count = num / float.Epsilon;
       // return count;
        int numDecPlaces = 0;
        decimal oNum = (decimal)num;
        decimal mult = 1;
        decimal tNum = 0;
        
        for (int i = 0; i < precision;i++ )
        {
            tNum = mult * oNum;
            if ((int)tNum == tNum)
            {
                return numDecPlaces;
            }
            mult *= 10;
            numDecPlaces++;
        }

        return numDecPlaces;
    }


    public static bool isInsideBounds(float boundXMin, float boundXMax
        , float boundYMin, float boundYMax
        , float xVal, float yVal)
    {
        
        if (xVal <= boundXMax && xVal >= boundXMin)
        {
            if (yVal <= boundYMax && yVal >= boundYMin)
            {
                return true;
            }
        }
        return false;
             
    }

    public static float rotateAngleToRad(float angRad, float angRadToReach, float rotSpeed, float dt)
    {
        float aDistAClock = getAngularDistAClockRad(angRad, angRadToReach);
        float aDistClock = getAngularDistClockRad(angRad, angRadToReach);
       
        if ( aDistAClock <= aDistClock)
        {
            if (aDistAClock < rotSpeed * dt)
            {
                angRad = angRadToReach;
                //rotationLeft = 0;
                return angRad;
            }
            angRad = getAngleSumRad(angRad, rotSpeed * dt);
            return angRad;
        }

        if (aDistClock < rotSpeed * dt)
        {
            angRad = angRadToReach;
            return angRad;
        }
        angRad = getAngleSumRad(angRad, -rotSpeed * dt);
        return angRad;
    }


    public static float getAngularDistClockRad(float a1Rad, float a2Rad)
    {
      //  Debug.Log("a1r ->" + a1Rad + " a2r->" + a2Rad);

        a1Rad = NormalizeAngleRad(a1Rad);
        a2Rad = NormalizeAngleRad(a2Rad);
     //   Debug.Log("normalized "+"a1r ->" + a1Rad + " a2r->" + a2Rad);
        return ((a1Rad - a2Rad) >= 0 ? (a1Rad - a2Rad) : (a1Rad - a2Rad + Mathf.PI * 2));

    }

    public static float NormalizeAngleRad(float angRad)
    {
        if (angRad < 0)
        {
            return (2 * Mathf.PI + angRad % (2 * Mathf.PI));
        }
        return angRad % Mathf.PI;
    }
    public static float getAngularDistAClockRad(float a1Rad, float a2Rad)
    {
        a1Rad = NormalizeAngleRad(a1Rad);
        a2Rad = NormalizeAngleRad(a2Rad);
        return (a2Rad - a1Rad) >= 0 ? (a2Rad - a1Rad) : (a2Rad - a1Rad + Mathf.PI * 2);
    }



}
