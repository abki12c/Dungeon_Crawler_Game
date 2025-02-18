using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;
    Camera mainCamera;  // Reference to the main camera

    int isWalkingHash;
    int isRunningHash;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;
    bool isRunPressed;
    bool isJumpPressed;  // Flag to track if the player is jumping

    [Header("Movement Settings")]
    public float walkSpeed = 3.5f;
    public float runSpeed = 5.0f;
    public float rotationSpeed = 10.0f;
    public float gravity = -9.8f;
    public float jumpHeight = 1.5f;  // Jump height variable

    private Vector3 playerVelocity;
    private bool isGrounded;

    private void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main; // Get the main camera

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        // Subscribe to Input Actions
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;  // Add the jump action
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void onJump(InputAction.CallbackContext context)
    {
        // When the jump button is pressed, set the jump flag to true
        if (characterController.isGrounded)
        {
            isJumpPressed = true;
        }
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();

        // Reset movement
        currentMovement = Vector3.zero;

        // Get camera directions
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        // Flatten to ignore vertical tilt
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Convert movement input to world space
        currentMovement = cameraForward * currentMovementInput.y + cameraRight * currentMovementInput.x;

        if (currentMovement.magnitude > 1)
        {
            currentMovement.Normalize();
        }

        isMovementPressed = currentMovement.magnitude > 0;
    }

    void handleRotation()
    {
        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentMovement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (isMovementPressed && isRunPressed && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    void handleGravity()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded)
        {
            playerVelocity.y = -0.05f;  // Small value to keep the player grounded
        }
        else
        {
            playerVelocity.y += gravity * Time.deltaTime;  // Apply gravity
        }
    }

    void handleJump()
    {
        if (isJumpPressed && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);  // Apply jump force
            isJumpPressed = false;  // Reset jump flag
        }
    }

    void Update()
    {
        handleGravity();
        handleRotation();
        handleAnimation();
        handleJump();  // Call jump handling

        float moveSpeed = isRunPressed ? runSpeed : walkSpeed;
        characterController.Move((currentMovement * moveSpeed + playerVelocity) * Time.deltaTime);
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
