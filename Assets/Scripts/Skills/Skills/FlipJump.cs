using UnityEngine;
using UnityEngine.InputSystem;


public class FlipJump : AbsSkill
{
    [SerializeField] private float speedJump;
    [SerializeField] private AnimationCurve forwardFlipCurve;
    [SerializeField] private AnimationCurve backFlipCurve;
    [SerializeField] private AnimationCurve sideFlipCurve;
    [SerializeField] private EffectObjectPool flipJumpVFX;

    private AnimationCurve speedCurve;
    private float currentSpeed;
    private bool inJump;
    private int jumpIndexHash;
    private float timer;
    private int originlayer;
    private Vector3 direction, directionJump;
    private Animator animator;
    private CharacterController controller;
    private PlayerInputSystem playerInput;
    private ObjectPoolerManager objectPoolerManager;

    private void Awake()
    {
        objectPoolerManager = ObjectPoolerManager.Instance;

        controller = GetComponentInParent<CharacterController>();
        animator = GetComponentInParent<Animator>();
        jumpIndexHash = Animator.StringToHash("JumpIndex");
        originlayer = gameObject.layer;
    }

    private void OnEnable()
    {
        OnDone += CanceleJump;
    }

    private void Start()
    {
        playerInput = GetComponentInParent<PlayerController>().playerInputSystem;

        playerInput.Player.Move.performed += GetDirection;
        playerInput.Player.Move.canceled += GetDirection;
    }

    private void Update()
    {
        if (inJump)
        {
            timer += Time.deltaTime;
            float time = Mathf.Clamp01(timer);
            currentSpeed = speedCurve.Evaluate(time) * speedJump;
            controller.SimpleMove(directionJump * currentSpeed);
        }


    }

    private void OnDisable()
    {
        OnDone -= CanceleJump;
        playerInput.Player.Move.performed -= GetDirection;
        playerInput.Player.Move.canceled -= GetDirection;
    }

    protected override void Action()
    {
        gameObject.layer = LayerMask.NameToLayer("Dodgelayer");
        timer = 0;
        inJump = true;
        currentSpeed = speedJump;
        int indexAnim;

        float dotDir = Vector3.Dot(transform.forward.normalized, direction.normalized);

        if (dotDir > 0)
        {
            indexAnim = 1;
            speedCurve = forwardFlipCurve;
            directionJump = direction.normalized;

        }
        else
        {
            indexAnim = 2;
            speedCurve = backFlipCurve;
            directionJump = -transform.forward.normalized;
        }


        // lộn trái phải (để lại để sau này có thể dùng)

        bool faceLeft = Vector3.Dot(transform.forward.normalized, Vector3.left) == 1;
        if (Vector3.Dot(direction.normalized, Vector3.forward) > 0)
        {
            indexAnim = faceLeft ? 4 : 3;
            speedCurve = sideFlipCurve;
            directionJump = direction.normalized;

        }
        else if (Vector3.Dot(direction.normalized, Vector3.forward) < 0)
        {
            indexAnim = faceLeft ? 3 : 4;
            speedCurve = sideFlipCurve;
            directionJump = direction.normalized;

        }

        animator.SetFloat(jumpIndexHash, indexAnim);
    }

    private void CanceleJump()
    {
        gameObject.layer = originlayer;
        inJump = false;
        animator.SetFloat(jumpIndexHash, 0);
    }



    private void GetDirection(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        direction = new Vector3(dir.x, 0, dir.y);
    }

    //Attach In Animation Event
    public void FlipJumpEffect()
    {
        objectPoolerManager.SpawnObject(flipJumpVFX, transform.position, Quaternion.identity);
    }
}
