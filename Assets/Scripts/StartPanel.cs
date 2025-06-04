using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPanel : MonoBehaviour
{
    [SerializeField] GameObject creditsPannel; //panel creditos
    [SerializeField] GameObject startPannel; //panel inicio
    [SerializeField] GameObject settingsPannel; //panel configuracion
    
    public AudioClip audioBGS; //audio de la escena

    public SettingsMenu settingsMenu; // script
    public CreditsPanel creditsPanel; // script
    

    void Start()
    {
        AudiomanagerTemp.Instance.PlayMusic(audioBGS);
        settingsMenu= settingsPannel.GetComponent<SettingsMenu>();
        creditsPanel = creditsPannel.GetComponent<CreditsPanel>();
    }

    public void startButton() 
    {
        startPannel.SetActive(false);
        AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void GoSettingsPanel()
    {
        settingsPannel.SetActive(true);
        startPannel.SetActive(false);
        AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
        if (settingsMenu != null)
        {
            settingsMenu.isOnStart = true;
            //Asigna la musica del panel objetivo
            AudiomanagerTemp.Instance.PlayMusic(settingsMenu.audioBGS);
        }
        else 
        {
            Debug.Log("no se encontro el objeto");
        }    
    }

    public void GoCreditsPanel() 
    {
        creditsPannel.SetActive(true);
        startPannel.SetActive(false);
        AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
        AudiomanagerTemp.Instance.PlayMusic(creditsPanel.audioBGS);
    }
}
