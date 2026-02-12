using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Global Audio Sources")]
    public AudioSource musicSource;    // 2D
    public AudioSource ambienceSource; // 2D

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(AudioClip clip, float volume = 0.6f, bool loop = true)
    {
        if (!musicSource) return;
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlayAmbience(AudioClip clip, float volume = 0.4f, bool loop = true)
    {
        if (!ambienceSource) return;
        ambienceSource.clip = clip;
        ambienceSource.volume = volume;
        ambienceSource.loop = loop;
        ambienceSource.Play();
    }
}
