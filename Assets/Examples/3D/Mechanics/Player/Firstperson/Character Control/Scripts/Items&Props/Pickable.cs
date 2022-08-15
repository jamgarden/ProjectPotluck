using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    [Tooltip("How far object will be holded in front of player")]
   public float holdDistance;

    public float throwForce;

    [HideInInspector]
    public bool isThrown;

    [HideInInspector]
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
}
