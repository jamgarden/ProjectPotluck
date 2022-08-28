using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPlayer : MonoBehaviour
{
    CharacterController cc;
    [SerializeField] float movingSpeed;
    [SerializeField] GameObject playerCharacter;

    private float inputX, inputZ;
    private Vector3 velocity;
    private Vector3 moveDirection, lockedDirection;

    void Start()
    {
     cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        Move();
        Rotate();
    }

    void Inputs()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");


    }

    void Move()
    {
        moveDirection = (transform.right * inputX) + (transform.forward * inputZ);
        cc.Move(moveDirection.normalized * movingSpeed * Time.deltaTime);

        velocity.y -= 9.81f * Time.deltaTime; //Gravity
        cc.Move(velocity * Time.deltaTime);
    }

    void Rotate()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            lockedDirection = moveDirection;
            playerCharacter.transform.rotation = Quaternion.Slerp(playerCharacter.transform.rotation, Quaternion.LookRotation(lockedDirection), 6 * Time.deltaTime);
        }
    }

}
