using UnityEngine;

public class ActivarPuertas : MonoBehaviour
{
private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("puertas"); // Asegúrate de que "puertas" sea el nombre correcto de la animación en el controlador.
         
   
    }
}
