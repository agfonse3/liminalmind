using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int numberActualScene; //guarda el numero de la escena actual

    [SerializeField] private GameObject player;


    //ultima escena
    private Vector3 lastPosition; // posicion del jugador en la ultima escena

    //List<GameObject> lista para el inventario
   // public List<GameObject> listaInventario = new List<GameObject>();
    public bool isGameActive;
    public bool isGamePaused;
    public bool gameOver;
    private Playerdata playerData;

    [SerializeField] GameObject gameOverPannel;


    private void Awake()
    {
        //garantiza una unica instancia del mismo
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else

        {

            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOver = false;
        isGamePaused = false;
        isGameActive = false;
        playerData = player.GetComponent<Playerdata>();
    }

    public void Restart() 
    {
        gameOver = false;
        isGamePaused = false;
        SceneManager.LoadScene(0);
        isGameActive = false;
        ResetAll();
    }

  public void NuevoJuego()
    {
        SceneManager.LoadScene(1);
        //isGameActive = true;
        ResetAll();
    }

    //Metodo para determinar el idioma
    public void ChangeLanguage(int option)
    {
        if (option == 0 || option == 1)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[option];
        }
    }

      //metodo que almacena la ultima posicion del jugador
    public void setLastPosition() 
    {
        //Vector3 actualPosition = player.transform.position;
        //lastPosition = actualPosition; // ultima posicion del player
    }

    public void GoToFirstFloor() //piso oficina
    {
        //SceneManager.LoadScene(1);
        SceneManager.LoadScene(2); // escena despues de intro
        //player.transform.position = lastPosition;
    }

    public void GoToSecondFloor()// piso apartamento
    {
        //SceneManager.LoadScene(2);
        SceneManager.LoadScene(3); //escena despues de intro
        //player.transform.position = lastPosition;
        player.transform.position = new Vector3(-0.43587f, 0.133f, 0.707046f);
    }


    public void SetGameOver() 
    {
        gameOver = true;
        GameOver();
    }

    public void ResetAll() 
    {
        playerData.SanityScriptableObject.ResetData();
        playerData.Inventorylist.ResetData();
        gameOverPannel.SetActive(false);
        gameOverPannel.transform.GetChild(0).gameObject.SetActive(false);
        gameOverPannel.transform.GetChild(1).gameObject.SetActive(false);
    }

    //metodo de gameover
    public void GameOver()
    {
        gameOverPannel.SetActive(true);
        gameOverPannel.transform.GetChild(0).gameObject.SetActive(true);
        //SceneManager.LoadScene(4);
        isGameActive = false;
        isGamePaused = true;
        gameOver = true;
    }

    public void GameCompleted()
    {
        gameOverPannel.SetActive(true);
        gameOverPannel.transform.GetChild(1).gameObject.SetActive(true);
        //SceneManager.LoadScene(5);
        isGameActive = false;
        isGamePaused = true;
        gameOver = true;
    }

}
