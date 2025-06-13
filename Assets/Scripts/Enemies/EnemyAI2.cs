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

    void Start()
    {
        agenteNavMesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void AsignarPuntosDePatrulla(Transform[] puntos)
    {
        if (puntos != null && puntos.Length > 0)
        {
            puntosPatrulla = puntos;
            MoverAlSiguientePunto();
        }
        else
        {
            Debug.LogError("EnemyAI: No se asignaron puntos de patrullaje.");
        }
    }

    void Update()
    {
        if (agenteNavMesh == null)
        {
            Debug.LogError("EnemyAI: NavMeshAgent no inicializado.");
            return;
        }

        animator.SetFloat("Velocidad", agenteNavMesh.velocity.magnitude);

        if (!agenteNavMesh.pathPending && agenteNavMesh.remainingDistance < 0.5f)
        {
            agenteNavMesh.speed = 0f;
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
            Debug.LogError("EnemyAI: No hay puntos de patrulla asignados.");
            return;
        }

        puntoActual = (puntoActual + 1) % puntosPatrulla.Length;
        agenteNavMesh.SetDestination(puntosPatrulla[puntoActual].position);
        agenteNavMesh.speed = velocidadMovimiento;
    }
}
