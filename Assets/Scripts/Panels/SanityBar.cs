using UnityEngine;
using UnityEngine.UI;

public class SanityBar : MonoBehaviour
{
    public Slider sanitySlider;
    [SerializeField]  GameObject player;
    public SanityScriptableObject sanityScriptableObject;

    public float sanitytemp; // esta se va a cambiar con el script de vida 


    private void Start()
    {
        sanitySlider = GetComponent<Slider>();
        sanityScriptableObject=player.GetComponent<Playerdata>().SanityScriptableObject;
        ChangeSanityMax(sanityScriptableObject.maxSanity);
        sanitySlider.value = 100;
    }

    public void Update()
    {
        ChangeCurrentSanity(sanityScriptableObject.currentSanity);
    }
    public void ChangeSanityMax(float sanityMax)  // variable scritable object
    {
        sanitySlider.maxValue = sanityMax;
    }

    public void ChangeCurrentSanity(float currentSanity) 
    {
        sanitySlider.value = currentSanity;
        ChangeColorBar();
        if (currentSanity==0 && GameManager.Instance.GetGameActive())
        {
            GameManager.Instance.SetGameOver();
        }
    }

    public void ChangeColorBar() 
    {
        if (sanitySlider != null)
        {
            Image fillImage = sanitySlider.fillRect.GetComponent<Image>();
            if (sanitySlider.value > 33 && sanitySlider.value <= 66)
            {
                Color mediumColor;
                if (ColorUtility.TryParseHtmlString("#A3961F", out mediumColor))
                {
                    fillImage.color = mediumColor;
                }
                    
            }
            else if (sanitySlider.value <= 33)
            {
                Color lowColor;
                if (ColorUtility.TryParseHtmlString("#913E38", out lowColor))
                {
                    fillImage.color = lowColor; ;
                }
                
            }
            else 
            {
                Color highColor;
                if (ColorUtility.TryParseHtmlString("#768028", out highColor))
                {
                    fillImage.color = highColor;
                }
                
            }
          
        }
    }
}
