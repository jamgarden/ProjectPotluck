using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCamera : MonoBehaviour
{
    private Camera mainCamera;
    private Camera cam;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cam = GetComponent<Camera>();
    }

    public void EnableCamera()
    {
        cam.enabled = true;
        mainCamera.enabled = false;
    }

    public void DisableCamera()
    {
        cam.enabled = false;
        mainCamera.enabled = true;
    }

    public void RePoseCamera(Vector3 pos, string name)
    {
        Transform target = GameObject.Find(name).gameObject.transform.Find("LookAt");

        //Change position if diffrent from pos
        if(transform.position != pos)
        {
            transform.position = pos;
        }

        //New target to look at
        if (target != null)
        {
            transform.LookAt(target.position, Vector3.up);
        }
    }
}
