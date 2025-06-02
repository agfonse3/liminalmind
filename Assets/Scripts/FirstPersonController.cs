using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float crouchSpeed = 2f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Footstep Settings")]
    public AudioClip[] footstepClips;
    public AudioSource footstepSource;
    public float minStepPitch = 0.9f;
    public float maxStepPitch = 1.1f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;
    private float xRotation = 0f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float crouchTransitionSpeed = 8f;
    private float originalHeight;
    private bool isCrouching = false;

    [Header("Stamina Settings")]
    public float maxStamina = 5f;
    public float staminaDrain = 1f;
    public float staminaRegen = 2f;
    public float regenDelay = 2f;
    public float sprintStaminaThreshold = 1f;

    private float currentStamina;
    private float regenTimer;
    private bool isSprinting;
    private bool wasSprintingLastFrame;

    [Header("Head Bobbing")]
    public float bobFrequency = 8f;
    public float bobAmplitude = 0.05f;
    private float bobTimer = 0f;
    private bool stepTriggered = false;
    private Vector3 cameraStartPos;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float standingCameraY;
    private float crouchingCameraY;
    private float currentCameraY;

    [Header("Sanity Settings")]
    public float maxSanity = 100f;
    public float sanityDecreaseRate = 5f;
    public float sanityRegenRate = 2f;
    public float sanityRegenDelay = 3f;
    private float currentSanity;
    private float timeSinceLastSeen = 0f;
    public Transform sanityTarget;
    public LayerMask lineOfSightObstacles;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        originalHeight = controller.height;
        currentStamina = maxStamina;

        cameraStartPos = cameraTransform.localPosition;
        standingCameraY = cameraStartPos.y;
        crouchingCameraY = standingCameraY - 0.5f;
        currentCameraY = standingCameraY;
        currentSanity = maxSanity;
    }

    void Update()
    {
        HandleMouseLook();
        HandleCrouch();
        HandleMovement();
        HandleHeadBob();
        RegenerateStamina();
        HandleSanity();

    }

    void HandleSanity()
    {
        Vector3 directionToTarget = sanityTarget.position - cameraTransform.position;
        float angleToTarget = Vector3.Angle(cameraTransform.forward, directionToTarget);

        Ray ray = new Ray(cameraTransform.position, directionToTarget.normalized);
        RaycastHit hit;

        bool seesEntity = false;

        if (angleToTarget < 60f) // Field of view angle (can adjust)
        {
            if (Physics.Raycast(ray, out hit, 30f, lineOfSightObstacles))
            {
                if (hit.transform == sanityTarget)
                {
                    seesEntity = true;
                }
            }
        }

        if (seesEntity)
        {
            currentSanity -= sanityDecreaseRate * Time.deltaTime;
            currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);
            timeSinceLastSeen = 0f;
        }
        else
        {
            timeSinceLastSeen += Time.deltaTime;
            if (timeSinceLastSeen >= sanityRegenDelay)
            {
                currentSanity += sanityRegenRate * Time.deltaTime;
                currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);
            }
        }

        Debug.Log("Sanity: " + currentSanity);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift) && !isCrouching && moveZ > 0;

        if (wantsToSprint && currentStamina > sprintStaminaThreshold)
        {
            isSprinting = true;
            currentStamina -= staminaDrain * Time.deltaTime;
            regenTimer = 0f;
        }
        else
        {
            isSprinting = false;
        }

        if (currentStamina <= 0f)
        {
            currentStamina = 0f;
            isSprinting = false;
        }

        if (!isSprinting && !wasSprintingLastFrame)
            regenTimer += Time.deltaTime;

        float speed = walkSpeed;
        if (isSprinting)
            speed = sprintSpeed;
        else if (isCrouching)
            speed = crouchSpeed;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        wasSprintingLastFrame = isSprinting;
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            isCrouching = !isCrouching;

        float targetHeight = isCrouching ? crouchHeight : originalHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);

        float targetCamY = isCrouching ? crouchingCameraY : standingCameraY;
        currentCameraY = Mathf.Lerp(currentCameraY, targetCamY, Time.deltaTime * crouchTransitionSpeed);
    }

    void RegenerateStamina()
    {
        if (!isSprinting && regenTimer >= regenDelay && currentStamina < maxStamina)
        {
            currentStamina += staminaRegen * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    void PlayFootstep()
    {
        if (footstepClips.Length == 0 || !footstepSource || !isGrounded)
            return;

        int index = Random.Range(0, footstepClips.Length);
        footstepSource.pitch = Random.Range(minStepPitch, maxStepPitch);
        footstepSource.PlayOneShot(footstepClips[index]);
    }

    void HandleHeadBob()
    {
        bool isMovingAndGrounded = isGrounded &&
            (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f);

        if (isMovingAndGrounded)
        {
            float currentBobFreq = bobFrequency;
            float currentBobAmp = bobAmplitude;

            if (isSprinting)
            {
                currentBobFreq *= 1.5f;
                currentBobAmp *= 1.5f;
            }
            else if (isCrouching)
            {
                currentBobFreq *= 0.5f;
                currentBobAmp *= 0.5f;
            }

            bobTimer += Time.deltaTime * currentBobFreq;
            float bobOffset = Mathf.Sin(bobTimer) * currentBobAmp;

            // Trigger step when bob reaches downward trough
            if (Mathf.Sin(bobTimer) < -0.95f && !stepTriggered)
            {
                PlayFootstep();
                stepTriggered = true;
            }
            else if (Mathf.Sin(bobTimer) > 0f)
            {
                stepTriggered = false;
            }

            Vector3 camPos = cameraTransform.localPosition;
            camPos.y = currentCameraY + bobOffset;
            cameraTransform.localPosition = camPos;
        }
        else
        {
            bobTimer = Mathf.Lerp(bobTimer, 0f, Time.deltaTime * 5f);
            stepTriggered = false;

            Vector3 camPos = cameraTransform.localPosition;
            camPos.y = Mathf.Lerp(camPos.y, currentCameraY, Time.deltaTime * 5f);
            cameraTransform.localPosition = camPos;
        }
    }
}

