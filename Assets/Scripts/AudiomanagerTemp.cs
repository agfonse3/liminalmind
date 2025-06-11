using UnityEngine;

public class AudiomanagerTemp : MonoBehaviour
{
    public static AudiomanagerTemp Instance { get; private set; }

    [Header("Fuentes de Audio")]
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource soundEffects;

    [Header("Clips de efectos")]
    public AudioClip sfxAgitacion;
    public AudioClip sfxAscensor;
    public AudioClip sfxPaso;
    public AudioClip sfxBotonAscensor;
    public AudioClip sfxPapel;
    public AudioClip sfxTerror;
    public AudioClip sfxBotonMenu;

    [Header("Clips de música")]
    public AudioClip musicMisterio;
    public AudioClip musicAscensor;
    public AudioClip musicMenu;
    public AudioClip musicOficina;

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

    // Reproduce un efecto de sonido
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && soundEffects != null)
        {
            soundEffects.PlayOneShot(clip);
        }
    }

    // Reproduce música en loop
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null || music == null) return;
        music.Stop();
        music.clip = clip;
        music.loop = loop;
        music.Play();
    }

    // Reproduce música una sola vez (sin loop)
    public void PlayEndMusic(AudioClip clip)
    {
        if (clip == null || music == null) return;
        music.Stop();
        music.clip = clip;
        music.loop = false;
        music.Play();
    }

    // Detiene la música
    public void StopMusic()
    {
        if (music != null) music.Stop();
    }

    // Silencia todo
    public void MuteAll()
    {
        if (music != null) music.volume = 0;
        if (soundEffects != null) soundEffects.volume = 0;
    }

    // Ajusta volumen de música
    public void VolumeMusic(float newVolume)
    {
        if (music != null) music.volume = Mathf.Clamp01(newVolume);
    }

    // Ajusta volumen de efectos
    public void VolumeSFX(float newVolume)
    {
        if (soundEffects != null) soundEffects.volume = Mathf.Clamp01(newVolume);
    }
}
