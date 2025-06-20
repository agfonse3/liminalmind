using UnityEngine;

public class PausePanel : PanelBasic
{
    [SerializeField] GameObject pausePannel; //panel activo
    [SerializeField] GameObject settingsPannel; //panel configuracion
    [SerializeField] GameObject confirmationPannel; //panel de confirmacion
    public AudioClip audioBGS; //audio de la escena
    private SettingsPanel settingsPanel; //script
    private ConfirmationPanel confirmationPanel; //script

    void Start()
    {
//        AudiomanagerTemp.Instance.PlayMusic(audioBGS);
        settingsPanel = settingsPannel.GetComponent<SettingsPanel>();
        confirmationPanel=confirmationPannel.GetComponent<ConfirmationPanel>();
    }
    
    private void OnEnable()
    {
        MouseActivatedInPanel();
    }

    private void OnDisable()
    {
        if (GameManager.Instance.GetGamePause())
        {
            MouseDesctivatedOutOfPanel();
            GameManager.Instance.SetGameActive();
        }
       
    }

        public void SettingsButton() 
    {
        AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
        pausePannel.SetActive(false);
        settingsPannel.SetActive(true);
        if (settingsPanel != null)
        {
            settingsPanel.isOnpause = true;
            //Asigna la musica del panel objetivo
            AudiomanagerTemp.Instance.PlayMusic(settingsPanel.audioBGS);
        }
    }

    public void RestartButton()
    {
        AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
        confirmationPanel.isRestartOrigin = true;
        confirmationPannel.SetActive(true);
    }

    public void ResumeButton() 
    {
        AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
        Time.timeScale = 1.0f;
        pausePannel.SetActive(false);     
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

}
