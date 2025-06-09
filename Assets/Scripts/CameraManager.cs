using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform[] views;
    public float transitionSpeed;
    private Transform currentView;
    private Transform defaultView;

    public FirstPersonController playerController; // Referencia al jugador
    public bool camaraFinalizada = false; // Indica si la cámara ya terminó su movimiento
    public bool teclaEPulsada = false; // Indica si la tecla E ha sido presionada

    void Start()
    {
        defaultView = transform; // Guarda la vista original
        currentView = defaultView;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && EncuentraZonaDeActivacion())
    {
        teclaEPulsada = true;
        currentView = views[0];
        BloquearMovimiento(true);
    }

    if (Input.GetKeyDown(KeyCode.M))
    {
        BloquearMovimiento(false);
        currentView = defaultView;
        teclaEPulsada = false;
    }
    }
private bool EncuentraZonaDeActivacion()
{
    return FindFirstObjectByType<ActivarZona>().jugadorDentro;
// Verifica si el jugador está dentro
}
    void LateUpdate()
    {
        if (currentView != null)
        {
            transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, currentView.rotation, Time.deltaTime * transitionSpeed);

            // Detecta cuando la cámara ha llegado a su destino
            if (Vector3.Distance(transform.position, currentView.position) < 0.1f)
            {
                camaraFinalizada = true;
            }
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
            currentView = defaultView;
        }

        BloquearMovimiento(bloquearMovimiento);
    }

    private void BloquearMovimiento(bool estado)
    {
        if (playerController != null)
        {
            playerController.enabled = !estado;
        }
    }
}
