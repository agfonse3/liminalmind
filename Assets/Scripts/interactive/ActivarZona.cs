using UnityEngine;
using TMPro;
public class ActivarZona : MonoBehaviour
{

    public MonoBehaviour scriptAActivar; // Script a activar
    public Renderer objetoRenderer; // Renderer del objeto con múltiples materiales
    public Material materialShader; // Shader/material extra
    public int indiceMaterial = 1; // Índice del material a modificar (en este caso el 1 material)
    public GameObject panelUI; // Panel de UI que muestra el mensaje
    public TextMeshProUGUI textoMensaje; // Texto dentro del panel
    public string mensaje = "Presiona la E para interactuar"; // Texto a mostrar
    private Material[] materialesOriginales; // Guardamos los materiales originales
    

    private void Start()
    {
        if (objetoRenderer != null)
        {
            materialesOriginales = objetoRenderer.materials; // Guardamos los materiales iniciales
        }
        if (panelUI != null)
        {
            panelUI.SetActive(false); // Aseguramos que el panel esté oculto al inicio
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador ha entrado: " + other.transform.position);
            scriptAActivar.enabled = true; // Activa el script
            ActivarShader(true);
            MostrarTexto(true);
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
            MostrarTexto(false);
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
    private void MostrarTexto(bool activar)
    {
        if (panelUI != null)
        {
            textoMensaje.text = mensaje; // Establece el mensaje
            panelUI.SetActive(activar); // Activa o desactiva el panel
        }
    }
}
