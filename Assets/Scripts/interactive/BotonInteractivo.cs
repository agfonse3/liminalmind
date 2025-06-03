using UnityEngine;


public class BotonInteractivo : MonoBehaviour
{
    private Animator animator;
    private Transform jugador;
    public float distanciaActivacion = 3f; // Distancia máxima para activar la animación

    void Start()
    {
        animator = GetComponent<Animator>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform; // Buscar al jugador
        animator.SetBool("Presionado", false); // Asegurar que inicia desactivado
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(jugador.position, transform.position); // Calcular distancia
        Debug.Log("Distancia al botón: " + distancia);

        if (distancia <= distanciaActivacion) // Si el jugador está dentro del rango
        {
            if (Input.GetKeyDown(KeyCode.F)) // Si presiona "F"
            {
                Debug.Log("Jugador presionó F cerca del botón.");
                animator.SetBool("Presionado", true); // Activar animación
            }
        }
    }
}
