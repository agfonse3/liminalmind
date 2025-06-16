using System.Collections;
using TMPro;
using UnityEngine;


public class FadeText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextMeshPro;
    [SerializeField] private float timeToFadeOut;
    private Color colorText;

    private void Start()
    {
        colorText = TextMeshPro.color;
        colorText.a = 0;
        TextMeshPro.color = colorText;
    }

    private void Update()
    {
        if (colorText.a < 1)
        {
            colorText.a += Time.deltaTime * 0.7f ;
            colorText.a = Mathf.Clamp(colorText.a,0f,1f);
            TextMeshPro.color = colorText;
        }
       
    }

}
