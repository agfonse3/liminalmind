using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicByScene : MonoBehaviour
{
    public static MusicByScene Instance;
    private string escenaAnterior = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string currentScene = scene.name;

        if (currentScene != escenaAnterior)
        {
            AudiomanagerTemp.Instance.StopMusic(); // corta cualquier música vieja

            switch (currentScene)
            {
                case "Menu":
                    AudiomanagerTemp.Instance.PlayMusic(AudiomanagerTemp.Instance.musicMenu);
                    break;

                case "level1_office":
                    AudiomanagerTemp.Instance.PlayMusic(AudiomanagerTemp.Instance.musicOficina);
                    break;

                case "Ascensor":
                    AudiomanagerTemp.Instance.PlayMusic(AudiomanagerTemp.Instance.musicAscensor);
                    break;

                default:
                    // Sin música por defecto
                    break;
            }

            escenaAnterior = currentScene;
        }
    }
}
