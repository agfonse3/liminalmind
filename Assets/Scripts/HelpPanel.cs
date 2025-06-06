using UnityEngine;

public class HelpPanel : MonoBehaviour
{
    [SerializeField] GameObject helpPannel; //panel activo
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ExitPanel()
    {
        helpPannel.SetActive(false);
    }
}
