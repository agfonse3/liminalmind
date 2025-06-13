// EnemyAI2.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI2 : MonoBehaviour
{
    private Transform[] puntosPatrulla;
    private int puntoActual = 0;
    private NavMeshAgent agenteNavMesh;
    private Animator animator;
    public float velocidadMovimiento = 3.5f;

    // Referencia al SpawnManager para liberar la zona
    private SpawnManager2 spawnManager;
    private int zonaAsignadaIndex = -1;

    void Awake() // Cambiado de Start a Awake
    {
        agenteNavMesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agenteNavMesh == null)
        {
            Debug.LogError("EnemyAI2: No se encontró el componente NavMeshAgent en el objeto.");
        }
        if (animator == null)
        {
            Debug.LogWarning("EnemyAI2: No se encontró el componente Animator en el objeto.");
        }

        // Buscar el SpawnManager en la escena
        spawnManager = FindObjectOfType<SpawnManager2>();

        
        if (spawnManager == null)
        {
            Debug.LogError("EnemyAI2: No se encontró una instancia de SpawnManager2 en la escena.");
        }
    }

    // Este método ahora recibe el índice de la zona para poder liberarla
    public void AsignarPuntosDePatrulla(Transform[] puntos, int zonaIndex = -1)
    {
        if (puntos == null || puntos.Length == 0)
        {
            Debug.LogError("EnemyAI2: Los puntos de patrullaje recibidos están vacíos o son nulos.");
            this.gameObject.SetActive(false); // Desactiva el enemigo si no hay puntos
            return;
        }

        puntosPatrulla = puntos;
        zonaAsignadaIndex = zonaIndex; // Guarda el índice de la zona
        puntoActual = 0; // Asegura que empiece en el primer punto
        MoverAlSiguientePunto();
    }

    void Update()
    {
        if (agenteNavMesh == null || !agenteNavMesh.isOnNavMesh)
        {
            // Debug.LogWarning("EnemyAI2: NavMeshAgent no inicializado o fuera de NavMesh. Esperando...");
            return;
        }

        // Asegúrate de que el animator no sea nulo antes de usarlo
        if (animator != null)
        {
            animator.SetFloat("Velocidad", agenteNavMesh.velocity.magnitude);
        }

        if (!agenteNavMesh.pathPending && agenteNavMesh.remainingDistance < 0.5f && agenteNavMesh.remainingDistance != 0)
        {
            agenteNavMesh.speed = 0f;
            if (animator != null)
            {
                animator.SetFloat("Velocidad", 0f); // Asegura que la animación de velocidad sea 0
            }
            StartCoroutine(EsperarYContinuar(2f));
        }
    }

    IEnumerator EsperarYContinuar(float tiempoEspera)
    {
        yield return new WaitForSeconds(tiempoEspera);
        MoverAlSiguientePunto();
    }

    void MoverAlSiguientePunto()
    {
        if (puntosPatrulla == null || puntosPatrulla.Length == 0)
        {
            Debug.LogError("EnemyAI2: No hay puntos de patrulla asignados para mover.");
            this.gameObject.SetActive(false); // Desactiva el enemigo si no hay puntos
            return;
        }

        if (agenteNavMesh == null || !agenteNavMesh.isOnNavMesh)
        {
            Debug.LogWarning("EnemyAI2: NavMeshAgent no está listo para moverse.");
            return;
        }

        puntoActual = (puntoActual + 1) % puntosPatrulla.Length;
        agenteNavMesh.SetDestination(puntosPatrulla[puntoActual].position);
        agenteNavMesh.speed = velocidadMovimiento;
    }

    void OnDisable() // Se llama cuando el GameObject se desactiva (vuelve al pool)
    {
        if (spawnManager != null && zonaAsignadaIndex != -1)
        {
            spawnManager.LiberarZona(zonaAsignadaIndex);
            zonaAsignadaIndex = -1; // Resetear el índice para evitar liberaciones duplicadas
        }
    }
}