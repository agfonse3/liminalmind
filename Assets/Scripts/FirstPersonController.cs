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
    public Transform cameraTransform; // Asigna el transform de la c�mara en el inspector.
    private float xRotation = 0f; // Rotaci�n actual de la c�mara sobre el eje X.

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

    private CharacterController controller; // Referencia al componente CharacterController.
    private Vector3 velocity; // Velocidad actual del jugador.
    private bool isGrounded; // Confirma si el juegador est� en el suelo
    private float standingCameraY;
    private float crouchingCameraY;
    private float currentCameraY;

    private SanitySystem sanitySystem; // Referencia al componente SanitySystem.

    void Start()
    {
        // Conseguir el componente CharacterController ligado al GameObject.
        controller = GetComponent<CharacterController>();

        // Deja el cursos en el centro de la pantalla y lo esconde.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalHeight = controller.height;
        currentStamina = maxStamina;

        cameraStartPos = cameraTransform.localPosition;
        standingCameraY = cameraStartPos.y;
        crouchingCameraY = standingCameraY - 0.5f;
        currentCameraY = standingCameraY;

        // Conseguir el componente SanitySystem, o lo a�ade si no existe para prevenir componentes duplicados.
        sanitySystem = GetComponent<SanitySystem>();
        if (sanitySystem == null)
        {
            sanitySystem = gameObject.AddComponent<SanitySystem>();
        }
        sanitySystem.cameraTransform = cameraTransform; // Asignar transform de la c�mara al SanitySystem.
    }

    void Update()
    {
        // Desbloquear curso si se presiona Esc.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        UpdateIsGrounded(); // Revisa si el jugador est� en el suelo.

        HandleMouseLook(); // Se encarga de la rotaci�n de la c�mara basada en el input del rat�n.
        HandleCrouch(); // Se encarga del input para agacharse y las transiciones de ese estado.
        HandleMovement(); // Se encarga del movimiento del jugador(caminar, correr, saltar).
        HandleHeadBob(); // Se encarga del meneo de la cabeza.
        RegenerateStamina(); // Regenera energ�a con el tiempo.
        sanitySystem.HandleSanity(); // LLama la l�gica del Update de SanitySystem.

    }

    void HandleMouseLook()
    {
        // Conseguir input del rat�n.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Ajustar rotaci�n de la c�mara en base al input del rat�n.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limita la rotaci�n vertical de la c�mara.

        // Aplica la rotaci�n al transform de la c�mara.
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Gira al GameObject jugador horizontalmente.
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        // Reinicia velocidad vertical cuando est� en el suelo.
        if (isGrounded && velocity.y < 0)
            velocity.y = -0.1f;

        // Conseguir input horizontal y vertical.
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ; // Calcular direcci�n del movimiento.

        // Determina si el jugador quiere correr.
        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift) && !isCrouching && move.magnitude > 0.1f;

        // Se encarga de correr y consumo de energ�a.
        if (wantsToSprint && currentStamina > sprintStaminaThreshold)
        {
            isSprinting = true;
            currentStamina -= staminaDrain * Time.deltaTime;
            regenTimer = 0f; // Reinicia el contador de regeneraci�n de energ�a.
        }
        else
        {
            isSprinting = false;
        }

        // Evita poder correr con 0 de energ�a.
        if (currentStamina <= 0f)
        {
            currentStamina = 0f;
            isSprinting = false;
        }

        // Comienza a regenerar energ�a despu�s de un momento.
        if (!isSprinting && !wasSprintingLastFrame)
            regenTimer += Time.deltaTime;

        // Determina la velocidad de moviemiento actual.
        float speed = walkSpeed;
        if (isSprinting)
            speed = sprintSpeed;
        else if (isCrouching)
            speed = crouchSpeed;

        // Mueve al jugador usando el CharacterController.
        controller.Move(move * speed * Time.deltaTime);

        // Se encarga de saltar.
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Aplicar gravedad.
        velocity.y += gravity * Time.deltaTime;

        // Mueve al jugador verticalmente.
        controller.Move(velocity * Time.deltaTime);

        wasSprintingLastFrame = isSprinting;
    }

    void HandleCrouch()
    {
        // Cambia entre agachado/erguido cuando se presiona la tecla.
        if (Input.GetKeyDown(KeyCode.LeftControl))
            isCrouching = !isCrouching;

        // Transici�n m�s suave de la altura entre estados agachado/erguido.
        float targetHeight = isCrouching ? crouchHeight : originalHeight;
        controller.height = Mathf.MoveTowards(controller.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);

        // Transici�n m�s suave de la posici�n vertical de la c�mara.
        float targetCamY = isCrouching ? crouchingCameraY : standingCameraY;
        currentCameraY = Mathf.MoveTowards(currentCameraY, targetCamY, crouchTransitionSpeed * Time.deltaTime);
    }

    void RegenerateStamina()
    {
        // Regenera energ�a si no se est� corriendo y despu�s de un momento.
        if (!isSprinting && regenTimer >= regenDelay && currentStamina < maxStamina)
        {
            currentStamina += staminaRegen * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // Cappear energ�a a valor m�ximo.
        }
    }

    void UpdateIsGrounded()
    {
        // Ver si el jugaro est� en el suelo usando CharacterController.isGrounded.
        isGrounded = controller.isGrounded;

        // Realiza un raycast adicional�para detecci�n de suelo m�s precisa.
        if (!isGrounded)
        {
            // Lanza un peque�o rayp hacia abajo para revisar si esta en el suelo.
            Vector3 origin = transform.position + Vector3.up * 0.1f;
            isGrounded = Physics.Raycast(origin, Vector3.down, out _, 0.2f);
        }
    }

    void PlayFootstep()
    {
        // Reproduce sonidos de pasos.

        if (footstepClips.Length == 0 || !footstepSource || !isGrounded)
            return;

        int index = Random.Range(0, footstepClips.Length);
        footstepSource.pitch = Random.Range(minStepPitch, maxStepPitch);
        footstepSource.PlayOneShot(footstepClips[index]);
    }

    void HandleHeadBob()
    {
        // Se encarga del efecto de menear la cabeza.

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
            float sine = Mathf.Sin(bobTimer);


            // Activa un paso para reproducir un sonido cuando termina de menear la cabeza del todo.
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
            stepTriggered = false;

            Vector3 camPos = cameraTransform.localPosition;
            camPos.y = Mathf.Lerp(camPos.y, currentCameraY, Time.deltaTime * 5f);
            cameraTransform.localPosition = camPos;
        }
    }
}





