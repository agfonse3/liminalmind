using UnityEngine;


public class BotonInteractivo : MonoBehaviour
{
     public CameraManager cameraManager; // Referencia al script de cámara
    public Transform objetoInteractivo;
    public float distanciaCambioVista = 3f;
    public int indiceVista; // Índice de la vista a activar

    private Transform jugador;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform; // Asegúrate de que tu jugador tenga el tag correcto
    }

    void Update()
    {
        if (jugador == null || cameraManager == null) return;

        float distancia = Vector3.Distance(jugador.position, objetoInteractivo.position);

        if (distancia <= distanciaCambioVista)
        {
            cameraManager.CambiarVista(indiceVista, true); // Activa la vista
        }
        else
        {
            cameraManager.CambiarVista(-1, false); // Regresa la cámara a la vista normal
        }
    }
}
