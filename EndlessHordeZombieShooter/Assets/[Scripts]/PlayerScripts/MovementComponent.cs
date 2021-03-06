using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    [SerializeField]
    float walkSpeed = 5;
    [SerializeField]
    float runSpeed = 10;
    [SerializeField]
    float jumpForce = 5;

    // components
    private PlayerController playerController;
    Rigidbody rigidbody;
    Animator playerAnimator;
    public GameObject followTarget;

    // references
    Vector2 inputVector = Vector2.zero;
    Vector3 moveDirection = Vector3.zero;
    Vector2 lookInput = Vector2.zero;

    public float aimSensitiviy = 1;

    // animator hashes
    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isJumpingHash = Animator.StringToHash("isJumping");
    public readonly int isRunningHash = Animator.StringToHash("isRunning");
    public readonly int isFiringHash = Animator.StringToHash("isFiring");
    public readonly int isReloadingHash = Animator.StringToHash("isReloading");
    public readonly int aimVerticalHash = Animator.StringToHash("AimVertical");

    private void Awake() 
    {
        playerAnimator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();

        if(!GameManager.instance.cursorActive)
        {
            AppEvents.InvokeMouseCursorEnable(false);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        //aiming/looking
        // horizontal rotation
        followTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.x * aimSensitiviy, Vector3.up);
        // vertical rotation
        followTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.y * aimSensitiviy, Vector3.left);

        var angles = followTarget.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTarget.transform.localEulerAngles.x;
        
        if(angle > 180 && angle < 300)
        {
            angles.x = 300;
        }
        else if(angle < 180 && angle > 70)
        {
            angles.x = 70;
        }

        followTarget.transform.localEulerAngles = angles;

        // rotate the player rotation based on look transform
        transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);

        followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
        playerAnimator.SetFloat(aimVerticalHash, 1);

        // movement
        if(playerController.isJumping) return;
        if(!(inputVector.magnitude > 0)) moveDirection = Vector3.zero;

        moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;
        float currentSpeed = playerController.isRunning ? runSpeed : walkSpeed;

        Vector3 movementDirection = moveDirection * (currentSpeed * Time.deltaTime);
        transform.position += movementDirection;

        
    }

    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
        playerAnimator.SetFloat(movementXHash, inputVector.x);
        playerAnimator.SetFloat(movementYHash, inputVector.y);
    }

    public void OnRun(InputValue value)
    {
        playerController.isRunning = value.isPressed;
        playerAnimator.SetBool(isRunningHash, playerController.isRunning);
    }

    public void OnJump(InputValue value)
    {
        if(playerController.isJumping) return;

        playerController.isJumping = value.isPressed;
        rigidbody.AddForce((transform.up + moveDirection) * jumpForce, ForceMode.Impulse);
        playerAnimator.SetBool(isJumpingHash, playerController.isJumping);
    }

    public void OnAim(InputValue value) 
    {
        playerController.isAiming = value.isPressed;
    }
        
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
        float lookParameter = Mathf.InverseLerp(70, 300, followTarget.transform.localEulerAngles.y);
        //playerAnimator.SetFloat(aimVerticalHash, lookInput.y);
        // if we aim up, down, adjust animations to have a mask that will let us properly animate aim
    }


    private void OnCollisionEnter(Collision other) 
    {
        if(!other.gameObject.CompareTag("Ground") && !playerController.isJumping) return;

        playerController.isJumping = false;
        playerAnimator.SetBool(isJumpingHash, false);
    }
}
