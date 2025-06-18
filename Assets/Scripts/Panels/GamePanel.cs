using UnityEngine;


public class GamePanel : MonoBehaviour
{

    [SerializeField] GameObject pausePannel; //panel configuracion
    [SerializeField] GameObject helpPannel; //panel ayuda
    [SerializeField] GameObject inventoryPannel; //panel inventario
    //[SerializeField] GameObject player;
    //SanitySystem sanitySystem;

    private PausePanel PausePanel;

    void Start()
    {
        PausePanel = pausePannel.GetComponent<PausePanel>();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.P) && !GameManager.Instance.GetGameOver()) 
        {
            GameManager.Instance.SetGamePause();
            Pause();
        }

        if (Input.GetKey(KeyCode.H) && !GameManager.Instance.GetGameOver())
        {
            GameManager.Instance.SetGamePause();
            HelpPan();
        }

        if (Input.GetKey(KeyCode.F) && !GameManager.Instance.GetGameOver())
        {
            GameManager.Instance.SetGamePause();
            InventoryPan();
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
        pausePannel.SetActive(true);
    }

    //metodo que activa panel de ayuda
    public void HelpPan()
    {
        Time.timeScale = 0f;
        helpPannel.SetActive(true);
    }

    //metodo que activa panel de inventario
    public void InventoryPan()
    {
        Time.timeScale = 0f;
        inventoryPannel.SetActive(true);
    }

}