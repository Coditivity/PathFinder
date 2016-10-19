using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public Camera characterCamera;
    Vector3 initialRelativeCamPos;
	// Use this for initialization
	void Start () {

        initialRelativeCamPos = characterCamera.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        
	}

    void LateUpdate()
    {
        characterCamera.transform.position = transform.position + initialRelativeCamPos;
    }
}
