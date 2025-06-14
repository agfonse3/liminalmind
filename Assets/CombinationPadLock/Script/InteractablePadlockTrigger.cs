using UnityEngine;

public class InteractablePadlockTrigger : MonoBehaviour
{
    public PadlockInteractionManager interactionManager;
    public Transform padlockFocusPoint; // The camera zoom-in target
    public float interactionRange = 3f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionRange && Input.GetKeyDown(KeyCode.E))
        {
            interactionManager.StartPadlockInteraction(padlockFocusPoint);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}



