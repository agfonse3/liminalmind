using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float sanityOfPlayer; //cordura del jugador
    public int numberActualScene; //guarda el numero de la escena actual

    //ultima escena
    public Vector3 lastPosition; // posicion del jugador en la ultima escena

    //List<GameObject> lista para el inventario
    public List<GameObject> listaInventario = new List<GameObject>();
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

    
    void Start()
    {
        gameOver = false;
        isGamePaused = false;
    }
    public void NuevoJuego()
    {
        AudiomanagerTemp.Instance.PlaySFX(AudiomanagerTemp.Instance.sfxBotonMenu);
        SceneManager.LoadScene(1);
        isGameActive = true;
    }
    //Metodo para determinar el idioma
    public void ChangeLanguage(int option)
    {
        AudiomanagerTemp.Instance.PlaySFX(AudiomanagerTemp.Instance.sfxBotonMenu);
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
        numberActualScene = value; // ultima escena en la que estuvo
    }

    public void SetGameOver()
    {
        gameOver = true;
    }

    //metodo de gameover
    public void GameOver()
    {
        AudiomanagerTemp.Instance.PlaySFX(AudiomanagerTemp.Instance.sfxBotonMenu);
        gameOverPannel.SetActive(true);
        //SceneManager.LoadScene(1);// carga UI scene
        isGameActive = false;
        gameOver = true;
    }

    // MÉTODOS DEL INVENTARIO
    // Método para agregar un objeto al inventario
    public void AgregarObjetoAlInventario(GameObject objeto)
    {
                AudiomanagerTemp.Instance.PlaySFX(AudiomanagerTemp.Instance.sfxBotonMenu);
        listaInventario.Add(objeto);
        Debug.Log("Objeto agregado al inventario: " + objeto.name);
    }

    // Método para mostrar los objetos que están en el inventario
    public void MostrarInventario()
    {
         AudiomanagerTemp.Instance.PlaySFX(AudiomanagerTemp.Instance.sfxBotonMenu);
        Debug.Log("Inventario actual:");

        foreach (GameObject obj in listaInventario)
        {
            if (obj != null) // Verifica si el objeto no ha sido destruido
            {
                Debug.Log("- " + obj.name);
            }
            else
            {
                Debug.Log("- Un objeto ha sido eliminado del inventario.");
            }
        }
    }
    // Método para verificar si el jugador tiene un objeto específico en el inventario
    public bool TieneObjeto(GameObject objeto)
    {
        return listaInventario.Contains(objeto);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // Presionar "I" para abrir el inventario
        {
            GameManager.Instance.MostrarInventario();
        }
    }
    
}
