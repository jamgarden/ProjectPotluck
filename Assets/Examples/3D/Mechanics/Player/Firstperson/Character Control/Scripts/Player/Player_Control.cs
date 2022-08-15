using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseLook))]
[RequireComponent(typeof(CharacterController))]
public class Player_Control : MonoBehaviour
{
    private MouseLook mouseLook;

    private CharacterController cc;

    [Header("Abilities")]
    public bool canMove = true;
    public float walkSpeed = 3.2f;

    public bool canCrouch = true;
    public float crouchSpeed = 2.4f;

    public bool canRun = true;
    public float runSpeed = 4.6f;

    float inputX, inputZ;
    private float movingSpeed;
    private Vector3 moveDirection;
    private Vector3 velocity;

    [Space(10)]
    public bool canJump = true;
    public float jumpForce = 2;

    //Crouching
    private bool isCrouching = false;
    private float orginalHeight;
    private float wantedHeight;
    private float crouchLerp;

    private bool isJumping;
    private float coolDownJumping;

    private float onGroundTimer;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        mouseLook = gameObject.GetComponent<MouseLook>();
        cc = gameObject.GetComponent<CharacterController>();

        cc.center = new Vector3(0, 1, 0);
        cc.height = 2;
        cc.radius = 0.4f;
        cc.stepOffset = 0.3f;

        movingSpeed = walkSpeed;

        orginalHeight = cc.height;
        wantedHeight = orginalHeight / 2;
        crouchLerp = 0;
    }

    private void Update()
    {
        Inputs();

        if(isGrounded) AdjustSpeed();
       
        Move();
        Crouch();
        Other();
    }

    void Other()
    {
        //stepoffset
        if(!isGrounded && cc.stepOffset != 0)
        {
            cc.stepOffset = 0;
        }

        else if(isGrounded && cc.stepOffset != 0.3f)
        {
            cc.stepOffset = 0.3f;
        }

        //Reset jumping once hit ground.
        if(isJumping && coolDownJumping <= 0 && isGrounded)
        {
            isJumping = false;
        }

        //Eliminates insta fast falling
        if(isGrounded && onGroundTimer < 0.2f)
        {
            onGroundTimer += Time.deltaTime;
        }

        if(coolDownJumping > 0)
        {
            coolDownJumping -= Time.deltaTime;
        }

        if(!isGrounded && onGroundTimer >= 0.2f && !isJumping)
        {
            velocity.y = 0;
            onGroundTimer = 0;
        }

        
    }

    void Inputs()
    {
        if (canMove && isGrounded)
        {
            inputX = Input.GetAxis("Horizontal");
            inputZ = Input.GetAxis("Vertical");
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(canJump && isGrounded && !isCrouching)
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.LeftControl) && canCrouch)
        {
            crouchLerp = 0;

            if(!cantStand && isCrouching)
            {
                isCrouching = false;
            }

            else
            {
                isCrouching = true;
            }
        }
    }

    void AdjustSpeed()
    {
        if (!isCrouching)
        {
            //Walking
            if (movingSpeed != walkSpeed && Input.GetKeyUp(KeyCode.LeftShift) || movingSpeed == crouchSpeed)
            {
                movingSpeed = walkSpeed;
            }


            //Running
            else if (movingSpeed != runSpeed && Input.GetKeyDown(KeyCode.LeftShift) && canRun)
            {
                movingSpeed = runSpeed;
            }

            if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                movingSpeed = walkSpeed;
            }
        }

        //Crouching
        if(isCrouching && movingSpeed != crouchSpeed)
        {
            movingSpeed = crouchSpeed;
        }
    }

    void Move()
    {
        moveDirection = (transform.right * inputX) + (transform.forward * inputZ);
        cc.Move(moveDirection.normalized * movingSpeed * Time.deltaTime);

        velocity.y -= 9.81f * Time.deltaTime; //Gravity
        cc.Move(velocity * Time.deltaTime);
    }

    void Crouch()
    {
        if(crouchLerp < 1)
        crouchLerp += 2.5f * Time.deltaTime;

       crouchLerp = Mathf.Clamp(crouchLerp, 0, 1);

        if (!isCrouching && cc.height != orginalHeight)
        {
            cc.height = Mathf.Lerp(cc.height, orginalHeight, crouchLerp);
            cc.center = Vector3.Lerp(cc.center, new Vector3(0, orginalHeight / 2, 0), crouchLerp);
            mouseLook.mainCamera.transform.position = Vector3.Slerp(mouseLook.mainCamera.transform.position, transform.position + new Vector3(0, 1.5f, 0), 5f * Time.deltaTime);
        }

        if(isCrouching && cc.height != wantedHeight)
        {
            cc.height = Mathf.Lerp(cc.height, wantedHeight, crouchLerp);
            cc.center = Vector3.Lerp(cc.center, new Vector3(0, wantedHeight / 2, 0), crouchLerp);
            mouseLook.mainCamera.transform.position = Vector3.Slerp(mouseLook.mainCamera.transform.position, transform.position + new Vector3(0, 0.5f, 0), 5f * Time.deltaTime);
        }
    }

    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        isJumping = true;
        coolDownJumping = 0.2f;
    }
    private bool isGrounded
    {
        get
        {
            return
                Physics.Raycast(transform.position + new Vector3(0, 0.25f, 0), Vector3.down, 0.5f);
        }
    }

    private bool cantStand
    {
        get
        {
            return
                Physics.Raycast(transform.position + new Vector3(0, 1f, 0), Vector3.up, 2.5f);
        }
    }

    public void DisablePlayer(bool canWeMove, bool canWeTurn, bool canWeCrouch, bool canWeJump)
    {
        canMove = canWeMove;
        canCrouch = canWeCrouch;
        canJump = canWeJump;

        GetComponent<MouseLook>().canTurn = canWeTurn;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PickAndDrop>().enabled = false;

        Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = false;
    }
}
