using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agente;
    private Collider zonaSpawn;
    private bool isInitialized = false; 
    private Animator animator; // Referencia al Animator

    void Awake() 
    {
        agente = GetComponent<NavMeshAgent>();
        if (agente == null)
        {
            Debug.LogError("NavMeshAgent no encontrado en " + gameObject.name + ". Agregándolo.");
            agente = gameObject.AddComponent<NavMeshAgent>();
        }
        
        animator = GetComponent<Animator>(); // Obtener el Animator
        if (animator == null)
        {
            Debug.LogWarning("Animator no encontrado en " + gameObject.name + ". Las animaciones no funcionarán.");
        }
    }

    void OnEnable() 
    {
        if (agente != null && agente.isOnNavMesh) 
        {
            agente.isStopped = false; 
            agente.velocity = Vector3.zero; 
            agente.ResetPath(); 
        }
        isInitialized = false; 
    }

    public void ConfigurarMovimiento(Collider zona)
    {
        if (zona == null)
        {
            Debug.LogError("Zona de spawn no asignada en " + gameObject.name);
            return;
        }

        zonaSpawn = zona;

        if (agente != null && agente.enabled && agente.isOnNavMesh)
        {
            CambiarDestino();
            if (!isInitialized)
            {
                StartCoroutine(Patrullar());
                isInitialized = true;
            }
        }
        else if (agente != null)
        {
            Debug.LogWarning(gameObject.name + " NavMeshAgent no está listo para navegar. Está habilitado: " + agente.enabled + ", En NavMesh: " + agente.isOnNavMesh);
            StartCoroutine(RetryConfiguringMovement(zona));
        }
    }

    IEnumerator RetryConfiguringMovement(Collider zona)
    {
        yield return null; 
        if (agente != null && agente.enabled && agente.isOnNavMesh)
        {
            ConfigurarMovimiento(zona); 
        } else {
            Debug.LogError(gameObject.name + " NavMeshAgent todavía no está listo después de un reintento.");
        }
    }

    void CambiarDestino()
    {
        Vector3 nuevoDestino = GenerarPosicionNavMesh();
        if (nuevoDestino != Vector3.zero && agente != null && agente.enabled && agente.isOnNavMesh)
        {
            Debug.Log(gameObject.name + " se mueve a " + nuevoDestino);
            agente.SetDestination(nuevoDestino);
            // Actualizar la animación de velocidad
            if (animator != null)
            {
                // Un pequeño valor para evitar vibraciones cuando la velocidad es casi cero
                float speed = agente.velocity.magnitude / agente.speed; // Normaliza la velocidad a un rango de 0 a 1
                animator.SetFloat("Speed", speed); // O el nombre de tu parámetro de velocidad
            }
        }
        else
        {
            Debug.LogWarning(gameObject.name + " no puede establecer un nuevo destino.");
            // Si no puede establecer destino, asegurarse de que la animación de movimiento se detenga
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }
        }
    }

    Vector3 GenerarPosicionNavMesh()
    {
        if (zonaSpawn == null)
        {
            Debug.LogError("zonaSpawn es nula al generar posición NavMesh para " + gameObject.name);
            return Vector3.zero;
        }

        Vector3 randomPointInBounds = new Vector3(
            Random.Range(zonaSpawn.bounds.min.x, zonaSpawn.bounds.max.x),
            zonaSpawn.bounds.center.y, 
            Random.Range(zonaSpawn.bounds.min.z, zonaSpawn.bounds.max.z)
        );

        NavMeshHit hit;
        float searchRadius = 20f; 
        if (NavMesh.SamplePosition(randomPointInBounds, out hit, searchRadius, NavMesh.AllAreas))
        {
            return hit.position; 
        }

        Debug.LogWarning(gameObject.name + " NO encontró posición válida en la NavMesh cerca de " + randomPointInBounds + " en zona " + zonaSpawn.name);
        return Vector3.zero; 
    }

    IEnumerator Patrullar()
    {
        while (gameObject.activeInHierarchy) 
        {
            // Esperar a que el agente llegue a su destino o se quede sin un path
            while (agente != null && agente.enabled && agente.isOnNavMesh && agente.pathPending || (agente.remainingDistance > agente.stoppingDistance && !agente.isStopped))
            {
                // Actualizar la velocidad del Animator mientras el agente se mueve
                if (animator != null)
                {
                    float speed = agente.velocity.magnitude / agente.speed; // Normaliza la velocidad
                    animator.SetFloat("Speed", speed); 
                }
                yield return null; 
            }

            // Una vez que el agente ha llegado o se ha detenido
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f); // Detener la animación de movimiento
            }

            if (agente == null || !agente.enabled || !agente.isOnNavMesh)
            {
                Debug.LogWarning(gameObject.name + " NavMeshAgent no está listo, saliendo de Patrullar.");
                yield break;
            }

            yield return new WaitForSeconds(Random.Range(3f, 6f)); 
            CambiarDestino();
        }
    }

    // Opcional: Para manejar animaciones de giro si tu personaje necesita girar suavemente
    // void Update() 
    // {
    //     if (agente != null && agente.enabled && agente.isOnNavMesh)
    //     {
    //         Vector3 horizontalVelocity = new Vector3(agente.velocity.x, 0, agente.velocity.z);
    //         float currentSpeed = horizontalVelocity.magnitude;
    //         if (animator != null)
    //         {
    //             animator.SetFloat("Speed", currentSpeed / agente.speed); // Normaliza la velocidad
    //             // Podrías añadir un parámetro para la velocidad angular si tu Animator lo requiere para giros
    //             // animator.SetFloat("AngularSpeed", agente.angularSpeed);
    //         }
    //     }
    // }
}