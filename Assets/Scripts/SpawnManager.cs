
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemigoPrefab;
    public int poolSize = 10;
    public Collider[] zonasSpawn; // Array de colliders que definen las zonas de spawn

    public float tiempoEntreSpawns = 5f;
    private float tiempoSiguienteSpawn;

    private Queue<GameObject> enemigoPool;

    // NUEVO: Array para rastrear si cada zona de spawn está ocupada
    private bool[] zonasOcupadas; 

    // Variables para la depuración visual (Gizmos)
    public bool mostrarGizmosDebug = true;
    public Vector3[] lastRandomPoints;
    public Vector3[] lastHitPositions;

    void Awake()
    {
        InitializePool();
        tiempoSiguienteSpawn = Time.time; 

        // NUEVO: Inicializar el array de zonas ocupadas
        if (zonasSpawn != null)
        {
            zonasOcupadas = new bool[zonasSpawn.Length];
            // Todas las zonas están libres al inicio
            for (int i = 0; i < zonasOcupadas.Length; i++)
            {
                zonasOcupadas[i] = false;
            }
        }
        else
        {
            Debug.LogError("SpawnManager: No se han asignado zonas de spawn. Por favor, asigna colliders a 'zonasSpawn' en el Inspector.");
        }
    }

    void Update()
    {
        if (Time.time >= tiempoSiguienteSpawn)
        {
            InstanciarEnemigo();
            tiempoSiguienteSpawn = Time.time + tiempoEntreSpawns;
        }
    }

    void InitializePool()
    {
        enemigoPool = new Queue<GameObject>();
        lastRandomPoints = new Vector3[poolSize];
        lastHitPositions = new Vector3[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemigo = Instantiate(enemigoPrefab);
            enemigo.SetActive(false);
            enemigoPool.Enqueue(enemigo);
            
            // NUEVO: Asignar un ID de zona al enemigo para saber de qué zona viene
            // Esto es crucial para marcar la zona como libre cuando el enemigo vuelve al pool.
            // Necesitas un script en el enemigo (ej. NpcControl) que tenga una propiedad/campo para almacenar este ID.
            EnemyAI npcControl = enemigo.GetComponent<EnemyAI>();
            if (npcControl != null)
            {
                // Este ID se usará cuando el enemigo sea "devuelto" al pool.
                // Tendrás que añadir 'public int spawnZoneID;' a tu NpcControl.cs
                // Si no se asigna aquí, tendrías que encontrar la zona por distancia o referencia al devolverlo.
            }
        }
    }

    public void InstanciarEnemigo()
    {
        if (enemigoPool.Count == 0)
        {
            Debug.LogWarning("Pool de enemigos vacío. Considera aumentar el tamaño del pool.");
            return;
        }

        if (zonasSpawn == null || zonasSpawn.Length == 0)
        {
            Debug.LogError("No hay zonas de spawn asignadas en el SpawnManager.");
            return;
        }

        // NUEVO: Intentar encontrar una zona de spawn libre
        int zonaElegidaIndex = -1;
        List<int> zonasLibresIndices = new List<int>();
        for (int i = 0; i < zonasOcupadas.Length; i++)
        {
            if (!zonasOcupadas[i])
            {
                zonasLibresIndices.Add(i);
            }
        }

        if (zonasLibresIndices.Count == 0)
        {
            Debug.Log("Todas las zonas de spawn están ocupadas. Esperando una zona libre.");
            return; // No spawnea si todas las zonas están ocupadas
        }

        // Elegir una zona libre aleatoria
        zonaElegidaIndex = zonasLibresIndices[Random.Range(0, zonasLibresIndices.Count)];
        Collider zonaAleatoria = zonasSpawn[zonaElegidaIndex];

        GameObject enemigoAActivar = enemigoPool.Dequeue(); 
        
        Vector3 spawnPosition = GenerarPosicionNavMesh(zonaAleatoria, enemigoPool.Count); 

        if (spawnPosition != Vector3.zero)
        {
            enemigoAActivar.transform.position = spawnPosition;
            enemigoAActivar.transform.rotation = Quaternion.identity;

            EnemyAI enemyScript = enemigoAActivar.GetComponent<EnemyAI>();
            if (enemyScript != null)
            {
                // NUEVO: Asignar el ID de la zona a la que pertenece este enemigo
                enemyScript.spawnZoneID = zonaElegidaIndex; // Necesitas 'public int spawnZoneID;' en NpcControl.cs
                enemyScript.ConfigurarMovimiento(zonaAleatoria); // Pasa la zona de spawn (Collider) al enemigo
                Debug.Log($"NavMeshAgent listo. Iniciando movimiento.");
            }
            else
            {
                Debug.LogWarning("El prefab del enemigo no tiene el script 'NpcControl'. No se configurará el movimiento.");
            }

            enemigoAActivar.SetActive(true);
            // NUEVO: Marcar la zona como ocupada
            zonasOcupadas[zonaElegidaIndex] = true; 
            Debug.Log($"Enemigo {enemigoAActivar.name} instanciado en zona {zonaElegidaIndex} en {spawnPosition}.");
        }
        else
        {
            Debug.LogWarning("No se pudo encontrar una posición válida en NavMesh para el enemigo. Devolviendo al pool.");
            enemigoAActivar.SetActive(false);
            enemigoPool.Enqueue(enemigoAActivar);
            // NO se marca la zona como ocupada si no se pudo spawnear.
        }
    }

    Vector3 GenerarPosicionNavMesh(Collider zona, int indexParaGizmos)
    {
        Vector3 randomPointInBounds = new Vector3(
            Random.Range(zona.bounds.min.x, zona.bounds.max.x),
            zona.bounds.center.y,
            Random.Range(zona.bounds.min.z, zona.bounds.max.z)
        );

        if (mostrarGizmosDebug && indexParaGizmos < poolSize)
        {
            lastRandomPoints[indexParaGizmos] = randomPointInBounds;
        }

        NavMeshHit hit;
        float searchRadius = 5f; 

        if (NavMesh.SamplePosition(randomPointInBounds, out hit, searchRadius, NavMesh.AllAreas))
        {
            if (mostrarGizmosDebug && indexParaGizmos < poolSize)
            {
                lastHitPositions[indexParaGizmos] = hit.position;
            }
            return hit.position;
        }

        Debug.LogWarning($"SpawnManager: No se encontró posición válida en NavMesh para el enemigo en zona {zona.name} cerca de {randomPointInBounds}");
        return Vector3.zero;
    }

    // Método para devolver un enemigo al pool
    public void DevolverEnemigoAlPool(GameObject enemigo)
    {
        enemigo.SetActive(false);
        enemigoPool.Enqueue(enemigo);

        // NUEVO: Marcar la zona como libre cuando el enemigo es devuelto al pool
        EnemyAI npcControl = enemigo.GetComponent<EnemyAI>();
        if (npcControl != null)
        {
            if (npcControl.spawnZoneID >= 0 && npcControl.spawnZoneID < zonasOcupadas.Length)
            {
                zonasOcupadas[npcControl.spawnZoneID] = false;
                Debug.Log($"Zona {npcControl.spawnZoneID} liberada.");
            }
        }
    }

    void OnDrawGizmos()
    {
        if (zonasSpawn != null)
        {
            for (int i = 0; i < zonasSpawn.Length; i++)
            {
                Collider zona = zonasSpawn[i];
                if (zona != null)
                {
                    // Cambiar color basado en si la zona está ocupada
                    if (zonasOcupadas != null && i < zonasOcupadas.Length && zonasOcupadas[i])
                    {
                        Gizmos.color = new Color(1, 0, 0, 0.3f); // Rojo semitransparente si está ocupada
                    }
                    else
                    {
                        Gizmos.color = new Color(0, 1, 0, 0.3f); // Verde semitransparente si está libre
                    }
                    Gizmos.DrawCube(zona.bounds.center, zona.bounds.size);
                }
            }
        }

        if (mostrarGizmosDebug)
        {
            if (lastRandomPoints != null)
            {
                Gizmos.color = Color.yellow;
                for (int i = 0; i < lastRandomPoints.Length; i++)
                {
                    if (lastRandomPoints[i] != Vector3.zero)
                    {
                        Gizmos.DrawSphere(lastRandomPoints[i], 0.2f);
                    }
                }
            }

            if (lastHitPositions != null)
            {
                Gizmos.color = Color.cyan;
                for (int i = 0; i < lastHitPositions.Length; i++)
                {
                    if (lastHitPositions[i] != Vector3.zero)
                    {
                        Gizmos.DrawSphere(lastHitPositions[i], 0.25f);
                    }
                }
            }
        }
    }
}
