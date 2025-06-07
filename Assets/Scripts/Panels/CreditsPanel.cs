using UnityEngine;

public class CreditsPanel : PanelBasic
{

    [SerializeField] GameObject creditsPannel; //panel activo
    [SerializeField] GameObject startPannel; //panel inicio
    public AudioClip audioBGS; //audio de la escena
    public StartPanel startPanel; //scrpit

    void Start()
    {
        AudiomanagerTemp.Instance.PlayMusic(audioBGS);// musica que se reproduce en el fondo
        startPanel = startPannel.GetComponent<StartPanel>();
    }

    private void OnEnable()
    {
        MouseActivatedInPanel();
    }

    public void ExitPanel()
    {
        creditsPannel.SetActive(false);
        if (AudiomanagerTemp.Instance != null)
        {
            AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
            
            if (startPanel != null)
            {
                //Asigna la musica del panel objetivo
                AudiomanagerTemp.Instance.PlayMusic(startPanel.audioBGS);
            }
            if (!startPannel.activeSelf)
            {
                startPannel.SetActive(true);
            }
        }
    }
}
