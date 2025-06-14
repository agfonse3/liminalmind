using UnityEngine;

public class SuitcaseController : MonoBehaviour
{
    public Animator animator;

    public void OpenSuitcase()
    {
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }
    }
}

