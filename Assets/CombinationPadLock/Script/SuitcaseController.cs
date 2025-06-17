using UnityEngine;
using System.Collections;

public class SuitcaseController : MonoBehaviour
{
    public Animator animator;

    public void OpenSuitcase()
    {
        if (animator != null)
        {
            animator.SetTrigger("Open");
            StartCoroutine(BeginGameCompleted());
        }
    }

    IEnumerator BeginGameCompleted()
    {
        yield return new WaitForSeconds(4);
        GameManager.Instance.GameCompleted();
    }



}

