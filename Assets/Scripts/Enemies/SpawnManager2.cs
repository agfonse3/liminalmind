using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
public class SpawnManager2 : MonoBehaviour
{
    public GameObject enemigoPrefab;
    public int poolSize = 10;
    [SerializeField] public Transform[][] puntosDePatrullaPorZona; // Cada zona tendrá sus puntos asignados
    public Collider[] zonasSpawn;

    public float tiempoEntreSpawns = 5f;
    private float tiempoSiguienteSpawn;

    private Queue<GameObject> enemigoPool;
    private bool[] zonasOcupadas;

    void Awake()
    {
        InitializePool();
        tiempoSiguienteSpawn = Time.time;

        if (zonasSpawn != null && zonasSpawn.Length > 0)
        {
            zonasOcupadas = new bool[zonasSpawn.Length];
            for (int i = 0; i < zonasOcupadas.Length; i++)
            {
                zonasOcupadas[i] = false;
            }
        }
        else
        {
            Debug.LogError("SpawnManager: No hay zonas de spawn asignadas.");
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

        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemigo = Instantiate(enemigoPrefab);
            enemigo.SetActive(false);
            enemigoPool.Enqueue(enemigo);
        }
    }

    public void InstanciarEnemigo()
    {
        if (enemigoPool.Count == 0 || zonasSpawn == null || zonasSpawn.Length == 0)
        {
            Debug.LogWarning("No se pudo instanciar el enemigo.");
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
            Debug.Log("Todas las zonas de spawn están ocupadas.");
            return;
        }

        zonaElegidaIndex = zonasLibresIndices[Random.Range(0, zonasLibresIndices.Count)];
        Collider zonaAleatoria = zonasSpawn[zonaElegidaIndex];

        GameObject enemigoAActivar = enemigoPool.Dequeue();
        Vector3 spawnPosition = GenerarPosicionNavMesh(zonaAleatoria);

        if (spawnPosition != Vector3.zero)
        {
            enemigoAActivar.transform.position = spawnPosition;
            enemigoAActivar.SetActive(true);

            EnemyAI2 enemyScript = enemigoAActivar.GetComponent<EnemyAI2>();
            if (enemyScript != null)
            {
                enemyScript.AsignarPuntosDePatrulla(puntosDePatrullaPorZona[zonaElegidaIndex]);
            }
            else
            {
                Debug.LogWarning("El prefab del enemigo no tiene el script 'EnemyAI'.");
            }

            zonasOcupadas[zonaElegidaIndex] = true;
        }
        else
        {
            enemigoAActivar.SetActive(false);
            enemigoPool.Enqueue(enemigoAActivar);
        }
    }

    Vector3 GenerarPosicionNavMesh(Collider zona)
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(zona.bounds.min.x, zona.bounds.max.x),
            zona.bounds.center.y,
            Random.Range(zona.bounds.min.z, zona.bounds.max.z)
        );

        NavMeshHit hit;
        float searchRadius = 5f;

        if (NavMesh.SamplePosition(randomPoint, out hit, searchRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero;
    }
}
