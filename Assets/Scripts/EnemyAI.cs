using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class EnemyAI : MonoBehaviour
{
  private NavMeshAgent agente;
    private Collider zonaSpawn;
    private bool isInitialized = false; // Flag para asegurar inicialización

    void Awake() // Usar Awake para obtener componentes antes de Start
    {
        agente = GetComponent<NavMeshAgent>();
        if (agente == null)
        {
            Debug.LogError("NavMeshAgent no encontrado en " + gameObject.name + ". Agregándolo.");
            agente = gameObject.AddComponent<NavMeshAgent>();
        }
        // Configura propiedades del NavMeshAgent si son importantes, como velocidad, aceleración, etc.
        // agente.speed = 3.5f;
        // agente.angularSpeed = 120f;
        // agente.acceleration = 8f;
        // agente.stoppingDistance = 0.5f; // Distancia para considerar que ha llegado al destino
    }

    void OnEnable() // Se llama cada vez que el GameObject se activa
    {
        // Resetear el agente al activarse del pool
        if (agente != null && agente.isOnNavMesh) // Asegurarse de que esté en el NavMesh antes de resetear
        {
            agente.isStopped = false; // Asegurarse de que el agente no esté detenido
            agente.velocity = Vector3.zero; // Limpiar velocidad
            agente.ResetPath(); // Limpiar cualquier path anterior
        }
        isInitialized = false; // Reiniciar flag de inicialización
    }

    // `ConfigurarMovimiento` debe llamarse después de que el GameObject se haya activado y posicionado
    public void ConfigurarMovimiento(Collider zona)
    {
        if (zona == null)
        {
            Debug.LogError("Zona de spawn no asignada en " + gameObject.name);
            return;
        }

        zonaSpawn = zona;

        // Asegurarse de que el NavMeshAgent esté habilitado y en el NavMesh antes de intentar establecer un destino
        if (agente != null && agente.enabled && agente.isOnNavMesh)
        {
            CambiarDestino();
            if (!isInitialized) // Solo iniciar la corrutina una vez por activación
            {
                StartCoroutine(Patrullar());
                isInitialized = true;
            }
        }
        else if (agente != null)
        {
            // Esto es crucial: si no está en el NavMesh o no está habilitado, no funcionará.
            // Puede que necesitemos un pequeño retraso o reintentar si el SamplePosition no fue exitoso.
            Debug.LogWarning(gameObject.name + " NavMeshAgent no está listo para navegar. Está habilitado: " + agente.enabled + ", En NavMesh: " + agente.isOnNavMesh);
            // Podrías intentar un pequeño retraso aquí si sabes que el objeto se posiciona justo antes de esta llamada
            // y el NavMeshAgent necesita un frame para registrarse.
            StartCoroutine(RetryConfiguringMovement(zona));
        }
    }

    IEnumerator RetryConfiguringMovement(Collider zona)
    {
        yield return null; // Esperar un frame
        if (agente != null && agente.enabled && agente.isOnNavMesh)
        {
            ConfigurarMovimiento(zona); // Reintentar la configuración
        } else {
            Debug.LogError(gameObject.name + " NavMeshAgent todavía no está listo después de un reintento.");
        }
    }


    void CambiarDestino()
    {
        Vector3 nuevoDestino = GenerarPosicionNavMesh();
        if (nuevoDestino != Vector3.zero && agente != null && agente.enabled && agente.isOnNavMesh)
        {
            agente.SetDestination(nuevoDestino);
        }
        else if (agente != null)
        {
            Debug.LogWarning(gameObject.name + " no puede establecer un nuevo destino. Destino inválido o agente no listo.");
        }
    }

    Vector3 GenerarPosicionNavMesh()
    {
        if (zonaSpawn == null)
        {
            Debug.LogError("zonaSpawn es nula al generar posición NavMesh para " + gameObject.name);
            return Vector3.zero;
        }

        // Generar un punto aleatorio dentro de los límites XZ del Collider
        Vector3 randomPointInBounds = new Vector3(
            Random.Range(zonaSpawn.bounds.min.x, zonaSpawn.bounds.max.x),
            zonaSpawn.bounds.center.y, // Usa el centro Y o un valor razonable como punto de partida
            Random.Range(zonaSpawn.bounds.min.z, zonaSpawn.bounds.max.z)
        );

        // Aumentar el `maxDistance` en `SamplePosition` para buscar en un rango más amplio
        // El segundo parámetro (out hit) es la posición encontrada en el NavMesh
        // El tercer parámetro es la distancia máxima de búsqueda (puede ser grande si la zona es grande)
        // El cuarto parámetro es la máscara de áreas del NavMesh (NavMesh.AllAreas por defecto)
        NavMeshHit hit;
        float searchRadius = 20f; // Puedes ajustar este valor. Debería ser al menos tan grande como tu collider.
        if (NavMesh.SamplePosition(randomPointInBounds, out hit, searchRadius, NavMesh.AllAreas))
        {
            return hit.position; // Retorna la posición válida en la NavMesh
        }

        Debug.LogWarning(gameObject.name + " NO encontró posición válida en la NavMesh cerca de " + randomPointInBounds + " en zona " + zonaSpawn.name);
        return Vector3.zero; // Si no encuentra, retorna Vector3.zero
    }

    IEnumerator Patrullar()
    {
        while (gameObject.activeInHierarchy) // Asegura que la corrutina se detenga si el GameObject se desactiva
        {
            // Esperar a que el agente llegue a su destino o se quede sin un path
            // Una pequeña tolerancia para 'remainingDistance' es buena para evitar problemas de flotantes
           while (agente != null && agente.enabled && agente.isOnNavMesh && agente.pathPending || (agente.remainingDistance > agente.stoppingDistance && !agente.isStopped))
            {
                yield return null; // Esperar un frame
            }

            // Si el agente no está activo o no está en el NavMesh, salir de la corrutina
           if (agente == null || !agente.enabled || !agente.isOnNavMesh)
            {
                Debug.LogWarning(gameObject.name + " NavMeshAgent no está listo, saliendo de Patrullar.");
                yield break;
            }

            yield return new WaitForSeconds(Random.Range(3f, 6f)); // Esperar un tiempo antes de cambiar de destino
            CambiarDestino();
        }
    }
}
