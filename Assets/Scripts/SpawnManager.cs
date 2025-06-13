// Tu SpawnManager.cs ya debería ser esta versión o muy similar:
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemigoPrefab;
    public int poolSize = 10;
    public Collider[] zonasSpawn; 

    public float tiempoEntreSpawns = 5f;
    private float tiempoSiguienteSpawn;

    private Queue<GameObject> enemigoPool;

    private bool[] zonasOcupadas; 

    public bool mostrarGizmosDebug = true;
    public Vector3[] lastRandomPoints;
    public Vector3[] lastHitPositions;

    void Awake()
    {
        InitializePool();
        tiempoSiguienteSpawn = Time.time; 

        if (zonasSpawn != null && zonasSpawn.Length > 0) // Añadida comprobación de Length
        {
            zonasOcupadas = new bool[zonasSpawn.Length];
            for (int i = 0; i < zonasOcupadas.Length; i++)
            {
                zonasOcupadas[i] = false;
            }
        }
        else
        {
            Debug.LogError("SpawnManager: No se han asignado zonas de spawn o el array está vacío. Asigna colliders a 'zonasSpawn' en el Inspector.");
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
            return;
        }

        zonaElegidaIndex = zonasLibresIndices[Random.Range(0, zonasLibresIndices.Count)];
        Collider zonaAleatoria = zonasSpawn[zonaElegidaIndex];

        GameObject enemigoAActivar = enemigoPool.Dequeue(); 
        
        // El count aquí se usa para los gizmos, asegúrate de que no cause un IndexOutOfRangeException si el pool se vacía.
        // Lo mantengo por ahora, pero ten cuidado con eso.
        Vector3 spawnPosition = GenerarPosicionNavMesh(zonaAleatoria, enemigoPool.Count); 

        if (spawnPosition != Vector3.zero)
        {
            enemigoAActivar.transform.position = spawnPosition;
            enemigoAActivar.transform.rotation = Quaternion.identity;

            EnemyAI enemyScript = enemigoAActivar.GetComponent<EnemyAI>(); // <-- Asegúrate que este nombre sea correcto
            if (enemyScript != null)
            {
                enemyScript.spawnZoneID = zonaElegidaIndex; 
                enemyScript.ConfigurarMovimiento(zonaAleatoria); 
                // Ya no necesitamos este log aquí, la lógica de "NavMeshAgent listo" está en EnemyAI.
                // Debug.Log($"NavMeshAgent listo. Iniciando movimiento."); 
            }
            else
            {
                Debug.LogWarning("El prefab del enemigo no tiene el script 'EnemyAI'. No se configurará el movimiento.");
            }

            enemigoAActivar.SetActive(true);
            zonasOcupadas[zonaElegidaIndex] = true; 
            Debug.Log($"Enemigo {enemigoAActivar.name} instanciado en zona {zonaElegidaIndex} en {spawnPosition}.");
        }
        else
        {
            Debug.LogWarning("No se pudo encontrar una posición válida en NavMesh para el enemigo. Devolviendo al pool.");
            enemigoAActivar.SetActive(false);
            enemigoPool.Enqueue(enemigoAActivar);
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

    public void DevolverEnemigoAlPool(GameObject enemigo)
    {
        enemigo.SetActive(false);
        enemigoPool.Enqueue(enemigo);

        EnemyAI enemyScript = enemigo.GetComponent<EnemyAI>(); // <-- Asegúrate que este nombre sea correcto
        if (enemyScript != null)
        {
            // Asegúrate de que el spawnZoneID sea válido antes de acceder al array
            if (enemyScript.spawnZoneID >= 0 && enemyScript.spawnZoneID < zonasOcupadas.Length)
            {
                zonasOcupadas[enemyScript.spawnZoneID] = false;
                Debug.Log($"Zona {enemyScript.spawnZoneID} liberada.");
            }
            else
            {
                Debug.LogWarning($"Enemy {enemigo.name} devuelto al pool, pero su spawnZoneID ({enemyScript.spawnZoneID}) es inválido.");
            }
        }
        else
        {
            Debug.LogWarning($"Enemy {enemigo.name} devuelto al pool, pero no tiene el script 'EnemyAI'. No se pudo liberar la zona.");
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
                    if (zonasOcupadas != null && i < zonasOcupadas.Length && zonasOcupadas[i])
                    {
                        Gizmos.color = new Color(1, 0, 0, 0.3f); 
                    }
                    else
                    {
                        Gizmos.color = new Color(0, 1, 0, 0.3f); 
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