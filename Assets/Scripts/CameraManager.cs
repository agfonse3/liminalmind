using UnityEngine;

public class CameraManager : MonoBehaviour
{
     public Transform[] views;
    public float transitionSpeed;
    private Transform currentView;
    public FirstPersonController playerController; // Referencia al script del jugador
 private Transform defaultView; // Guarda la vista original
    void Start()
    {
        currentView = transform;
        currentView = defaultView;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentView = views[0];
            BloquearMovimiento(true); // Bloquea el movimiento del jugador
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            currentView = views[1];
            BloquearMovimiento(true); // Bloquea el movimiento del jugador
        }
        if (Input.GetKeyDown(KeyCode.M)) // Nueva tecla para desbloquear
        {

            BloquearMovimiento(false); // Restaura el movimiento del jugador
            currentView = defaultView; // Regresa la cámara a la vista del jugador
        }
    }

    private void LateUpdate()
    {
        if (currentView != null) // Verifica si currentView tiene un valor válido
        {
            transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);

            Vector3 currentAngle = new Vector3(
                Mathf.Lerp(transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
                Mathf.Lerp(transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
                Mathf.Lerp(transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed)
            );

            transform.eulerAngles = currentAngle;
        }
    }

    private void BloquearMovimiento(bool estado)
    {
        if (playerController != null)
        {
            playerController.enabled = !estado; // Si estado es true, desactiva el movimiento, si es false, lo activa
        }
        else
        {
            Debug.LogWarning("PlayerController no está asignado en el inspector.");
        }
    }
public void CambiarVista(int indice, bool bloquearMovimiento)
{
    if (indice >= 0 && indice < views.Length)
    {
        currentView = views[indice];
    }
    else
    {
        currentView = defaultView; // Vista original si el índice no es válido
    }

    BloquearMovimiento(bloquearMovimiento);
}
}
