using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=BSybcKPQCnc

public class SimpleCarControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 50;
    [SerializeField] private float maxSpeed = 15;
    [SerializeField] private float drag = 0.98f;
    [SerializeField] float steerAngle = 20;
    [SerializeField] float traction = 1;

    private Vector3 moveForce;

    void Update()
    {
        Move();
        Steer();
        Drag();
        Traction();
    }

    void Move()
    {
        moveForce += transform.forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += moveForce * Time.deltaTime;
    }

    void Steer()
    {
        float steerInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerInput * moveForce.magnitude * steerAngle * Time.deltaTime);
    }

    void Drag()
    {
        moveForce *= drag;
        moveForce = Vector3.ClampMagnitude(moveForce, maxSpeed);
    }

    void Traction()
    {
        Debug.DrawRay(transform.position, moveForce.normalized * 3);
        Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
        moveForce = Vector3.Lerp(moveForce.normalized, transform.forward, traction * Time.deltaTime) * moveForce.magnitude;
    }
}
