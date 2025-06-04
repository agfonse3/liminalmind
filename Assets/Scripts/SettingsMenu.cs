using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Slider volumeBGMSlider; // Control de volumen sonidos de fondo
    [SerializeField] Slider volumeBFXSlider; // Control de volumen sonidos de fondo
    [SerializeField] Button volumeFXButton; // Boton que mutea
    [SerializeField] GameObject settingsPannel; //panel activo
    [SerializeField] GameObject startPannel; //panel inicio
    [SerializeField] GameObject pausePannel; //panel pausa

    public StartPanel startPanel; //script
    public PausePanel pausePanel; //script

    bool isOnVolume = true; //variable que controla el muteado

    public AudioClip audioBGS; //audio de prueba en la escena 
    public AudioClip audioBFX; //audio de prueba para efectos
    public Sprite soundOnImage; // imagen boton sonido on
    public Sprite soundOffImage; // imagen boton mute

    public bool isOnpause = false;
    public bool isOnStart = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudiomanagerTemp.Instance.PlayMusic(audioBGS);// musica que se reproduce en el fondo
        startPanel = startPannel.GetComponent<StartPanel>();
        pausePanel = pausePannel.GetComponent<PausePanel>();
    }

    //metodo dinamica para cambiar el volumen musica de fondo
   public void ChangeVolumeBGM(float value) 
    {
        if (isOnVolume) 
        {
            AudiomanagerTemp.Instance.VolumeMusic(value / 100); // se divide en 100 para control mas preciso
        }            
    }

    //metodo dinamica para cambiar el volumen musica de efectos de sonido
    public void ChangeVolumeBFX(float value)
    {
        if (isOnVolume) 
        {
            AudiomanagerTemp.Instance.PlaySFX(audioBFX);
            AudiomanagerTemp.Instance.VolumeSFX(value / 100);// se divide en 100 para control mas preciso
        }        
    }

    //metodo para silenciar todo el juego
    public void MuteVolume()
    {
        if (isOnVolume)
        {
            AudiomanagerTemp.Instance.MuteAll();
            isOnVolume = false;
            volumeFXButton.image.sprite = soundOffImage; // cambia el icono del boton para indicar que esta apagado
        }
        else {
            isOnVolume = true;
            AudiomanagerTemp.Instance.VolumeMusic(volumeBGMSlider.value / 100);
            AudiomanagerTemp.Instance.VolumeSFX(volumeBFXSlider.value / 100);
            volumeFXButton.image.sprite = soundOnImage;
        }
               
    }

    public void ExitPanel() 
    {
        if (GameManager.Instance != null)
        {
            AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
            if (isOnStart)
            {
                settingsPannel.SetActive(false);
                if (startPanel != null)
                {
                    AudiomanagerTemp.Instance.PlayMusic(startPanel.audioBGS);
                }
                else
                {
                    Debug.Log("no se encontro el objeto");
                }
                startPannel.SetActive(true);
                isOnStart = false;
                isOnpause = false;
            }
            if (isOnpause)
            {
                settingsPannel.SetActive(false);
                if (pausePanel != null)
                {
                    AudiomanagerTemp.Instance.PlayMusic(pausePanel.audioBGS);
                }
                else
                {
                    Debug.Log("no se encontro el objeto");
                }
                pausePannel.SetActive(true);
                isOnStart = false;
                isOnpause = false;
            }




        }

    }

    

    


}
