using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicTrigger : MonoBehaviour
{
    void Start()
    {
        if (AudiomanagerTemp.Instance == null) return;

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (sceneIndex)
        {
            case 0: // Menú principal
                AudiomanagerTemp.Instance.PlayMusic(AudiomanagerTemp.Instance.musicMenu);
                break;
            case 1: // Escena de misterio
                AudiomanagerTemp.Instance.PlayMusic(AudiomanagerTemp.Instance.musicMisterio);
                break;
            case 2: // Ascensor
                AudiomanagerTemp.Instance.PlayMusic(AudiomanagerTemp.Instance.musicAscensor);
                break;
            case 3: // Oficina
                AudiomanagerTemp.Instance.PlayMusic(AudiomanagerTemp.Instance.musicOficina);
                break;
            default:
                AudiomanagerTemp.Instance.StopMusic();
                break;
        }
    }
}
