using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] GameObject pausePannel; //panel activo
    [SerializeField] GameObject settingsPannel; //panel configuracion
    [SerializeField] GameObject confirmationPannel; //panel de confirmacion
    public AudioClip audioBGS; //audio de la escena
    public SettingsMenu settingsMenu; //script
    public ConfirmationPanel confirmationPanel; //script


    void Start()
    {
        AudiomanagerTemp.Instance.PlayMusic(audioBGS);
        settingsMenu = settingsPannel.GetComponent<SettingsMenu>();
        confirmationPanel=confirmationPannel.GetComponent<ConfirmationPanel>();
    }
    public void QuitButton() 
    {
        AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
        confirmationPanel.isQuitOrigin = true;
        confirmationPannel.SetActive(true);
    }

    public void SettingsButton() 
    {
        AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
        pausePannel.SetActive(false);
        settingsPannel.SetActive(true);
        if (settingsMenu != null)
        {
            settingsMenu.isOnpause = true;
            //Asigna la musica del panel objetivo
            AudiomanagerTemp.Instance.PlayMusic(settingsMenu.audioBGS);
        }
        else
        {
            Debug.Log("no se encontro el objeto");
        }
    }

    public void restartButton()
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

}
