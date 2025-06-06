using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// Clase auxiliar que se mostrará en el Inspector
[System.Serializable]
public class EventoTecla
{
    public KeyCode tecla = KeyCode.Alpha1;       // Tecla que activa el evento
    public string nombreAnimacion;               // Nombre de la animación a reproducir
    public Renderer objetoRenderer;              // Objeto al que se le aplica el shader
    public int indiceEscena;                     // Índice de la escena a cargar
}

public class ActivarAnimacion : MonoBehaviour
{
    public Animator animator;                    // Animator principal
    public List<EventoTecla> eventos;            // Lista de eventos configurables
    public Material shaderMaterial;              // Shader/material compartido para todos
    public int indiceMaterial = 1;               // Índice del material a reemplazar
    public CameraManager cameraManager;          // Referencia opcional al CameraManager

    private int escenaSeleccionada = -1;

    void Update()
    {
        foreach (var evento in eventos)
        {
            // Verifica si se presionó la tecla configurada y si la cámara terminó su transición
            if (Input.GetKeyDown(evento.tecla) && (cameraManager == null || cameraManager.camaraFinalizada))
            {
                Debug.Log("Tecla presionada: " + evento.tecla);
                ActivarAnimacionYShader(evento.nombreAnimacion, evento.objetoRenderer);
                escenaSeleccionada = evento.indiceEscena;
                Invoke(nameof(CambiarEscena), 2f); // Espera 2 segundos para cambiar de escena
                break;
            }
        }
    }

    void ActivarAnimacionYShader(string animacion, Renderer targetRenderer)
    {
        if (animator != null && !string.IsNullOrEmpty(animacion))
        {
            animator.Play(animacion);
            Debug.Log("Animación reproducida: " + animacion);
        }

        if (targetRenderer != null)
        {
            Material[] materiales = targetRenderer.materials;
            if (indiceMaterial >= 0 && indiceMaterial < materiales.Length)
            {
                materiales[indiceMaterial] = shaderMaterial;
                targetRenderer.materials = materiales;
                Debug.Log("Shader aplicado al objeto: " + targetRenderer.name);
            }
            else
            {
                Debug.LogError("Índice de material fuera de rango en objeto: " + targetRenderer.name);
            }
        }
    }

    void CambiarEscena()
    {
        if (escenaSeleccionada >= 0)
        {
            Debug.Log("Cambiando a escena con índice: " + escenaSeleccionada);
            SceneManager.LoadScene(escenaSeleccionada);
        }
        else
        {
            Debug.LogWarning("No se ha seleccionado una escena válida.");
        }
    }
}
