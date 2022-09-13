using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushCoin : MonoBehaviour
{
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        rb.AddForce(Vector3.up * Random.Range(3,8) + transform.forward * 4, ForceMode.Impulse);
    }


}
