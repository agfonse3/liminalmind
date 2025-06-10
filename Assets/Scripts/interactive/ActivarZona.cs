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
    public bool jugadorDentro = false;

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
        jugadorDentro = true; // Marcamos que el jugador está en la zona
        scriptAActivar.enabled = true;
        ActivarShader(true);
        MostrarTexto(true);
        
    }
}

private void OnTriggerExit(Collider other)
{
    if (other.CompareTag("Player"))
    {
        jugadorDentro = false; // El jugador salió de la zona
        scriptAActivar.enabled = false;
        ActivarShader(false);
        MostrarTexto(false);
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
