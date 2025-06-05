using UnityEngine;

public class ActivarZona : MonoBehaviour
{
    public MonoBehaviour scriptAActivar; // Referencia al script que quieres activar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Si el jugador entra en la zona
        {
            scriptAActivar.enabled = true; // Activa el script
            Debug.Log("Script activado porque el jugador entró en la zona.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Si el jugador sale de la zona
        {
            scriptAActivar.enabled = false; // Desactiva el script
            Debug.Log("Script desactivado porque el jugador salió de la zona.");
        }
    }
}
