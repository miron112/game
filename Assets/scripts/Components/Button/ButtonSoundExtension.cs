using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSoundExtension : MonoBehaviour, 
    IPointerEnterHandler, IPointerClickHandler
{
    [Header("Custom Sounds")]
    [SerializeField] private AudioClip customClickSound;
    [SerializeField] private AudioClip customHoverSound;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        
        if (customClickSound != null)
            SFXManager.Instance.PlayOneShot(customClickSound);
        else
            SFXManager.Instance.PlayButtonClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_button.interactable) return;

        if (customHoverSound != null)
            SFXManager.Instance.PlayOneShot(customHoverSound);
        else
            SFXManager.Instance.PlayButtonHover();
    }
}