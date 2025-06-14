using UnityEngine;

public class BotonInteractivo : MonoBehaviour
{
    public Animator botonAnimator;           // Animator del botón
    public Animator puertasAnimator;         // Animator de las puertas

    public string animacionBoton = "botonarriba";   // Nombre de la animación del botón
    public string animacionPuertas = "puertas";     // Nombre de la animación de las puertas
    public float delaySegundaAnimacion = 2f;        // Tiempo de espera antes de abrir la puerta

    public ActivarZona zonaDeActivacion;     // Referencia a la zona donde debe estar el jugador

    void Update()
    {
        // Al presionar E dentro de la zona, se activa la animación del botón
        if (Input.GetKeyDown(KeyCode.E) && zonaDeActivacion.jugadorDentro)
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

        // Esperar para reproducir la animación de la puerta
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
