using UnityEngine;
using System.Collections;

public class TestRotationScript : MonoBehaviour {

    public GameObject targetObject;
	// Use this for initialization
	void Start () {

        

	}
	
	// Update is called once per frame
	void Update () {
        Vector3 dir = (targetObject.transform.position - transform.position).normalized;
        Quaternion rotQuat = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotQuat, 500 * Time.deltaTime);
    }
}
