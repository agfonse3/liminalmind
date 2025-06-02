using UnityEngine;

public class AudiomanagerTemp : MonoBehaviour
{
    public static AudiomanagerTemp Instance { get; private set; }
    [SerializeField] AudioSource music, soundEffects;
    
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

}
