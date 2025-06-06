using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmationPanel : MonoBehaviour
{
    [SerializeField] GameObject confirmationPannel; //este panel
    [SerializeField] GameObject pausePannel; //panel pausa
    [SerializeField] GameObject startPannel; //panel inicio

    public PausePanel PausePanel;

    public bool isQuitOrigin=false;
    public bool isRestartOrigin = false;

    void Start()
    {
        PausePanel = pausePannel.GetComponent<PausePanel>();
    }

    public void YesButton() 
    {
        if (isQuitOrigin)
        {
            isQuitOrigin = false;
            isRestartOrigin = false;
            pausePannel.SetActive(false);
            confirmationPannel.SetActive(false);
            YesQuit();
        }
        if (isRestartOrigin)
        {
            isQuitOrigin = false;
            isRestartOrigin = false;
            pausePannel.SetActive(false);
            confirmationPannel.SetActive(false);
            YesRestart();
        }

    }

    public void NoButton()
    {
        if (isQuitOrigin)
        {
            isQuitOrigin = false;
            NoQuit();
        }
        if (isRestartOrigin)
        {
            isQuitOrigin = false;
            isRestartOrigin = false;
            NoRestart();
        }
    }

    public void YesQuit() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        //SceneManager.LoadScene(0);// carga primera escena titulo
        
    }
    public void NoQuit() 
    {
        confirmationPannel.SetActive(false);
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


    public void YesRestart() 
    {
        // Aquí va la lógica para Restart el juego
        //confirmationPannel.SetActive(false);
        PlayerPrefs.SetInt("restart", 1); // Marcamos que el juego se está reiniciando
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        
    }
    public void NoRestart() 
    {
        confirmationPannel.SetActive(false);
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
