using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [Header("Main Settings")]
    [Range(0f, 1f)] public float defaultVolume = 0.7f;
    public AudioMixerGroup sfxMixerGroup;

    [Header("UI Sounds")]
    public AudioClip buttonClickSound;
    public AudioClip buttonHoverSound;

    [Header("Game SFX")]
    public List<AudioClip> gameEffects;

    private AudioSource _sfxSource;
    private float _currentVolume;
    private List<AudioSource> _sfxPool = new List<AudioSource>();

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
            return;
        }

        Initialize();
    }

    private void Initialize()
    {
        // Основной источник звука
        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.outputAudioMixerGroup = sfxMixerGroup;

        // Загрузка сохранённых настроек
        _currentVolume = PlayerPrefs.GetFloat("SFXVolume", defaultVolume);
        ApplyVolume();
    }

    public void SetVolume(float volume)
    {
        _currentVolume = Mathf.Clamp(volume, 0f, 1f);
        PlayerPrefs.SetFloat("SFXVolume", _currentVolume);
        ApplyVolume();
        
        // Тестовый звук при изменении громкости
        PlayButtonClick();
    }

    private void ApplyVolume()
    {
        if (sfxMixerGroup != null && sfxMixerGroup.audioMixer != null)
        {
            // Конвертация линейной громкости в децибелы
            sfxMixerGroup.audioMixer.SetFloat("SFXVolume", 
                _currentVolume > 0 ? Mathf.Log10(_currentVolume) * 20 : -80f);
        }
        else if (_sfxSource != null)
        {
            _sfxSource.volume = _currentVolume;
        }
    }

    public float GetVolume() => _currentVolume;

    // === Основные методы воспроизведения ===
    public void PlayButtonClick()
    {
        if (buttonClickSound != null)
            PlayOneShot(buttonClickSound);
    }

    public void PlayButtonHover()
    {
        if (buttonHoverSound != null)
            PlayOneShot(buttonHoverSound);
    }

    public void PlayGameEffect(int index)
    {
        if (index >= 0 && index < gameEffects.Count)
            PlayOneShot(gameEffects[index]);
    }

    // === Пул звуков для оптимизации ===
    public void PlayOneShot(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null) return;

        var source = GetAvailableAudioSource();
        source.PlayOneShot(clip, _currentVolume * volumeScale);
    }

    private AudioSource GetAvailableAudioSource()
    {
        // Поиск свободного источника
        foreach (var source in _sfxPool)
            if (!source.isPlaying) 
                return source;

        // Создание нового при необходимости
        var newSource = gameObject.AddComponent<AudioSource>();
        newSource.outputAudioMixerGroup = sfxMixerGroup;
        _sfxPool.Add(newSource);
        return newSource;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}