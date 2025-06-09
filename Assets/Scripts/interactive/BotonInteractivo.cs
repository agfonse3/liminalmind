using UnityEngine;


public class BotonInteractivo : MonoBehaviour
{  
    
    public Animator botonAnimator;   // Animator del botón
    public Animator puertasAnimator; // Animator de las puertas

    public string animacionBoton = "botonarriba"; // Animación del botón
    public string animacionPuertas = "puertas";   // Animación de las puertas
    public float delaySegundaAnimacion = 2f; // Tiempo de espera antes de reproducir la segunda animación
 // ¡Referencia a la ActivarZona específica que controla este botón!
    public ActivarZona zonaDeActivacion; 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&& zonaDeActivacion.jugadorDentro) // Al presionar E, se activan las animaciones en orden
        {
            ReproducirAnimaciones();
        }
    }

    void ReproducirAnimaciones()
    {
        if (botonAnimator != null)
        {
            botonAnimator.Play(animacionBoton);
            Debug.Log("Animación del botón reproducida: " + animacionBoton);
        }
        else
        {
            Debug.LogError("El Animator del botón no está asignado.");
        }

        Invoke(nameof(ReproducirSegundaAnimacion), delaySegundaAnimacion);
    }

    void ReproducirSegundaAnimacion()
    {
        if (puertasAnimator != null)
        {
            puertasAnimator.Play(animacionPuertas);
            Debug.Log("Animación de las puertas reproducida: " + animacionPuertas);
        }
        else
        {
            Debug.LogError("El Animator de las puertas no está asignado.");
        }
    }
}
