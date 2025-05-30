using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemigosPrefabs; // Diferentes tipos de enemigos
    public Collider[] zonasSpawn; // Diferentes zonas de spawn
    public int cantidadMaxima = 10; // Máximo de enemigos activos
    public float tiempoEntreSpawns = 3f; // Tiempo entre spawn
    public float tiempoVida = 5f; // Tiempo de vida de cada enemigo

    private List<GameObject> pool = new List<GameObject>();
    private Dictionary<Collider, GameObject> enemigosPorZona = new Dictionary<Collider, GameObject>();

    void Start()
    {
        // Object Pool con diferentes tipos de enemigos
        for (int i = 0; i < cantidadMaxima; i++)
        {
            int indexEnemigo = Random.Range(0, enemigosPrefabs.Length);
            GameObject enemigo = Instantiate(enemigosPrefabs[indexEnemigo]);
            enemigo.SetActive(false);
            pool.Add(enemigo);
        }

        InvokeRepeating("InstanciarEnemigo", 2f, tiempoEntreSpawns);
    }

    void InstanciarEnemigo()
    {
        if (pool.Find(e => !e.activeInHierarchy) == null) return; // No hay enemigos disponibles en el pool no instancia otro

        // Elegir una zona de spawn al azar
        Collider zonaSeleccionada = zonasSpawn[Random.Range(0, zonasSpawn.Length)];

        // Verificar si la zona ya tiene un enemigo activo
        if (enemigosPorZona.ContainsKey(zonaSeleccionada) && enemigosPorZona[zonaSeleccionada].activeInHierarchy)
        {
            return; // No genera un nuevo enemigo en esta zona si ya existe otro en el area
        }

        // Elegir un enemigo disponible del pool
        GameObject enemigoDisponible = pool.Find(e => !e.activeInHierarchy);
        if (enemigoDisponible == null) return;

        // Genera una posición aleatoria dentro de la zona
        Vector3 posicionAleatoria = new Vector3(
            Random.Range(zonaSeleccionada.bounds.min.x, zonaSeleccionada.bounds.max.x),
            Random.Range(zonaSeleccionada.bounds.min.y, zonaSeleccionada.bounds.max.y),
            Random.Range(zonaSeleccionada.bounds.min.z, zonaSeleccionada.bounds.max.z)
        );

        enemigoDisponible.transform.position = posicionAleatoria;
        enemigoDisponible.SetActive(true);

        // Registra enemigo en la zona
        enemigosPorZona[zonaSeleccionada] = enemigoDisponible;

        StartCoroutine(DesactivarEnemigo(zonaSeleccionada, enemigoDisponible));
    }

    IEnumerator DesactivarEnemigo(Collider zona, GameObject enemigo)
    {
        yield return new WaitForSeconds(tiempoVida);
        enemigo.SetActive(false);

        // Elimina la referencia de la zona cuando el enemigo desaparece
        if (enemigosPorZona.ContainsKey(zona) && enemigosPorZona[zona] == enemigo)
        {
            enemigosPorZona.Remove(zona);
        }
    }
}
