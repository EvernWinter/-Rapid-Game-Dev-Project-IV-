using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Button))]
public class CustomButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private AudioSource _audioSource => GetComponent<AudioSource>();
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float hoverScaleFactor = 1.2f;
    [SerializeField] private float originalScaleFactor = 1.0f;

    private Button button;
    private Vector3 originalScale;

    void Start()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_canvasGroup != null && _canvasGroup.alpha == 0)
        {
            return;
        }
        
        // Increase size on mouse enter
        if (button.interactable)
        {
            transform.localScale = originalScale * hoverScaleFactor;
            if (hoverSound != null)
            {
                //AudioSource.PlayClipAtPoint(hoverSound, Camera.main.transform.position);
                _audioSource.PlayOneShot(hoverSound);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Return to original size on mouse leave
        if (button.interactable)
        {
            transform.localScale = originalScale * originalScaleFactor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Play click sound on button click
        if (button.interactable && clickSound != null)
        {
            //AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
            _audioSource.PlayOneShot(clickSound);
        }
    }
}
