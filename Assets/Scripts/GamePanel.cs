using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePanel : MonoBehaviour
{

    [SerializeField] GameObject pausePannel; //panel configuracion
    public PausePanel PausePanel;

    void Start()
    {
        PausePanel = pausePannel.GetComponent<PausePanel>();
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        if (PausePanel != null)
        {
            //Asigna la musica del panel objetivo
            AudiomanagerTemp.Instance.PlayMusic(PausePanel.audioBGS);
        }
        else
        {
            Debug.Log("no se encontro el objeto");
        }
        pausePannel.SetActive(true);
    }
}