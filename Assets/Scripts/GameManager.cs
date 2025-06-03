using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
public Animator animacionTransicion; // Asigna el Animator de la animación de transición
    public float tiempoEspera = 1f; // Tiempo antes de cargar la escena

    public void CambiarEscena(string nombreEscena)
    {
        StartCoroutine(Transicion(nombreEscena)); // Llamar la animación antes de cambiar de escena
    }

    IEnumerator Transicion(string nombreEscena)
    {
        animacionTransicion.SetTrigger("Iniciar"); // Activar animación de transición
        yield return new WaitForSeconds(tiempoEspera); // Esperar la duración de la animación
        SceneManager.LoadScene(nombreEscena); // Cargar la nueva escena
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeLanguage(int option)
    {
        if (option == 0 || option == 1)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[option];
        }
        
    }
}
