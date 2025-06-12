using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int numberActualScene; //guarda el numero de la escena actual

    //ultima escena
    public Vector3 lastPosition; // posicion del jugador en la ultima escena

    //List<GameObject> lista para el inventario
   // public List<GameObject> listaInventario = new List<GameObject>();
    public bool isGameActive;
    public bool isGamePaused;
    public bool gameOver;
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
    }
  public void NuevoJuego()
    {
        SceneManager.LoadScene(1);
        isGameActive = true;
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
    public void setLastPosition(Vector3 actualPosition) 
    {
        lastPosition = actualPosition; // ultima posicion del player
    }

    //metodo que almacena la ultima escena del jugador
    public void SetActualScene(int value) 
    {
        numberActualScene=value; // ultima escena en la que estuvo
    }

   public void SetGameOver() 
    {
        gameOver = true;
    }

    //metodo de gameover
    public void GameOver()
    {
        gameOverPannel.SetActive(true);
        isGameActive = false;
        isGamePaused = true;
        gameOver = true;
    }

}
