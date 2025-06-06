using UnityEngine;

public class CameraManager : MonoBehaviour
{
     public Transform[] views;
    public float transitionSpeed;
    private Transform currentView;
    private Transform defaultView;

    public FirstPersonController playerController; // Referencia al jugador
    public bool camaraFinalizada = false; // Indica si la cámara ya terminó su movimiento

    void Start()
    {
        defaultView = transform; // Guarda la vista original
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
