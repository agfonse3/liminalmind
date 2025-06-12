using UnityEngine;

[CreateAssetMenu(fileName = "SanityScriptableObject", menuName = "sanity")]
public class SanityScriptableObject : ScriptableObject
{
    public float maxSanity = 100f;
    public float sanityDecreaseRate = 5f;
    public float sanityRegenRate = 2f;
    public float sanityRegenDelay = 3f;
    public float currentSanity;

    public void ResetData()
    {
        maxSanity = 100f;
        sanityDecreaseRate = 5f;
        sanityRegenRate = 2f;
        sanityRegenDelay = 3f;
        currentSanity=0;
    }

}


