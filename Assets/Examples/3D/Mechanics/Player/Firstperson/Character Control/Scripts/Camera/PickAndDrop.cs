using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAndDrop : MonoBehaviour
{
    private Camera mainCamera;

    public float pickDistance = 2;
    public float smooth = 2.8f;
    private float currentDistance;

    Pickable pickable;
    private Vector3 holdPosition;
    private Rigidbody carried_rb;
    private bool isCarrying;

    private void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isCarrying)
            {
                PickUse();
            }

            else
            {
                Drop(0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && isCarrying && pickable) // Throw
        {
            Drop(pickable.throwForce);
        }
    }

    private void FixedUpdate()
    {
        if(isCarrying && carried_rb)
        {
            Carry(carried_rb);
        }
    }

    void PickUse()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray ray = mainCamera.ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickDistance))
        {

            if (hit.collider.gameObject.GetComponent<Pickable>())
            {
                pickable = hit.transform.GetComponent<Pickable>();
                carried_rb = pickable.rb;
                carried_rb.interpolation = RigidbodyInterpolation.Interpolate;
                carried_rb.angularDrag = 5;
                isCarrying = true;
                return;
            }

             if(hit.collider.gameObject.GetComponent<Rebirth>())
            {
                hit.transform.GetComponent<Rebirth>().Pick();
                return;
            }

            Debug.Log($"[{hit.transform.tag}]"); //Tells tag of object what rays hit.
        }
    }

    void Carry(Rigidbody rb)
    {
        currentDistance = (transform.position - rb.transform.position).sqrMagnitude;

        holdPosition = (mainCamera.transform.position - rb.transform.position) + mainCamera.transform.forward * pickable.holdDistance;
        rb.velocity = holdPosition * Mathf.Sqrt(rb.mass * 9.81f * smooth);

        float angle = Vector3.Angle(Vector3.down, transform.forward);

        //Drop if too far from player or angle is 0 or lower.
        if (currentDistance > (pickable.holdDistance * 2) + 2.5f || angle <= 0)
        {
            Drop(0);
            currentDistance = 0;
        }
    }

    void Drop(float throwForce)
    {
        carried_rb.angularDrag = 0.05f;
        carried_rb.AddForce(mainCamera.transform.forward * throwForce, ForceMode.Impulse);

        if(throwForce > 0)
        {
            pickable.isThrown = true;
        }

        pickable = null;
        carried_rb = null;
        isCarrying = false;
    }
}
