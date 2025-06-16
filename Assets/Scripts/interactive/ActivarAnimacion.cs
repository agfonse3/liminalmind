using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
        if (cameraManager != null && cameraManager.teclaEPulsada) // Solo si E ha sido presionado
        {
            foreach (var evento in eventos)
            {
                if (Input.GetKeyDown(evento.tecla) && cameraManager.camaraFinalizada)
                {
                    Debug.Log("Tecla presionada después de E: " + evento.tecla);
                    ActivarAnimacionYShader(evento.nombreAnimacion, evento.objetoRenderer);
                    escenaSeleccionada = evento.indiceEscena;
                    Invoke(nameof(CambiarEscena), 2f);
                    break;
                }
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
            if (escenaSeleccionada == 1) 
            {
                GameManager.Instance.setLastPosition();
                GameManager.Instance.GoToFirstFloor();
            }
            if (escenaSeleccionada == 2)
            {
                GameManager.Instance.setLastPosition();
                GameManager.Instance.GoToSecondFloor();
            }
        }
        else
        {
            Debug.LogWarning("No se ha seleccionado una escena válida.");
        }
    }
}
