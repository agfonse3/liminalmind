using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] GameObject pausePannel; //panel activo
    [SerializeField] GameObject settingsPannel; //panel configuracion

    public void QuitButton() 
    {
        pausePannel.SetActive(false);
        //SceneManager.LoadScene(1);// carga primera escena titulo
    }

    public void SettingsButton() 
    {
        pausePannel.SetActive(false);
        settingsPannel.SetActive(true);
    }

    public void ResumeButton() 
    {
        if (GameManager.Instance != null) 
        {
            Time.timeScale = 1.0f;
            pausePannel.SetActive(false);
            int temporalscene = GameManager.Instance.numberActualScene; // llama la ultima escena en la que estuvo
            Vector3 lastposition = GameManager.Instance.lastPosition;// llama la posicion del jugador en la ultima escena
            //player.transform.position = lastposition; //asigna la posicion al player 
            SceneManager.LoadScene(temporalscene);// carga ultima escena en la que estuvo
        }
        
    }

}
