using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour // Cambiado de NpcControl a EnemyAI
{
    private NavMeshAgent agent;
    private Animator anim;
    
    // Para que el SpawnManager sepa de qué zona es este enemigo
    public int spawnZoneID; 

    public float rango = 10f; // Rango para buscar un nuevo punto de destino
    public float tiempoEspera = 2f; // Tiempo que el NPC espera antes de moverse de nuevo
    [Range(0f, 1f)] // Restringe el valor en el Inspector entre 0 y 1
    public float probabilidadQuieto = 0.5f; // Probabilidad de quedarse quieto en lugar de moverse

    private bool esperando = false; // Controla si el NPC está en estado de espera

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogWarning("NavMeshAgent no encontrado en " + gameObject.name + ". Agregándolo.");
            agent = gameObject.AddComponent<NavMeshAgent>();
        }
        
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning("Animator no encontrado en " + gameObject.name + ". Las animaciones no funcionarán.");
        }
    }

    void OnEnable() // Se llama cada vez que el objeto se activa (ideal para el pooling)
    {
        if (agent != null)
        {
            agent.isStopped = false; // Asegura que el agente no esté detenido
            agent.velocity = Vector3.zero; // Resetea la velocidad
            agent.ResetPath(); // Borra cualquier camino previo
        }
        esperando = false; // Reinicia el estado de espera
        
        // Iniciar la corrutina de configuración y movimiento principal
        StartCoroutine(WaitForNavMeshAgentAndStartPatrol()); 
    }

    void Update()
    {
        // Solo ejecutar la lógica de movimiento si el agente está listo y no está esperando
        if (agent != null && agent.enabled && agent.isOnNavMesh && !esperando)
        {
            // Si no está calculando un camino y ha llegado (o está muy cerca) de su destino
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                // Decide si el NPC se queda quieto o busca un nuevo punto
                if (Random.value < probabilidadQuieto)
                {
                    StartCoroutine(esperaYMueve()); // Entra en estado de espera (idle)
                }
                else
                {
                    moverAlPunto(); // Busca un nuevo punto
                }
            }
            
            // Actualiza el parámetro "walk" del Animator basado en la velocidad del agente
            anim.SetBool("walk", agent.velocity.sqrMagnitude > 0.1f);
        }
        else // Si el agente no está listo o está esperando
        {
            anim.SetBool("walk", false); // Siempre desactiva la animación de caminar
        }
    }

    void moverAlPunto()
    {
        // Protección extra: asegurar que el agente esté listo antes de llamar a SetDestination
        // Si no está listo, la corrutina WaitForNavMeshAgentAndStartPatrol lo manejará.
        if (agent == null || !agent.isOnNavMesh || !agent.enabled || esperando) 
        {
            // Debug.Log("EnemyAI: Agente no listo o esperando. No se puede establecer destino.");
            anim.SetBool("walk", false);
            return; 
        }

        Vector3 randomDirection = Random.insideUnitSphere * rango;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, rango, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            // La animación se activa en Update() si el agente se mueve
        }
        else
        {
            Debug.LogWarning("EnemyAI: No se encontró una posición válida en NavMesh cerca de " + randomDirection);
            anim.SetBool("walk", false);
            // Si no se encuentra un punto, espera y luego intenta de nuevo.
            StartCoroutine(esperaYMueve()); 
        }
    }

    IEnumerator esperaYMueve()
{
    esperando = true; // Marcar el estado de espera
    agent.isStopped = true; // <--- ESTO ES CRUCIAL: Detener el NavMeshAgent
    anim.SetBool("walk", false); // Asegurarse de que la animación de caminar esté desactivada

    yield return new WaitForSeconds(tiempoEspera); // Esperar el tiempo definido

    agent.isStopped = false; // Reanudar el movimiento del NavMeshAgent
    esperando = false; // Salir del estado de espera
    
    moverAlPunto(); // Después de esperar, intenta moverte de nuevo
}

    // CORRUTINA PRINCIPAL PARA INICIAR MOVIMIENTO DESPUÉS DE LA ACTIVACIÓN/SPAWN
    IEnumerator WaitForNavMeshAgentAndStartPatrol()
    {
        // Espera un frame para que Unity procese la activación y el posicionamiento.
        yield return null; 

        // Bucle para esperar hasta que el agente esté realmente en el NavMesh y habilitado
        // y no esté esperando (si ya lo activamos desde otra corrutina como esperaYMueve)
        while (agent != null && (!agent.isOnNavMesh || !agent.enabled || esperando))
        {
            //Debug.Log("EnemyAI: Esperando que el NavMeshAgent esté en el NavMesh o no esté esperando...");
            yield return null; 
        }
        
        // Una vez que el agente está listo y no en estado de espera, iniciar el movimiento
        if (agent != null && agent.isOnNavMesh && agent.enabled && !esperando)
        {
            moverAlPunto();
        }
    }

    // MÉTODO 'ConfigurarMovimiento' LLAMADO POR SPAWNMANAGER
    public void ConfigurarMovimiento(Collider zona)
    {
        // Este método sirve como un gancho para que SpawnManager lo llame.
        // La lógica de inicio de movimiento real se maneja en OnEnable() a través de la corrutina.
        // Aseguramos que el agente esté habilitado aquí si es necesario, aunque OnEnable ya lo hace.
        if (agent != null)
        {
            agent.enabled = true; // Asegúrate de que esté habilitado si fue deshabilitado previamente.
        }
    }

    void OnDrawGizmosSelected()
    {
        if (agent != null && agent.hasPath)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(agent.destination, 0.3f); 

            Vector3[] pathCorners = agent.path.corners;
            for (int i = 0; i < pathCorners.Length - 1; i++)
            {
                Gizmos.DrawLine(pathCorners[i], pathCorners[i + 1]);
                Gizmos.DrawSphere(pathCorners[i], 0.1f); 
            }
        }
        else if (agent != null && !agent.isOnNavMesh)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f); 
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2f); 
        }
    }
}