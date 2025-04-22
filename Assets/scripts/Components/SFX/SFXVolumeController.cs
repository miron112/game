using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeController : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private bool _playTestSound = true;

    private void Start()
    {
        if (SFXManager.Instance != null)
        {
            _volumeSlider.value = SFXManager.Instance.GetVolume();
        }

        _volumeSlider.onValueChanged.AddListener(UpdateVolume);
    }

    private void UpdateVolume(float value)
    {
        SFXManager.Instance.SetVolume(value);

        if (_playTestSound)
            SFXManager.Instance.PlayButtonClick();
    }

    private void OnDestroy()
    {
        _volumeSlider.onValueChanged.RemoveListener(UpdateVolume);
    }
}