using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide_Door : MonoBehaviour
{
    [SerializeField] private GameObject prefabDoor; //NB! Make sure that pivot is bottom left
    [SerializeField] private bool showGizmo;

    [Space(10)]
    [SerializeField] private float openDistance = 1;
    [SerializeField] private float transitationTime = 1;
    [Tooltip("This is meant for 1 door only")]
    [SerializeField] private bool oppositeDirection;

    [Space(10)]
    [SerializeField] private bool twoDoors;
    [SerializeField] private bool useRigidbody;

    private Transform leftDoor, rightDoor;
    private Rigidbody rbLeft, rbRight;
    public float t;
    private Vector3 startPos, endLeftPos, endRightPos;

    [SerializeField]
    private bool open;
   
    private void Start()
    {
        SetDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            open = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            open = false;
        }
    }

    void SetDoor()
    {
        startPos = transform.position;
        int doorCount = 1;
        if (twoDoors) doorCount = 2;
        

        for(int i = 0; i < doorCount; i++)
        {
            int angleDoor = 0;
            if (oppositeDirection && !twoDoors) angleDoor = 180;
            else angleDoor = i * 180;

            GameObject door = Instantiate(prefabDoor, transform.position, transform.rotation * Quaternion.Euler(0, angleDoor, 0));
            door.transform.parent = this.transform;
            //door.transform.name = door.transform.name + i;

            if (!leftDoor && !rightDoor)
            {
                leftDoor = door.transform;

                if (useRigidbody)
                {
                    door.AddComponent<Rigidbody>();
                    door.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
                    rbLeft = door.GetComponent<Rigidbody>();
                }
            }

            else
            {
                rightDoor = door.transform;

                if (useRigidbody)
                {
                    door.AddComponent<Rigidbody>();
                    door.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
                    rbRight = door.GetComponent<Rigidbody>();
                }
            }
        }

        if (leftDoor)
        {
         if(oppositeDirection && !twoDoors) endLeftPos = transform.position + (-transform.right * openDistance);
         else endLeftPos = transform.position + (transform.right * openDistance);
        }


        if (rightDoor) endRightPos = transform.position + (-transform.right * openDistance);
    }

    private void Update()
    {
        if(!useRigidbody) Transition();
    }

    private void FixedUpdate()
    {
        if(useRigidbody) Transition();
    }

    void Transition()
    {
        if (open && t < 1)
        {
            if (useRigidbody) t += Time.fixedDeltaTime;
            else t += Time.deltaTime;
        }

        if (!open && t > 0)
        {
            if (useRigidbody) t -= Time.fixedDeltaTime;
            else t -= Time.deltaTime; 
        }

        if (useRigidbody)
        {
            if (leftDoor)
            {
                rbLeft.MovePosition(Vector3.Lerp(startPos, endLeftPos, t / transitationTime));
            }

            if (rightDoor)
            {
                rbRight.MovePosition(Vector3.Lerp(startPos, endRightPos, t / transitationTime));
            }
        }

        else
        {
            if(leftDoor)
            {
                leftDoor.position = Vector3.Lerp(startPos, endLeftPos, t / transitationTime);
            }

            if (rightDoor)
            {
                rightDoor.position = Vector3.Lerp(startPos, endRightPos, t / transitationTime);
            }
        }


    }

    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            if (twoDoors)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + (transform.right * openDistance));
                Gizmos.DrawLine(transform.position, transform.position + (-transform.right * openDistance));

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position + transform.right * openDistance, 0.15f);
                Gizmos.DrawWireSphere(transform.position + -transform.right * openDistance, 0.15f);
            }

            else
            {
                if (oppositeDirection)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(transform.position, transform.position + (-transform.right * openDistance));

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(transform.position + -transform.right * openDistance, 0.15f);
                }
                else
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(transform.position, transform.position + (transform.right * openDistance));

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(transform.position + transform.right * openDistance, 0.15f);
                }
            }
        }
    }
}
