using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Animator))]
public class NewPlayerController : MonoBehaviour
{
    private int velocityXHash;
    private int velocityZHash;
    private Vector3 direction;
    private Animator animator;
    public PlayerInputSystem playerInputSystem;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerInputSystem = new PlayerInputSystem();
        velocityXHash = Animator.StringToHash("VelocityX");
        velocityZHash = Animator.StringToHash("VelocityZ");
    }

    private void OnEnable()
    {
        playerInputSystem.Enable();
        playerInputSystem.Player.Move.performed += GetDirectionMove;
        playerInputSystem.Player.Move.canceled += GetDirectionMove;
    }

    private void OnDisable() {
        playerInputSystem.Disable();
        playerInputSystem.Player.Move.performed -= GetDirectionMove;
        playerInputSystem.Player.Move.canceled -= GetDirectionMove;
    }

    private void Update() {
        HandleAnimation();
        RotationLook();
    }

    private void GetDirectionMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        direction = new Vector3(dir.x, 0, dir.y);
    }

    private void HandleAnimation()
    {
        Vector3 motionMove = direction.normalized;
        animator.SetFloat(velocityXHash, Math.Abs(motionMove.x));
        animator.SetFloat(velocityZHash, motionMove.z);
    }

    private void RotationLook()
    {
        if (direction.x < 0)
        {
            Quaternion rot = Quaternion.LookRotation(Vector3.left);
            transform.rotation = rot;
        }
        else if (direction.x > 0)
        {
            Quaternion rot = Quaternion.LookRotation(Vector3.right);
            transform.rotation = rot;
        }
    }
}
