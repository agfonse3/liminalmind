using UnityEngine;

public class ActivarZona : MonoBehaviour
{
    
    public MonoBehaviour scriptAActivar; // Script a activar
    public Renderer objetoRenderer; // Renderer del objeto con múltiples materiales
    public Material materialShader; // Shader/material extra
    public int indiceMaterial = 1; // Índice del material a modificar (en este caso el 1 material)

    private Material[] materialesOriginales; // Guardamos los materiales originales

    private void Start()
    {
        if (objetoRenderer != null)
        {
            materialesOriginales = objetoRenderer.materials; // Guardamos los materiales iniciales
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador ha entrado: " + other.transform.position);
            scriptAActivar.enabled = true; // Activa el script
            ActivarShader(true);
            Debug.Log("Shader activado.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador ha salido: " + other.transform.position);
            scriptAActivar.enabled = false; // Desactiva el script
            ActivarShader(false);
            Debug.Log("Shader desactivado.");
        }
    }

    private void ActivarShader(bool activar)
    {
        if (objetoRenderer != null)
        {
            Material[] materiales = objetoRenderer.materials; // Copia los materiales actuales
            materiales[indiceMaterial] = activar ? materialShader : materialesOriginales[indiceMaterial];
            objetoRenderer.materials = materiales;
        }
    }
}
