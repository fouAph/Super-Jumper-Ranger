using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Singleton;
    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("More than one Instance of " + Singleton.GetType());
            Debug.LogWarning("Destroying " + Singleton.name);
            DestroyImmediate(Singleton.gameObject);
        }

        Singleton = this;

        source.volume = (AudioPoolSystem.Singleton.musicVolume * AudioPoolSystem.Singleton.masterVolume) *.05f;
    }

    public AudioClip mainMenuMusic;
    public AudioClip inGameMusic;
    public AudioClip gameOverMusic;
    public AudioSource source = new AudioSource();

    public void PlayMainMenuMusic()
    {
        StopMusic();
        source.loop = true;
        source.clip = mainMenuMusic;
        source.Play();
    }

    public void PlayInGameMusic()
    {
        StopMusic();
        source.loop = true;
        source.clip = inGameMusic;
        source.Play();
    }

    public void PlayGameOverMusic()
    {
        StopMusic();
        source.loop = false;
        source.clip = gameOverMusic;
        source.Play();
    }

    public void StopMusic()
    {
        source.Stop();
    }
    
}
