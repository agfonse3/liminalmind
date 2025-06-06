using UnityEngine;

public class AudiomanagerTemp : MonoBehaviour
{
    public static AudiomanagerTemp Instance { get; private set; }
    [SerializeField] AudioSource music, soundEffects;

    // Música de fondo
    public AudioClip elevatorMusic;
    public AudioClip officeAmbience;
    public AudioClip apartmentAmbience;

    // Efectos de sonido
    public AudioClip characterAgitation;
    public AudioClip footsteps;
    public AudioClip elevatorButton;
    public AudioClip paperShuffling;
    public AudioClip horrorSting;

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

    public void PlaySFX(AudioClip sound)
    {
        soundEffects.PlayOneShot(sound);
    }

    public void PlayEndMusic(AudioClip song)
    {
        music.clip = song;
        music.Stop();
        music.loop = false;
    }

    public void PlayMusic(AudioClip song)
    {
        music.Stop();
        music.clip = song;
        music.Play();
        music.loop = true;
    }


    public void MuteAll()
    {
        music.volume = 0;
        soundEffects.volume = 0;
    }


    public void VolumeMusic(float newVolume)
    {
        music.volume = newVolume;
    }

    public void VolumeSFX(float newVolume)
    {
        soundEffects.volume = newVolume;
    }

// Música de fondo
public void PlayElevatorMusic() => PlayMusic(elevatorMusic);
public void PlayOfficeAmbience() => PlayMusic(officeAmbience);
public void PlayApartmentAmbience() => PlayMusic(apartmentAmbience);

// Efectos
public void PlayCharacterAgitation() => PlaySFX(characterAgitation);
public void PlayFootsteps() => PlaySFX(footsteps);
public void PlayElevatorButton() => PlaySFX(elevatorButton);
public void PlayPaperShuffling() => PlaySFX(paperShuffling);
    public void PlayHorrorSting() => PlaySFX(horrorSting);

}
