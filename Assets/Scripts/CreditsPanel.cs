using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsPanel : MonoBehaviour
{

    [SerializeField] GameObject creditsPannel; //panel activo
    [SerializeField] GameObject startPannel; //panel inicio
    public AudioClip audioBGS; //audio de la escena
    public StartPanel startPanel;

    void Start()
    {
        AudiomanagerTemp.Instance.PlayMusic(audioBGS);// musica que se reproduce en el fondo
        startPanel = startPannel.GetComponent<StartPanel>();
    }

    public void ExitPanel()
    {
        if (GameManager.Instance != null)
        {
            creditsPannel.SetActive(false);
            AudiomanagerTemp.Instance.PlayEndMusic(audioBGS);
            
            if (startPanel != null)
            {
                //Asigna la musica del panel objetivo
                AudiomanagerTemp.Instance.PlayMusic(startPanel.audioBGS);
            }
            else
            {
                Debug.Log("no se encontro el objeto");
            }
            if (!startPannel.activeSelf)
            {
                startPannel.SetActive(true);
            }
        }

    }
}
