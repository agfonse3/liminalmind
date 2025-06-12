using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePanel : MonoBehaviour
{

    [SerializeField] GameObject pausePannel; //panel configuracion
    [SerializeField] GameObject helpPannel; //panel ayuda
    [SerializeField] GameObject inventoryPannel; //panel inventario
    //[SerializeField] GameObject player;
    //SanitySystem sanitySystem;

    public PausePanel PausePanel;

    void Start()
    {
        PausePanel = pausePannel.GetComponent<PausePanel>();
        //sanitySystem = player.gameObject.GetComponent<SanitySystem>();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.P) && !GameManager.Instance.gameOver) 
        {
            GameManager.Instance.isGamePaused = true;
            Pause();
        }

        if (Input.GetKey(KeyCode.H) && !GameManager.Instance.gameOver)
        {
            GameManager.Instance.isGamePaused = true;
            HelpPan();
        }

        if (Input.GetKey(KeyCode.F) && !GameManager.Instance.gameOver)
        {
            GameManager.Instance.isGamePaused = true;
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