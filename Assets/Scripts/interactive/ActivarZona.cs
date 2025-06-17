using UnityEngine;
using TMPro;

public class ActivarZona : MonoBehaviour
{
    public MonoBehaviour scriptAActivar; // Script a activar
    public Renderer objetoRenderer; // Renderer del objeto con múltiples materiales
    public Material materialShader; // Shader/material extra
    public int indiceMaterial = 1; // Índice del material a modificar
    public GameObject panelUI; // Panel de UI que muestra el mensaje
    public GameObject textInteractuable;
    private Material[] materialesOriginales;
    public bool jugadorDentro = false;

    private void Start()
    {
        if (objetoRenderer != null)
        {
            materialesOriginales = objetoRenderer.materials;
        }
        if (panelUI != null)
        {
            panelUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            ActivarScript(true);
            ActivarShader(true);
            MostrarTexto(true);

            // 🎧 Sonido de agitación al entrar en la zona
            //AudiomanagerTemp.Instance.PlaySFX(AudiomanagerTemp.Instance.sfxAgitacion);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
            ActivarScript(false);
            ActivarShader(false);
            MostrarTexto(false);
        }
    }

    private void ActivarScript(bool activar)
    {
        if (scriptAActivar != null && scriptAActivar.GetType() != typeof(Interactuable) &&
            scriptAActivar.GetType() != typeof(AbrirPuertaConLlave) &&
            scriptAActivar.GetType() != typeof(AbrirPuertasSencilla))
        {
            scriptAActivar.enabled = activar;
        }
    }

    private void ActivarShader(bool activar)
    {
        if (objetoRenderer != null && materialShader != null && materialesOriginales != null)
        {
            Material[] materiales = objetoRenderer.materials;
            if (indiceMaterial >= 0 && indiceMaterial < materiales.Length)
            {
                materiales[indiceMaterial] = activar ? materialShader : materialesOriginales[indiceMaterial];
                objetoRenderer.materials = materiales;
            }
        }
    }

    private void MostrarTexto(bool activar)
    {
        if (panelUI != null)
        {
            panelUI.SetActive(activar);
        }
        if (textInteractuable != null)
        {
            textInteractuable.SetActive(activar);
        }
    }
}