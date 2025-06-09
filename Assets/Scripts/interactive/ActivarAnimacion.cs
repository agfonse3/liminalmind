using UnityEngine;
using UnityEngine.SceneManagement;
public class ActivarAnimacion : MonoBehaviour
{
    public Animator animator; // Referencia al Animator
    public string nombreAnimacion = "boton1"; // Nombre de la animación

    public Renderer objetoRenderer; // Renderer del objeto
    public Material materialActivado; // Shader/material especial
    public int indiceMaterial = 1; // Índice del material en el renderer

    public string nombreEscena = "Ultemp"; // Nombre de la escena a cargar

    public CameraManager cameraManager; // Referencia al CameraManager

    private Material[] materialesOriginales;

    void Start()
    {
        if (objetoRenderer != null)
        {
            materialesOriginales = objetoRenderer.materials;
        }
    }

    void Update()
    {
        // Solo ejecuta si la cámara ha terminado su transición
        if (Input.GetKeyDown(KeyCode.Alpha1) && cameraManager.camaraFinalizada)
        {
            ActivarAnimacionYShader();
            Invoke("CambiarEscena", 2f); // Retrasa el cambio de escena 2 segundos
        }
    }

    void ActivarAnimacionYShader()
    {
        // Activar animación
        if (animator != null)
        {
            animator.Play(nombreAnimacion);
            Debug.Log("Animación activada.");
        }

        // Activar shader
        if (objetoRenderer != null)
        {
            Material[] materiales = objetoRenderer.materials;
            if (indiceMaterial >= 0 && indiceMaterial < materiales.Length)
            {
                materiales[indiceMaterial] = materialActivado;
                objetoRenderer.materials = materiales;
                Debug.Log("Shader activado.");
            }
            else
            {
                Debug.LogError("Índice de material fuera de rango.");
            }
        }
    }

    void CambiarEscena()
    {
        Debug.Log("Cambiando de escena en 2 segundos...");
        SceneManager.LoadScene(0); // Carga la nueva escena después de la animación
    }
}
