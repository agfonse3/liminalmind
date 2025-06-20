using UnityEngine;

public class StartPanel : PanelBasic
{
    [SerializeField] GameObject creditsPannel; //panel creditos
    [SerializeField] GameObject startPannel; //panel inicio
    [SerializeField] GameObject settingsPannel; //panel configuracion

    public AudioClip audioBGS; //audio de la escena

    private SettingsPanel settingsPanel; // script
    private CreditsPanel creditsPanel; // script


    void Start()
    {
        AudiomanagerTemp.Instance.PlayMusic(audioBGS);
        settingsPanel = settingsPannel.GetComponent<SettingsPanel>();
        creditsPanel = creditsPannel.GetComponent<CreditsPanel>();
    }

    public void startButton()
    {
        startPannel.SetActive(false);
        AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
        GameManager.Instance.NuevoJuego();
    }

    private void OnEnable()
    {
        MouseActivatedInPanel();
    }

    private void OnDisable()
    {
        AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
    }


    public void GoSettingsPanel()
    {
        settingsPannel.SetActive(true);
        startPannel.SetActive(false);
        AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
        if (settingsPanel != null)
        {
            settingsPanel.isOnStart = true;
            //Asigna la musica del panel objetivo
            AudiomanagerTemp.Instance.PlayMusic(settingsPanel.audioBGS);
        }
    }

    public void GoCreditsPanel()
    {
        creditsPannel.SetActive(true);
        startPannel.SetActive(false);
        AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
        AudiomanagerTemp.Instance.PlayMusic(creditsPanel.audioBGS);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

}
