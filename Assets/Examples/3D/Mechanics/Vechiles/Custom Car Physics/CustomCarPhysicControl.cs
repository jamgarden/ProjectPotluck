using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CustomCarPhysicControl : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject reverseText;

    private Rigidbody rb;
    [SerializeField] Transform []tires;

    [Header("Suspension")]
    [SerializeField] private float offset = 0.4f;
    [SerializeField] private float springForce = 100;
    [SerializeField] private float dampingForce = 10;

    [Header("Accleration")]
    [SerializeField] private AnimationCurve powerCurve;
    [Space(5)]
    [SerializeField] private AnimationCurve brakesCurve;
 
    private float accelerationInput;
    private float currentValue, wantedValue;
    private float brakeValue;

    [Header("Steering")]
    [SerializeField] private float tireMass = 2;
    [SerializeField] [Range(0, 1)] private float gripFactor = 0.4f;

    private float maxTurnTire = 45;
    private float currentTurn; private float t = 0.0f;

    private bool backing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        slider.maxValue = 1;
    }

    private void Update()
    {
        Inputs();
        WheelDisplay();
    }

    void Inputs()
    {
        //Accelerate / Reverse
        if (!backing && Input.GetAxis("Vertical") > 0)
        {
            wantedValue += 0.5f * Time.deltaTime;
        }

        else if (!backing && Input.GetAxis("Vertical") < 0)
        {
            wantedValue -= 0.5f * Time.deltaTime;
        }

        else if (backing && Input.GetAxis("Vertical") < 0)
        {
            wantedValue += 0.5f * Time.deltaTime;
        }

                else if (backing && Input.GetAxis("Vertical") > 0)
        {
            wantedValue -= 0.5f * Time.deltaTime;
        }

        //Turning
        currentTurn += Input.GetAxis("Horizontal") * 0.5f;

        if (Input.GetAxis("Horizontal") == 0)
        {
            if (currentTurn > 0)
            {
                currentTurn -= 0.5f;
            }

            else if (currentTurn < 0)
            {
                currentTurn += 0.5f;
            }
        }

        currentTurn = Mathf.Clamp(currentTurn, -maxTurnTire, maxTurnTire);


       // wantedValue += Input.GetAxis("Vertical") * Time.deltaTime / 2;
        wantedValue = Mathf.Clamp(wantedValue, 0, 1);

        if(currentValue < wantedValue)
        {
            currentValue += 0.25f * Time.deltaTime;
        }

        else if(currentValue > wantedValue)
        {
            currentValue += -0.25f * Time.deltaTime;
        }

        currentValue = Mathf.Clamp(currentValue, 0, 1);
        slider.value = wantedValue;

        //Brakes
        if(Input.GetKeyDown(KeyCode.Space) && brakeValue != 0)
        {
            brakeValue = 0;
        }

        if(Input.GetKey(KeyCode.Space))
        {
            brakeValue += 0.5f * Time.deltaTime;
            brakeValue = Mathf.Clamp(brakeValue, 0, 1);
        }

        if(wantedValue == 0 && Input.GetAxis("Vertical") < 0 && !backing)
        {
            backing = true;
            reverseText.SetActive(true);
        }

        else if (wantedValue == 0 && Input.GetAxis("Vertical") > 0 && backing)
        {
            backing = false;
            reverseText.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        Suspension();
        Acceleration();
        Steering();

        if (Input.GetKey(KeyCode.Space))
        {
            Brakes();
        }
    }

    void Acceleration()
    {
        RaycastHit hit;

        float carSpeed = 0;

        for (int i = 0; i < tires.Length; i++)
        {
            if (Physics.Raycast(tires[i].position, -tires[i].transform.up, out hit, offset))
            {
                carSpeed = Vector3.Dot(transform.forward, rb.velocity);

                Vector3 accelerationDirection = tires[i].forward;
                float availableTorque = powerCurve.Evaluate(currentValue);

                if (!backing)
                {
                    rb.AddForceAtPosition(accelerationDirection * availableTorque, tires[i].position);
                }

                else
                {
                    rb.AddForceAtPosition(-accelerationDirection * availableTorque / 2, tires[i].position);
                }
                Debug.DrawLine(tires[i].position, tires[i].position + accelerationDirection * availableTorque, Color.red);
            }
        }
    }

    void Suspension()
    {
        RaycastHit hit;

        for (int i = 0; i < tires.Length; i++)
        {
            if(Physics.Raycast(tires[i].position, -tires[i].transform.up, out hit, offset))
            {
                //world space direction of the spring force.
                Vector3 springDirection = tires[i].up;

                //wold space velocity of this tire.
                Vector3 tireWorldVelocity = rb.GetPointVelocity(tires[i].position);

                //calculate offset from the raycast ( suspensionRestDist - tireRay.distance )
                float raycastOffset = offset - hit.distance;

                //calculate velocity along the spring direction
                //note that springDir is a unit Vector, so this returns the magnitude of tireWorldVelocity
                float springVelocity = Vector3.Dot(springDirection, tireWorldVelocity);

                //calculate the magnitude of the dampened spring force.
                float force = (raycastOffset * springForce) - (springVelocity * dampingForce);

                //apply the force at the location of this tire, in the direction of suspension
                rb.AddForceAtPosition(springDirection * force, tires[i].position);
                Debug.DrawLine(tires[i].position, tires[i].position + springDirection * force * 0.1f , Color.blue);
            }
        }
    }

    void Brakes()
    {
        RaycastHit hit;


            float carSpeed = 0;

        if(!backing)
        {
            carSpeed = Vector3.Dot(transform.forward, rb.velocity);
        }

        else
        {
          carSpeed = Vector3.Dot(-transform.forward, rb.velocity);
        }
 

        for (int i = 0; i < tires.Length; i++)
        {

         if (Physics.Raycast(tires[i].position, -tires[i].transform.up, out hit, offset))
         {
          if (carSpeed > 0)
          {

                        wantedValue -= Time.deltaTime / 2;

                    Vector3 accelerationDirection = tires[i].forward;
                    
                    if (!backing)
                    {
                        rb.AddForceAtPosition(-accelerationDirection * brakesCurve.Evaluate(brakeValue), tires[i].position);
                    }

                    else
                    {
                        rb.AddForceAtPosition(accelerationDirection * brakesCurve.Evaluate(brakeValue), tires[i].position);
                    }
          }
         }
        }

    }

    void WheelDisplay()
    {
        if (tires.Length > 0)
        {
            tires[0].transform.localRotation = Quaternion.Euler(0f, currentTurn, 0f);
            tires[1].transform.localRotation = Quaternion.Euler(0f, currentTurn, 0f);
        }
    }

    void Steering()
    {
        RaycastHit hit;

        for (int i = 0; i < tires.Length; i++)
        {
            if (Physics.Raycast(tires[i].position, -tires[i].transform.up, out hit, offset))
            {
                Vector3 steeringDirection = tires[i].right;
                Vector3 tireWorldVelocity = rb.GetPointVelocity(tires[i].position);
                float steeringVelocity = Vector3.Dot(steeringDirection, tireWorldVelocity);
                float desiredVelocityChange = -steeringVelocity * gripFactor;
                float desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime;
                rb.AddForceAtPosition(steeringDirection * tireMass * desiredAcceleration, tires[i].position);
            }
        }
    }

                private void OnDrawGizmos()
    {
        if (tires.Length > 0)
        {
            for (int i = 0; i < tires.Length; i++)
            {
                Gizmos.DrawRay(tires[i].position, Vector3.down * offset);
            }
        }
    }
}
