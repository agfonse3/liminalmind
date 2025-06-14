using System.Collections;
using UnityEngine;

public class FickedLight : MonoBehaviour
{
    public bool isFlicked = false;
    public float timeDelay;

    void Update()
    {
        if (isFlicked == false)
        {
            StartCoroutine(FlickedLight());
        }
    }
    IEnumerator FlickedLight()
    {
        isFlicked = true;
        this.gameObject.GetComponent<Light>().enabled = false; // apaga la luz
        timeDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(timeDelay);
        this.gameObject.GetComponent<Light>().enabled = true; // enciende la luz
        timeDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(timeDelay);
        isFlicked = false;
    }
}