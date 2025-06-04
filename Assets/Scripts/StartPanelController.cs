using UnityEngine;

public class StartPanelController : MonoBehaviour
{
    [SerializeField] private GameObject buttonPause;
    [SerializeField] private GameObject menuPause;
    [SerializeField] private GameObject optionsPause;
    [SerializeField] private GameObject confirmMenu;
    [SerializeField] private GameObject menuStart;

    //public Button botonYes;
    //public Button botonNo;
    //public Button botonRestart;
    //public Button botonQuit;
    //private string accionPendiente;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        /*
            // Verificamos si el juego se está reiniciando
            if (PlayerPrefs.GetInt("restart", 0) == 1)
            {
                menuStart.SetActive(false);// Si el juego se reinició, ocultamos el menú de inicio
                PlayerPrefs.SetInt("restart", 0); // Reseteamos la variable para futuras cargas normales
                buttonPause.SetActive(true);
                botonRestart.onClick.AddListener(() => SetAction("Restart"));
                botonQuit.onClick.AddListener(() => SetAction("Quit"));
                botonYes.onClick.AddListener(ApplyAction);
                botonNo.onClick.AddListener(CancelarAccion);
            }
            else
            {
                menuStart.SetActive(true); // Si no se ha reiniciado, mostramos el menú de inicio
                Time.timeScale = 0f;
                botonRestart.onClick.AddListener(() => SetAction("Restart"));
                botonQuit.onClick.AddListener(() => SetAction("Quit"));
                botonYes.onClick.AddListener(ApplyAction);
                botonNo.onClick.AddListener(CancelarAccion);
            }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
        }
}
