// Script by Marcelli Michele

using System.Linq;
using UnityEngine;

public class PadLockPassword : MonoBehaviour
{
    public Animator animator; // Animator for the padlock
    public PadlockInteractionManager interactionManager;
    public SuitcaseController suitcase;
    public GameObject padlockRoot;
    public float hideDelay = 0.01f;

    private MoveRuller _moveRull;
    public int[] _numberPassword = { 0, 0, 0, 0 };

    private bool _isUnlocked = false; // <-- Prevent repeat

    private void Awake()
    {
        _moveRull = FindObjectOfType<MoveRuller>();
    }

    public void Password()
    {
        //  If already unlocked, do nothing
        if (_isUnlocked) return;

        if (_moveRull._numberArray.SequenceEqual(_numberPassword))
        {
            _isUnlocked = true; //  Lock out further checks
            Debug.Log("Password correct");

            if (animator != null)
                animator.SetTrigger("Unlock");

            if (interactionManager != null)
                interactionManager.EndPadlockInteraction();

            if (suitcase != null)
                suitcase.OpenSuitcase();

            if (padlockRoot != null)
                StartCoroutine(HidePadlockAfterDelay());

            for (int i = 0; i < _moveRull._rullers.Count; i++)
            {
                var emission = _moveRull._rullers[i].GetComponent<PadLockEmissionColor>();
                if (emission != null)
                {
                    emission._isSelect = false;
                    emission.BlinkingMaterial();
                }
            }
        }
    }

    private System.Collections.IEnumerator HidePadlockAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        padlockRoot.SetActive(false);
    }
}


