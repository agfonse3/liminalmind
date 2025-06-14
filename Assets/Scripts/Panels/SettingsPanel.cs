using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : PanelBasic
{
    [SerializeField] Slider volumeBGMSlider; // Control de volumen sonidos de fondo
    [SerializeField] Slider volumeBFXSlider; // Control de volumen efectos de sonidos
    [SerializeField] Button volumeFXButton; // Boton que mutea
    [SerializeField] GameObject settingsPannel; // Panel activo
    [SerializeField] GameObject startPannel; // Panel inicio
    [SerializeField] GameObject pausePannel; // Panel pausa

    public StartPanel startPanel; // Script
    public PausePanel pausePanel; // Script

    bool isOnVolume = true; // Controla el muteado

    public AudioClip audioBGS; // Audio de prueba para fondo (ya no se reproduce automáticamente)
    public AudioClip audioBFX; // Audio de prueba para efectos
    public Sprite soundOnImage; // Imagen botón sonido on
    public Sprite soundOffImage; // Imagen botón mute

    public bool isOnpause = false;
    public bool isOnStart = false;

    void Start()
    {
        // 🔇 Comentado para que la música sea controlada desde MusicByScene
        // AudiomanagerTemp.Instance.PlayMusic(audioBGS);

        startPanel = startPannel.GetComponent<StartPanel>();
        pausePanel = pausePannel.GetComponent<PausePanel>();
    }

    private void OnEnable()
    {
        MouseActivatedInPanel();
    }

    public void ChangeVolumeBGM(float value)
    {
        if (isOnVolume)
        {
            AudiomanagerTemp.Instance.VolumeMusic(value / 100);
        }
    }

    public void ChangeVolumeBFX(float value)
    {
        if (isOnVolume)
        {
            AudiomanagerTemp.Instance.PlaySFX(audioBFX);
            AudiomanagerTemp.Instance.VolumeSFX(value / 100);
        }
    }

    public void MuteVolume()
    {
        if (isOnVolume)
        {
            AudiomanagerTemp.Instance.MuteAll();
            isOnVolume = false;
            volumeFXButton.image.sprite = soundOffImage;
        }
        else
        {
            isOnVolume = true;
            AudiomanagerTemp.Instance.VolumeMusic(volumeBGMSlider.value / 100);
            AudiomanagerTemp.Instance.VolumeSFX(volumeBFXSlider.value / 100);
            volumeFXButton.image.sprite = soundOnImage;
        }
    }

    public void ExitPanel()
    {
        if (AudiomanagerTemp.Instance != null)
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
                    Debug.Log("No se encontró el objeto StartPanel");
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
                    Debug.Log("No se encontró el objeto PausePanel");
                }
                pausePannel.SetActive(true);
                isOnStart = false;
                isOnpause = false;
            }
        }
    }
}
