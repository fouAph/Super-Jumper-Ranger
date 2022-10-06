using System.Collections.Generic;
using UnityEngine;

public class AudioPoolSystem : MonoBehaviour
{
    #region Singleton
    public static AudioPoolSystem Singleton;
    private void Awake()
    {
        if (Singleton != null)
        {
            DestroyImmediate(Singleton);
        }

        Singleton = this;
    }
    #endregion

    public int poolSize;
    public Queue<AudioSource> audioqueue;

    [Range(0, 1)]
    public float masterVolume = 1f;
    [Range(0, 1)]
    public float shootVolume = 0.3f;
    [Range(0, 1)]
    public float SFXVolume = 0.3f;
    [Range(0, 1)]
    public float musicVolume = .3f;

    private AudioSource audioSource;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        audioqueue = new Queue<AudioSource>();
        var obj = new GameObject("AudioSource");
        obj.AddComponent<AudioSource>();
        audioSource = obj.GetComponent<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            var go = Instantiate(audioSource);
            audioqueue.Enqueue(go);
        }
    }

    public void PlaySFXAudio(AudioClip clip, float volume = .1f)
    {
        AudioSource source = audioqueue.Dequeue();
        source.volume = volume * SFXVolume;
        source.clip = clip;
        source.Play();

        audioqueue.Enqueue(source);
    }

    public void PlayShootAudio(AudioClip clip, float volume = .1f)
    {
        AudioSource source = audioqueue.Dequeue();
        source.volume = shootVolume * masterVolume;
        source.clip = clip;
        source.Play();

        audioqueue.Enqueue(source);
    }

    public void PlayShootAudio(AudioClip clip)
    {
        AudioSource source = audioqueue.Dequeue();
        source.volume = shootVolume * masterVolume;
        source.clip = clip;
        source.Play();

        audioqueue.Enqueue(source);
    }

    public AudioClip PlayAudioClip(AudioClip clip, float volume = .1f)
    {
        AudioSource source = audioqueue.Dequeue();
        source.volume = volume;
        source.clip = clip;
        source.Play();

        audioqueue.Enqueue(source);

        return clip;
    }

    public void PlayAudioSFX(AudioClip clip)
    {
        AudioSource source = audioqueue.Dequeue();
        source.volume = SFXVolume * masterVolume;
        source.clip = clip;
        source.Play();

        audioqueue.Enqueue(source);
    }

    public void PlayAudioLoop(AudioClip clip, bool loop)
    {
        AudioSource source = audioqueue.Dequeue();
        source.volume = musicVolume * masterVolume;
        source.loop = loop;
        source.clip = clip;
        source.Play();

        if (!loop)
            audioqueue.Enqueue(source);
    }



    public void PlayAudio(AudioClip clip, float volume = .1f)
    {
        AudioSource source = audioqueue.Dequeue();
        source.volume = volume;
        source.clip = clip;
        source.Play();

        audioqueue.Enqueue(source);
    }

    public void PlayAudioAtLocation(AudioClip clip, Vector3 position, float volume = .1f)
    {
        AudioSource source = audioqueue.Dequeue();
        source.spatialBlend = 1;
        source.volume = volume;
        source.clip = clip;
        source.transform.position = position;
        source.Play();

        audioqueue.Enqueue(source);
    }

}
public enum SoundType { SFX, Music }
