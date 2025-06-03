using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int sanityOfPlayer; //cordura del jugador
    private int maxValueOfSanity=100; // maxima cordura
    public int numberActualScene; //guarda el numero de la escena actual

    //ultima escena
    public Vector3 lastPosition; // posicion del jugador en la ultima escena

    //List<GameObject> lista para el inventario

    public bool isGameActive;
    [SerializeField] GameObject gameOverPannel;

    private void Awake()
    {
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

    }

    public void ChangeLanguage(int option)
    {
        if (option == 0 || option == 1)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[option];
        }
    }

    public void IncreaseSanity(int value) 
    {
        if (sanityOfPlayer < maxValueOfSanity)
        { 
            sanityOfPlayer += value;
            if (sanityOfPlayer>=maxValueOfSanity) 
            {
                sanityOfPlayer=maxValueOfSanity;
            }
        }
    }

    public void DecreaseSanity(int value)
    {
        if (sanityOfPlayer > 0)
        {
            sanityOfPlayer -= value;
            if (sanityOfPlayer <=0 )
            {
                sanityOfPlayer = 0;
            }
        }
    }

    public void setLastPosition(Vector3 actualPosition) 
    {
        lastPosition = actualPosition; // ultima posicion del player
    }

    public void SetActualScene(int value) 
    {
        numberActualScene=value; // ultima escena en la que estuvo
    }

    public void GameOver()
    {
        gameOverPannel.SetActive(true);
        //SceneManager.LoadScene(1);// carga UI scene
        isGameActive = false;

    }


}
