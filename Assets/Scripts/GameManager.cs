using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int numberActualScene; //guarda el numero de la escena actual

    [SerializeField] private GameObject player;


    //ultima escena
    private Vector3 lastPosition; // posicion del jugador en la ultima escena
    [SerializeField] private bool isGameActive;
    [SerializeField] private bool isGamePaused;
    [SerializeField] private bool isGameOver;
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
        isGameOver = false;
        isGamePaused = false;
        isGameActive = false;
        playerData = player.GetComponent<Playerdata>();
      
    }
      public void SetGameOverPanelReference(GameObject panel)
    {
        gameOverPannel = panel;
    }

    public void Restart()
    {
        isGameOver = false;
        isGamePaused = false;
        SceneManager.LoadScene(0);
        isGameActive = false;
        ResetAll();
    }
    //inicio de juego, resetea las variables
  public void NuevoJuego()
    {
        SceneManager.LoadScene(1);
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
        SceneManager.LoadScene(2); // orden escena en build
        isGameActive = true;
        //player.transform.position = lastPosition;
    }

    public void GoToSecondFloor()// piso apartamento
    {
        SceneManager.LoadScene(3); // orden escena en build
        isGameActive = true;
        player.transform.position = new Vector3(-0.43587f, 0.133f, 0.707046f);
    }

    public void SetGameActive()
    {
        isGameActive = true;
        isGamePaused = false;
        Time.timeScale = 1.0f;
    }

    public bool GetGameActive()
    {
        return isGameActive;
    }

    public void SetGamePause()
    {
        isGamePaused = true;
        isGameActive = false;
        Time.timeScale = 0f;
    }

    public bool GetGamePause()
    {
        return isGamePaused;
    }


    public void SetGameOver()
    {
        isGameOver = true;
            isGamePaused = true;
        isGameActive = false;
        StartCoroutine(ActivarPanelGameOverConDelay());
        //Debug.Log("Game Over");
        //GameOver();
    }

    public bool GetGameOver()
    {
        return isGameOver;
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
        if (gameOverPannel == null)
    {
        Debug.LogWarning("No se encontró GameOverPanel en GameOver()");
        return;
    }

    Debug.Log("Activando panel game over: " + gameOverPannel.name);
    gameOverPannel.SetActive(true); 

    if (gameOverPannel.transform.childCount > 0)
        gameOverPannel.transform.GetChild(0).gameObject.SetActive(true);


        //gameOverPannel.SetActive(true);
        //gameOverPannel.transform.GetChild(0).gameObject.SetActive(true);
        //SceneManager.LoadScene(4);
        isGameActive = false;
        isGamePaused = true;
        isGameOver = true;
        Time.timeScale = 0f;
    }

    public void GameCompleted()
    {
        gameOverPannel.SetActive(true);
        gameOverPannel.transform.GetChild(1).gameObject.SetActive(true);
        //SceneManager.LoadScene(5);
        isGameActive = false;
        isGamePaused = true;
        //gameOver = true;
    }
private IEnumerator ActivarPanelGameOverConDelay()
{
    yield return new WaitForSecondsRealtime(0.2f); 
    GameOver();
}

}
