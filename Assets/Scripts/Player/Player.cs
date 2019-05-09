using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float runSpeed = 8f;
    public float walkSpeed = 6f;
    public float gravity = -10f;
    public float jumpHeight = 15f;

    [Header("Dash")]
    public float dashSpeed = 50f;
    public float dashDuration = .5f;
    public float dashtimer;
    public bool dashing;
    Quaternion _boostForward;

    public bool dashingDir;

    private CharacterController controller; // Reference to CharacterController
    private Vector3 motion; // Is the movement offset per frame
    private bool isJumping;
    private float currentJumpHeight, currentSpeed;

    private void OnValidate()
    {
        currentJumpHeight = jumpHeight;
        currentSpeed = walkSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Set intial states
        currentSpeed = walkSpeed;
        currentJumpHeight = jumpHeight;
    }

    // Update is called once per frame
    void Update()
    {
        // Get W, A, S, D or Left, Right, Up, Down Input
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        // Left Shift Input
        bool inputRun = Input.GetKeyDown(KeyCode.LeftShift);
        bool inputWalk = Input.GetKeyUp(KeyCode.LeftShift);

        // Spacebar Input 
        bool inputJump = Input.GetButtonDown("Jump");

        if(inputRun)
        {
            currentSpeed = runSpeed;
        }
        if(inputWalk)
        {
            currentSpeed = walkSpeed;
        }

        if(dashing)
        {
            inputV += Mathf.Lerp(1,0,1-dashtimer);
            dashtimer -= Time.deltaTime;
            currentSpeed = dashSpeed;
            if(dashtimer < 0)
            {
                dashing = false;
                currentSpeed = inputRun ? runSpeed : walkSpeed;
            }
        }
        if(dashingDir)
        {
            inputV = BoostDirectionPad(_boostForward).eulerAngles.normalized.z;
            inputH = BoostDirectionPad(_boostForward).eulerAngles.normalized.x;
            dashtimer -= Time.deltaTime;
            currentSpeed = dashSpeed;
            if (dashtimer < 0)
            {
                dashingDir = false;
                currentSpeed = inputRun ? runSpeed : walkSpeed;
            }
        }


        // Move character motion with inputs
        Move(inputH, inputV);
        // Is the Player grounded?
        if (controller.isGrounded)
        {
            // Cancel gravity
            motion.y = 0f;
            // Pressing Jump Button?
            if (inputJump)
            {
                Jump(jumpHeight);
            }
            if(isJumping)
            {
                // Make the Player Jump!
                motion.y = currentJumpHeight;
                // Reset back to false
                isJumping = false;
            }
        }
        // Apply gravity
        motion.y += gravity * Time.deltaTime;
        // Move the controller with motion
        controller.Move(motion * Time.deltaTime);
    }


    // Move the character's motion in direction of input
    void Move(float inputH, float inputV)
    {
        // Generate direction from input
        Vector3 direction = new Vector3(inputH, 0f, inputV);

        // Convert local space to world space direction
        direction = transform.TransformDirection(direction);

        // check if direction exceeds magnitude of 1
        if(direction.magnitude > 1f)
        {
            // normalise
            direction.Normalize();
        }
        // Apply motion to only X and Z
        motion.x = direction.x * currentSpeed;
        motion.z = direction.z * currentSpeed;
    }

    public void Jump(float height)
    {
        isJumping = true; // tell controller to jump at the right time
        currentJumpHeight = height;

    }

    public Quaternion BoostDirectionPad(Quaternion boostForward)
    {
        _boostForward = boostForward;
        Debug.Log("PlayerForward: "+transform.rotation);
        Debug.Log("PADForward: " + boostForward);
        dashtimer = dashDuration;
        dashingDir = true;
        Quaternion playerForward = transform.rotation;
        Quaternion newRotation = playerForward * boostForward;
        return newRotation;
    }

    public void Dash()
    {
        dashtimer = dashDuration;
        dashing = true;

        //StartCoroutine(SpeedBoost(dashSpeed, walkSpeed, dashDuration));
    }

    //public IEnumerator SpeedBoost(float startSpeed, float endSpeed, float duration)
    //{
    //    currentSpeed = startSpeed;
    //    float startTime = 0;
    //    while(startTime < duration)
    //    {
    //        Move(0, 1);
    //        startTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    //yield return new WaitForSeconds(duration);

    //    currentSpeed = endSpeed;
    //}
}
