using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebirth : MonoBehaviour
{
    [Tooltip("1 = 1min")]
    [Range(1,10)]
    public int duration = 10;
    private float currentTime;

    private MeshRenderer meshRender;
    private BoxCollider bc;

    private void Start()
    {
        meshRender = GetComponent<MeshRenderer>();
        bc = GetComponent<BoxCollider>();
        readyToPick = true;
    }

    private bool readyToPick;

    public void Pick()
    {
        meshRender.enabled = false;
        bc.enabled = false;
        currentTime = 60 * duration;
        readyToPick = false;
    }
    private void Update()
    {
        if(!readyToPick)
        {
            currentTime -= Time.deltaTime;
        }

        if(currentTime <= 0 && !readyToPick)
        {
            meshRender.enabled = true;
            bc.enabled = true;
            readyToPick = true;
        }
    }
}
