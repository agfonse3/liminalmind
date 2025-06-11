using UnityEngine;

public class SanityController : MonoBehaviour
{
    
    public float maxSanity = 100f; // Cordura m�xima.
    public float sanityDecreaseRate = 5f; // Cordura perdida/segundo.
    public float sanityRegenRate = 2f; // Regeneraci�n de cordura/segundo.
    public float sanityRegenDelay = 3f; // Retraso ant�s que la regeneraci�n comience.
    private float currentSanity;
    private float timeSinceLastSeen = 0f;
    public LayerMask lineOfSightObstacles; // Obst�culos para el raycast (paredes, puertas).
    public Transform cameraTransform; // Transform de la c�mara.

    public float CurrentSanity { get { return currentSanity; } private set { currentSanity = value; } }

    void Start()
    {
        currentSanity = maxSanity;
    }
    // Detectar GameObject que bajen la cordura con un raycast, disminuirla si se encuentra en el campo de visi�n y regererarla poco a poco.
    public void HandleSanity()
    {
        SanityAffectingEntity[] targets = Object.FindObjectsByType<SanityAffectingEntity>(FindObjectsSortMode.None);
        bool seesAnyEntity = false;

        foreach (var entity in targets)
        {
            Vector3 directionToTarget = entity.transform.position - cameraTransform.position;
            Vector3 dirNormalized = directionToTarget.normalized;
            float distance = directionToTarget.magnitude;

            Ray ray = new Ray(cameraTransform.position, dirNormalized);

            if (Vector3.Dot(cameraTransform.forward, dirNormalized) > 0.5f)
            {
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, distance)) // Golpea directamente al enemigo si no hay obst�culos.
                {
                    var sanityComponent = hit.transform.GetComponentInParent<SanityAffectingEntity>();
                    if (sanityComponent == entity)
                    {
                        seesAnyEntity = true;
                        break;
                    }
                }
            }
        }

        if (seesAnyEntity)
        {
            currentSanity -= sanityDecreaseRate * Time.deltaTime;
            currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);
            timeSinceLastSeen = 0f;
            Debug.Log($"[Sanity] Decreasing: {currentSanity:F2}");
        }
        else
        {
            timeSinceLastSeen += Time.deltaTime;
            if (timeSinceLastSeen >= sanityRegenDelay)
            {
                currentSanity += sanityRegenRate * Time.deltaTime;
                currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);
            }
        }
    }

}
