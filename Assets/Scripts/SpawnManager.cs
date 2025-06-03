using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
     public GameObject[] enemigosPrefabs;
    public Collider[] zonasSpawn;
    public int cantidadMaxima = 10;
    public float tiempoEntreSpawns = 3f;
    public float tiempoVida = 5f;

    private List<GameObject> pool = new List<GameObject>();
    // Usar Dictionary para mapear zonas a enemigos activos (para evitar re-spawn en la misma zona)
    private Dictionary<Collider, GameObject> enemigosActivosPorZona = new Dictionary<Collider, GameObject>();

    void Start()
    {
        // Crear el pool de enemigos con diferentes prefabs
        for (int i = 0; i < cantidadMaxima; i++)
        {
            int indexEnemigo = Random.Range(0, enemigosPrefabs.Length);
            GameObject enemigo = Instantiate(enemigosPrefabs[indexEnemigo]);
            // Asegúrate de que el NavMeshAgent esté en el prefab o se agregue aquí si no lo está.
            // Si el prefab ya tiene EnemyAI y NavMeshAgent, no necesitas AddComponent.
            // Si el prefab NO tiene EnemyAI, entonces AddComponent está bien.
            if (enemigo.GetComponent<EnemyAI>() == null)
            {
                enemigo.AddComponent<EnemyAI>();
            }
            enemigo.SetActive(false); // Desactivar al instante
            pool.Add(enemigo);
        }

        // Iniciar la invocación repetida para instanciar enemigos
        InvokeRepeating("InstanciarEnemigo", 2f, tiempoEntreSpawns);
    }

    void InstanciarEnemigo()
    {
        // Encontrar un enemigo disponible en el pool
        GameObject enemigoDisponible = null;
        foreach (GameObject go in pool)
        {
            if (!go.activeInHierarchy)
            {
                enemigoDisponible = go;
                break;
            }
        }

        if (enemigoDisponible == null)
        {
            // Debug.Log("Pool de enemigos lleno o no hay enemigos disponibles.");
            return; // No hay enemigos disponibles en el pool
        }

        // Elegir una zona de spawn al azar
        if (zonasSpawn == null || zonasSpawn.Length == 0)
        {
            Debug.LogError("No hay zonas de spawn asignadas en SpawnManager.");
            return;
        }
        Collider zonaSeleccionada = zonasSpawn[Random.Range(0, zonasSpawn.Length)];

        // Verificar si la zona ya tiene un enemigo activo
        if (enemigosActivosPorZona.ContainsKey(zonaSeleccionada) && enemigosActivosPorZona[zonaSeleccionada] != null && enemigosActivosPorZona[zonaSeleccionada].activeInHierarchy)
        {
            // Debug.Log("Ya hay un enemigo activo en la zona de spawn: " + zonaSeleccionada.name);
            return; // No spawnea un nuevo enemigo si ya hay uno activo en esta zona
        }

        // Generar una posición aleatoria válida en la NavMesh
        Vector3 posicionAleatoria = GenerarPosicionNavMesh(zonaSeleccionada);
        if (posicionAleatoria == Vector3.zero)
        {
            Debug.LogWarning("No se pudo encontrar una posición válida en la NavMesh para spawn en la zona: " + zonaSeleccionada.name);
            return;
        }

        // Posicionar y activar el enemigo
        enemigoDisponible.transform.position = posicionAleatoria;
        enemigoDisponible.SetActive(true);

        // Obtener el script EnemyAI y configurarlo
        EnemyAI enemyAI = enemigoDisponible.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.ConfigurarMovimiento(zonaSeleccionada);
        }
        else
        {
            Debug.LogError("El enemigo instanciado no tiene el componente EnemyAI: " + enemigoDisponible.name);
            enemigoDisponible.SetActive(false); // Desactivar si no tiene el script esencial
            return;
        }

        // Registrar el enemigo en la zona
        enemigosActivosPorZona[zonaSeleccionada] = enemigoDisponible;

        // Iniciar la corrutina para desactivar el enemigo después de su tiempo de vida
        StartCoroutine(DesactivarEnemigo(zonaSeleccionada, enemigoDisponible));
    }

    Vector3 GenerarPosicionNavMesh(Collider zona)
    {
        // Generar un punto aleatorio dentro de los límites XZ del Collider
        Vector3 randomPointInBounds = new Vector3(
            Random.Range(zona.bounds.min.x, zona.bounds.max.x),
            zona.bounds.center.y, // Usar el centro Y del collider como punto de partida para SamplePosition
            Random.Range(zona.bounds.min.z, zona.bounds.max.z)
        );

        UnityEngine.AI.NavMeshHit hit;
        float searchRadius = 20f; // Asegúrate de que este radio sea suficiente para encontrar el NavMesh
        if (UnityEngine.AI.NavMesh.SamplePosition(randomPointInBounds, out hit, searchRadius, UnityEngine.AI.NavMesh.AllAreas))
        {
            return hit.position;
        }

        Debug.LogWarning("SpawnManager: No se encontró una posición válida en la NavMesh cerca de " + randomPointInBounds + " en la zona: " + zona.name);
        return Vector3.zero;
    }

    IEnumerator DesactivarEnemigo(Collider zona, GameObject enemigo)
    {
        yield return new WaitForSeconds(tiempoVida);

        // Solo desactivar si el enemigo aún está activo y es el mismo que se registró para esta zona
        if (enemigo != null && enemigo.activeInHierarchy)
        {
            enemigo.SetActive(false);
            if (enemigosActivosPorZona.ContainsKey(zona) && enemigosActivosPorZona[zona] == enemigo)
            {
                enemigosActivosPorZona.Remove(zona);
            }
        }
    }
    
}
