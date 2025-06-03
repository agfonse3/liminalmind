using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class Menu : MonoBehaviour
{
[SerializeField] private GameObject buttonPause;
[SerializeField] private GameObject menuPause;
[SerializeField] private GameObject optionsPause;
[SerializeField] private GameObject confirmMenu;
[SerializeField] private GameObject menuStart;
 
public Button botonYes;
public Button botonNo;
public Button botonRestart;
public Button botonQuit;
private string accionPendiente;

void Start()
{
   
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
public void SetAction(string accion)
{
    accionPendiente = accion; // Guarda la acción pendiente
    ShowConfirm(); // Muestra la pantalla de confirmación
}

public void ShowConfirm()
{
    optionsPause.SetActive(false);
    confirmMenu.SetActive(true);
}

public void ApplyAction()
{
    confirmMenu.SetActive(false); // Cierra la confirmación
    menuStart.SetActive(false);
    if (accionPendiente == "Restart")
    {
        Restart();
    }
    else if (accionPendiente == "Quit")
    {
        Quit();
    }
}

public void CancelarAccion()
{
    confirmMenu.SetActive(false); // Cierra la confirmación sin hacer nada
    optionsPause.SetActive(true);
}

void Restart()
{
    // Aquí va la lógica para Restart el juego
    Time.timeScale = 1f;
    PlayerPrefs.SetInt("restart", 1); // Marcamos que el juego se está reiniciando

    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}

void Quit()
{
    
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    menuPause.SetActive(false);
    buttonPause.SetActive(false);
    menuStart.SetActive(true);
      
 
}

public void Pause()
{
    Time.timeScale = 0f;
    buttonPause.SetActive(false);
    menuPause.SetActive(true);
}

public void Resume()
{
    Time.timeScale = 1f;
    buttonPause.SetActive(true);
    menuPause.SetActive(false);
}

}
