using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
   private NavMeshAgent agent;
    private Animator anim;
    // Dentro de la clase NpcControl
public int spawnZoneID; // Para que el SpawnManager sepa de qué zona es este enemigo
    public float rango = 10f;
    public float tiempoEspera = 2f;
    public float quieto = 0.5f;
    
    private bool esperando = false;

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
            agent.isStopped = false;
            agent.velocity = Vector3.zero;
            agent.ResetPath();
        }
        esperando = false;
        
        // Iniciar la corrutina de configuración aquí.
        // Esto es crucial porque el agente necesita un momento para reconocer el NavMesh.
        StartCoroutine(WaitForNavMeshAgentAndConfigure()); 
    }

    // Eliminamos la llamada a moverAlPunto() en Start(), ya que OnEnable lo maneja.
    // void Start() { } 

    void Update()
    {
        if (!esperando && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (Random.value < quieto)
            {
                StartCoroutine(esperaYMueve());
            }
            else
            {
                moverAlPunto();
            }
        }
        else
        {
            // Solo actualiza la animación si el agente está activo y en el NavMesh
            if (agent.isOnNavMesh && agent.enabled) // Añadida verificación para isOnNavMesh y enabled
            {
                 anim.SetBool("walk", agent.velocity.sqrMagnitude > 0.1f);
            }
            else
            {
                 anim.SetBool("walk", false); // Si no está en NavMesh, no debería walkr
            }
        }
    }

    void moverAlPunto()
    {
        // Esta línea 89 es la que estaba dando el error
        // Asegúrate de que solo se llama si el agente está listo.
        if (!agent.isOnNavMesh || !agent.enabled || agent.isStopped) 
        {
            // Opcional: Debug.Log("NpcControl: Agente no listo para SetDestination.");
            anim.SetBool("walk", false);
            return; 
        }

        Vector3 randomDirection = Random.insideUnitSphere * rango;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, rango, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            anim.SetBool("walk", true);
        }
        else
        {
            Debug.LogWarning("NpcControl: No se encontró una posición válida en NavMesh cerca de " + randomDirection);
            anim.SetBool("walk", false);
            // Podrías intentar buscar un nuevo punto de spawn o simplemente esperar.
            StartCoroutine(esperaYMueve()); // Espera y luego intenta de nuevo.
        }
    }

    IEnumerator esperaYMueve()
    {
        esperando = true;
        anim.SetBool("walk", false);
        yield return new WaitForSeconds(tiempoEspera);
        
        // Antes de moverAlPunto, asegura que el agente esté listo.
        yield return StartCoroutine(WaitForNavMeshAgentAndConfigure()); // Espera de nuevo si no está listo
        moverAlPunto();
        esperando = false;
    }

    // NUEVA CORRUTINA: Espera hasta que el NavMeshAgent esté habilitado y en el NavMesh
    IEnumerator WaitForNavMeshAgentAndConfigure()
    {
        // Espera un frame para que Unity procese la activación y el posicionamiento en el NavMesh
        yield return null; 

        // Espera hasta que el agente esté habilitado y reconozca que está en el NavMesh
        // Esto puede tardar un poco después de que el GameObject se activa o se mueve.
        while (agent != null && (!agent.isOnNavMesh || !agent.enabled))
        {
            // Opcional: Debug.Log("NpcControl: Esperando que el NavMeshAgent esté en el NavMesh...");
            yield return null; 
        }

        // Una vez que el agente está listo, puedes iniciar el movimiento.
        // Solo llama a moverAlPunto() si no estamos ya esperando un nuevo destino desde esperaYMueve.
        // Si no tienes la lógica de 'esperando', puedes llamar a moverAlPunto() aquí.
        // Sin embargo, si 'moverAlPunto()' ya es llamado en OnEnable/Start, este extra puede no ser necesario.
        // La clave es que el *primer* SetDestination se haga de forma segura.
    }

    // MÉTODO 'ConfigurarMovimiento' REQUERIDO POR SPAWNMANAGER
    public void ConfigurarMovimiento(Collider zona)
    {
        // Este método simplemente sirve como un gancho para que SpawnManager lo llame.
        // La lógica de inicio del movimiento ya está en OnEnable con WaitForNavMeshAgentAndConfigure.
        // Puedes poner un log aquí si quieres confirmar que se llama.
        // Debug.Log("NpcControl: ConfigurarMovimiento llamado por SpawnManager.");
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
    /*
    private NavMeshAgent agente;
    private Collider zonaSpawn; // Variable para la zona de spawn (el collider)
    private bool isInitialized = false; 
    private Animator animator; // Referencia al Animator

    void Awake() 
    {
        agente = GetComponent<NavMeshAgent>();
        if (agente == null)
        {
            Debug.LogWarning("NavMeshAgent no encontrado en " + gameObject.name + ". Agregándolo.");
            agente = gameObject.AddComponent<NavMeshAgent>();
        }
        
        animator = GetComponent<Animator>(); 
        if (animator == null)
        {
            Debug.LogWarning("Animator no encontrado en " + gameObject.name + ". Las animaciones no funcionarán.");
        }
    }

    void OnEnable() 
    {
        if (agente != null) 
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

        zonaSpawn = zona; // Aquí se asigna el Collider recibido
        StartCoroutine(WaitForNavMeshAgentAndConfigure());
    }

    private IEnumerator WaitForNavMeshAgentAndConfigure()
    {
        float timeout = 10f; 
        float startTime = Time.time;

        while ((agente == null || !agente.enabled || !agente.isOnNavMesh) && (Time.time - startTime < timeout))
        {
            Debug.Log(gameObject.name + " esperando NavMeshAgent para estar listo... (tiempo transcurrido: " + (Time.time - startTime).ToString("F2") + "s)");
            yield return null; 
        }

        if (agente != null && agente.enabled && agente.isOnNavMesh)
        {
            Debug.Log(gameObject.name + " NavMeshAgent listo. Iniciando movimiento.");
            CambiarDestino();
            if (!isInitialized)
            {
                StartCoroutine(Patrullar());
                isInitialized = true;
            }
        }
        else
        {
            Debug.LogError(gameObject.name + " NavMeshAgent NO está listo después de esperar. Desactivando. " +
                             "Habilitado: " + (agente != null ? agente.enabled.ToString() : "N/A") + 
                             ", En NavMesh: " + (agente != null ? agente.isOnNavMesh.ToString() : "N/A"));
            gameObject.SetActive(false); 
        }
    }

    void CambiarDestino()
    {
        Vector3 nuevoDestino = GenerarPosicionNavMesh();
        if (nuevoDestino != Vector3.zero && agente != null && agente.enabled && agente.isOnNavMesh)
        {
            Debug.Log(gameObject.name + " se mueve a " + nuevoDestino);
            agente.SetDestination(nuevoDestino);
            
            if (animator != null)
            {
                float speed = agente.velocity.magnitude / agente.speed; 
                animator.SetFloat("Speed", speed); 
            }
        }
        else
        {
            Debug.LogWarning(gameObject.name + " no puede establecer un nuevo destino. Motivo: " + 
                             (nuevoDestino == Vector3.zero ? "Destino inválido." : "") +
                             (agente == null ? "Agente nulo." : "") +
                             (!agente.enabled ? "Agente deshabilitado." : "") +
                             (!agente.isOnNavMesh ? "Agente fuera de NavMesh." : ""));
            
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
        float searchRadius = 5f; // Valor recomendado, ajusta si es necesario
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
            if (agente == null || !agente.enabled || !agente.isOnNavMesh)
            {
                Debug.LogWarning(gameObject.name + " NavMeshAgent no está listo en Patrullar, saliendo del ciclo.");
                if (animator != null) animator.SetFloat("Speed", 0f);
                yield break; 
            }

            while (agente != null && agente.enabled && agente.isOnNavMesh && 
                   (agente.pathPending || (agente.remainingDistance > agente.stoppingDistance + 0.1f && !agente.isStopped)))
            {
                if (animator != null)
                {
                    float speed = agente.velocity.magnitude / agente.speed; 
                    animator.SetFloat("Speed", speed); 
                }
                yield return null; 
            }

            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }

            if (agente != null && agente.enabled && agente.isOnNavMesh && agente.remainingDistance <= agente.stoppingDistance + 0.1f)
            {
                yield return new WaitForSeconds(Random.Range(3f, 6f)); 
                CambiarDestino();
            }
            else
            {
                Debug.LogWarning(gameObject.name + " Agente detenido por motivos inesperados. Intentando nuevo destino.");
                yield return new WaitForSeconds(1f); 
                CambiarDestino();
            }
        }
    }

    void OnDrawGizmos()
    {
        if (agente != null && agente.hasPath)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(agente.destination, 0.3f); 

            Vector3[] pathCorners = agente.path.corners;
            for (int i = 0; i < pathCorners.Length - 1; i++)
            {
                Gizmos.DrawLine(pathCorners[i], pathCorners[i + 1]);
                Gizmos.DrawSphere(pathCorners[i], 0.1f); 
            }
        }
        else if (agente != null && !agente.isOnNavMesh)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f); 
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2f); 
        }
    }*/
}