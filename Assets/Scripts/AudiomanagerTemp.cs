using UnityEngine;

public class AudiomanagerTemp : MonoBehaviour
{
    public static AudiomanagerTemp Instance { get; private set; }

    [Header("Fuentes de Audio")]
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource soundEffects;

    [Header("Clips de efectos")]
    public AudioClip sfxPaso;              // Pasos del jugador
    public AudioClip sfxBotonAscensor;     // Botón del ascensor
    public AudioClip sfxBotonMenu;         // Botón de interfaz
    public AudioClip sfxTerror;            // Evento de terror
    public AudioClip sfxAgitacion;         // Enemigo aparece o susto
    public AudioClip sfxPapel;             // Leer nota o abrir papel

    [Header("Clips adicionales personalizados")]
    public AudioClip sfxPuertaAbrir;       // Abrir puerta
    public AudioClip sfxError;             // No tienes la llave / error
    public AudioClip sfxRecolectar;        // Recoger objeto o llave
    public AudioClip sfxBotonFisico;       // Botón físico de puerta u otro

    [Header("Clips de música")]
    public AudioClip musicMenu;            // Música del menú
    public AudioClip musicOficina;         // Música de la oficina
    public AudioClip musicAscensor;        // Música del ascensor
    public AudioClip musicMisterio;        // Ambiente general / misterio

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene el audio entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Reproducir efecto de sonido (OneShot)
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && soundEffects != null)
        {
            soundEffects.PlayOneShot(clip);
        }
    }

    // Reproducir música en loop
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null || music == null) return;
        music.Stop();
        music.clip = clip;
        music.loop = loop;
        music.Play();
    }

    // Reproducir música una sola vez (sin loop)
    public void PlayEndMusic(AudioClip clip)
    {
        if (clip == null || music == null) return;
        music.Stop();
        music.clip = clip;
        music.loop = false;
        music.Play();
    }

    // Detener música
    public void StopMusic()
    {
        if (music != null) music.Stop();
    }

    // Silenciar todo
    public void MuteAll()
    {
        if (music != null) music.volume = 0;
        if (soundEffects != null) soundEffects.volume = 0;
    }

    // Ajustar volumen de música
    public void VolumeMusic(float newVolume)
    {
        if (music != null) music.volume = Mathf.Clamp01(newVolume);
    }

    // Ajustar volumen de efectos
    public void VolumeSFX(float newVolume)
    {
        if (soundEffects != null) soundEffects.volume = Mathf.Clamp01(newVolume);
    }
}
