// EnemyAI2.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI2 : MonoBehaviour
{

    NavMeshAgent agent;
    Animator anim;

    public float rango = 10;
    public float tiempoEspera = 2;
    public float quieto = 0.5f;


    bool esperando;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        moverAlPunto();
    }

    private void Update()
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
            anim.SetBool("caminando", agent.velocity.sqrMagnitude > 0.1f);
        }
    }

    void moverAlPunto()
    {
        Vector3 randomDirection = Random.insideUnitSphere * rango;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, rango, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            anim.SetBool("caminando", true);
        }
    }
    IEnumerator esperaYMueve()
    {
        esperando = true;
        anim.SetBool("caminando", false);
        yield return new WaitForSeconds(tiempoEspera);
        moverAlPunto();
        esperando = false;
    }



}