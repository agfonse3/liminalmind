using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePanel : MonoBehaviour
{

    [SerializeField] GameObject pausePannel; //panel configuracion
    [SerializeField] GameObject helpPannel; //panel ayuda
    public PausePanel PausePanel;

    void Start()
    {
        PausePanel = pausePannel.GetComponent<PausePanel>();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.P) && !GameManager.Instance.gameOver) 
        {
            Pause();
        }

        if (Input.GetKey(KeyCode.H) && !GameManager.Instance.gameOver)
        {
            HelpPan();
        }
    }

    //Metodo que activa panel de pausa
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

    //metodo que activa panel de ayuda
    public void HelpPan()
    {
        Time.timeScale = 0f;
        helpPannel.SetActive(true);
    }

}