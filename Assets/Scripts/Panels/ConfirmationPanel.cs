using UnityEngine;


public class ConfirmationPanel : PanelBasic
{
    [SerializeField] GameObject confirmationPannel; //este panel
    [SerializeField] GameObject pausePannel; //panel pausa
    [SerializeField] GameObject startPannel; //panel inicio

    public PausePanel PausePanel;

    public bool isRestartOrigin = false;

    void Start()
    {
        PausePanel = pausePannel.GetComponent<PausePanel>();
    }

    private void OnEnable()
    {
        MouseActivatedInPanel();
    }

    public void YesButton() 
    {
        if (isRestartOrigin)
        {
            isRestartOrigin = false;
            pausePannel.SetActive(false);
            confirmationPannel.SetActive(false);
            YesRestart();
        }

    }

    public void NoButton()
    {
        if (isRestartOrigin)
        {
            isRestartOrigin = false;
            confirmationPannel.SetActive(false);
            NoRestart();
        }
    }

    public void YesRestart() 
    {
        // Aquí va la lógica para Restart el juego
        PlayerPrefs.SetInt("restart", 1); // Marcamos que el juego se está reiniciando
        GameManager.Instance.Restart();
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
        pausePannel.SetActive(true);
    }
}
