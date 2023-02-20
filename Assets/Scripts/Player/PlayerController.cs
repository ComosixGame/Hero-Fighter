using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputSystem           playerInputSystem;
    private Animator                    animator;
    private CharacterController         characterController;
    private Vector3                     direction;
    private bool                        turnLeft, turnRight;
    private float                       fallingVelocity;
    private float                       jumpVelocity;
    [SerializeField]
    private float                       speed = 20f;
    private float                       gravity = - 9.81f;
    private int                         velocityHash;
    private int                         jumpHash;
    private bool inJump;



    private void Awake() 
    {
        characterController = GetComponent<CharacterController>();
        playerInputSystem   = new PlayerInputSystem();

        velocityHash        = Animator.StringToHash("Velocity");
        jumpHash            = Animator.StringToHash("Jump");

    }

    private void OnEnable() 
    {
        playerInputSystem.Enable();
        playerInputSystem.Player.Move.performed += GetDirectionMove;
        playerInputSystem.Player.Move.canceled  += GetDirectionMove;
        playerInputSystem.Player.Jump.started   += HandleJump;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        RotationLook();
        HandleGravity();
        HandleAnimation();
    }

    private void GetDirectionMove(InputAction.CallbackContext ctx) {
        Vector2 dir = ctx.ReadValue<Vector2>();
        direction = new Vector3(dir.x, 0, dir.y);
    }

    private void RotationLook()
    {
        if(direction.x < 0) {
            turnLeft =  true;
            turnRight =  false;
        } else if(direction.x > 0) {
            turnRight = true;
            turnLeft =  false;
        }

        if(turnRight) {
            Quaternion rot = Quaternion.LookRotation(Vector3.right);
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, rot, 25 * Time.deltaTime);
            if(Vector3.Angle(transform.forward, Vector3.right) <= 0) {
                turnRight = false;
            }
        }

        if(turnLeft) {
            Quaternion rot = Quaternion.LookRotation(Vector3.left);
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, rot, 25 * Time.deltaTime);
            if(Vector3.Angle(transform.forward, Vector3.left) <= 0) {
                turnLeft = false;
            }
        }
    }

    private void Move()
    {
        Vector3 motionMove = direction*speed*Time.deltaTime;
        Vector3 motionFall = Vector3.up*fallingVelocity*Time.deltaTime;
        Vector3 motionJump = Vector3.up*jumpVelocity*Time.deltaTime;
        characterController.Move(motionMove + motionFall + motionJump);
    }

    private void HandleGravity()
    {
        if(characterController.isGrounded)
        {
            jumpVelocity = 0;
            fallingVelocity = gravity/10;
            inJump = false;
        } 
        else
        {
            jumpVelocity -= 10;
            fallingVelocity += gravity/10;
        }
            Debug.Log(jumpVelocity);


    }

    private void HandleJump(InputAction.CallbackContext ctx)
    {
        if(!inJump) {
            inJump = true;
            animator.SetTrigger(jumpHash);
            jumpVelocity = 300;
        }
    }

    private void HandleAnimation()
    {
        Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0 , characterController.velocity.z);
        float Velocity = horizontalVelocity.magnitude/speed;
        if (Velocity > 0)
        {
            animator.SetFloat(velocityHash, Velocity);
        } else 
        {
            float v = animator.GetFloat(velocityHash);
            v = v> 0.10f? Mathf.Lerp(v, 0, 20f*Time.deltaTime) : 0;
            animator.SetFloat(velocityHash, v);
        }
    }

    private void OnDisable() 
    {
        playerInputSystem.Player.Move.performed -= GetDirectionMove;
        playerInputSystem.Player.Move.canceled  -= GetDirectionMove;
        playerInputSystem.Player.Jump.canceled  -= HandleJump;
    }
}
