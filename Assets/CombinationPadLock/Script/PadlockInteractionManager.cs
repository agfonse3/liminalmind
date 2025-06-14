using UnityEngine;

public class PadlockInteractionManager : MonoBehaviour
{
    public GameObject player;
    public Transform mainCamera; // Assign your Main Camera here
    public GameObject interactionUI;
    public float zoomSpeed = 5f;
    private bool returningToPlayer = false;

    private bool isInteracting = false;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    private Transform padlockFocusPoint; // Assigned on interaction

    public void StartPadlockInteraction(Transform padlockFocus)
    {
        isInteracting = true;
        padlockFocusPoint = padlockFocus;

        // Save camera position
        originalCameraPosition = mainCamera.position;
        originalCameraRotation = mainCamera.rotation;

        // Freeze player movement
        player.GetComponent<FirstPersonController>().enabled = false;

        if (interactionUI != null)
            interactionUI.SetActive(true);

        // ENABLE the padlock input
        var moveRuller = padlockFocusPoint.GetComponentInParent<MoveRuller>();
        if (moveRuller != null)
            moveRuller.isActive = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void EndPadlockInteraction()
    {
        returningToPlayer = true;
        var moveRuller = padlockFocusPoint.GetComponentInParent<MoveRuller>();
        if (moveRuller != null)
            moveRuller.isActive = false;

        if (interactionUI != null)
            interactionUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        if (isInteracting && padlockFocusPoint != null && !returningToPlayer)
        {
            // Move camera to padlock
            mainCamera.position = Vector3.Lerp(mainCamera.position, padlockFocusPoint.position, Time.deltaTime * zoomSpeed);
            mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, padlockFocusPoint.rotation, Time.deltaTime * zoomSpeed);
        }

        if (returningToPlayer)
        {
            mainCamera.position = Vector3.Lerp(mainCamera.position, originalCameraPosition, Time.deltaTime * zoomSpeed);
            mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, originalCameraRotation, Time.deltaTime * zoomSpeed);

            // Close enough — restore control
            if (Vector3.Distance(mainCamera.position, originalCameraPosition) < 0.01f)
            {
                returningToPlayer = false;
                isInteracting = false;
                player.GetComponent<FirstPersonController>().enabled = true;
            }
        }

        // Allow exit with Escape
        if (isInteracting && Input.GetKeyDown(KeyCode.Escape))
        {
            EndPadlockInteraction();
        }
    }

}


