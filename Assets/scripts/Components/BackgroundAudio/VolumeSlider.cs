using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        // Инициализация слайдера текущим значением громкости
        musicSlider.value = AudioManager.instance.GetMusicVolume();
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
    }

    private void UpdateMusicVolume(float volume)
    {
        AudioManager.instance.SetMusicVolume(volume);
    }

    private void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
    }
}