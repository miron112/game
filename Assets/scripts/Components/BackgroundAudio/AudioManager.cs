using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music Settings")]
    public AudioClip menuMusic;
    public List<AudioClip> gameMusicTracks;
    [Range(0f, 1f)] public float defaultVolume = 0.7f;

    [Header("Audio Mixer")]
    public AudioMixerGroup musicMixerGroup;

    private AudioSource musicSource;
    private float currentMusicVolume;
    private int lastPlayedIndex = -1;

    private void Awake()
    {
        // Реализация синглтона
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Инициализация AudioSource
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicMixerGroup;
        musicSource.loop = true;

        // Загрузка сохраненной громкости
        currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", defaultVolume);
        SetMusicVolume(currentMusicVolume);
    }

    public void PlayMenuMusic()
    {
        if (musicSource.clip == menuMusic && musicSource.isPlaying) return;
        
        musicSource.clip = menuMusic;
        musicSource.Play();
    }

    public void PlayRandomGameMusic()
    {
        if (gameMusicTracks == null || gameMusicTracks.Count == 0)
        {
            Debug.LogWarning("No game music tracks assigned!");
            return;
        }

        // Выбираем случайный трек, отличный от предыдущего
        int randomIndex;
        do {
            randomIndex = Random.Range(0, gameMusicTracks.Count);
        } while (randomIndex == lastPlayedIndex && gameMusicTracks.Count > 1);

        lastPlayedIndex = randomIndex;
        musicSource.clip = gameMusicTracks[randomIndex];
        musicSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        currentMusicVolume = Mathf.Clamp(volume, 0f, 1f);
        musicSource.volume = currentMusicVolume;
        
        // Сохраняем настройки
        PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);
        PlayerPrefs.Save();
    }

    public float GetMusicVolume()
    {
        return currentMusicVolume;
    }

    // Вызывается когда трек заканчивается
    private void Update()
    {
        if (!musicSource.isPlaying && musicSource.clip != null && 
            gameMusicTracks.Contains(musicSource.clip))
        {
            PlayRandomGameMusic();
        }
    }
}