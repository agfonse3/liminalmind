using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class EventoTecla
{
    public KeyCode tecla = KeyCode.Alpha1;
    public string nombreAnimacion;
    public Renderer objetoRenderer;
    public int indiceEscena;
}

public class ActivarAnimacion : MonoBehaviour
{
    public Animator animator;
    public List<EventoTecla> eventos;
    public Material shaderMaterial;
    public int indiceMaterial = 1;
    public CameraManager cameraManager;

    private int escenaSeleccionada = -1;

    void Start()
    {
        escenaSeleccionada = -1;

        if (cameraManager != null)
        {
            cameraManager.teclaEPulsada = false;
            cameraManager.camaraFinalizada = false;
        }
    }

    void Update()
    {
        if (cameraManager != null && cameraManager.teclaEPulsada) 
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
            if (GameManager.Instance != null)
            {
                GameManager.Instance.setLastPosition();

                if (escenaSeleccionada == 1)
                    GameManager.Instance.GoToFirstFloor();
                else if (escenaSeleccionada == 2)
                    GameManager.Instance.GoToSecondFloor();
            }
        }
        else
        {
            Debug.LogWarning("No se ha seleccionado una escena válida.");
        }
    }
}
