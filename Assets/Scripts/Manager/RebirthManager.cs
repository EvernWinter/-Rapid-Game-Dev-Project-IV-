using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RebirthManager : MonoBehaviour
{
    [Header("Rebirth Setttings")]
    [SerializeField] private Button rebirthButton;
    [SerializeField] private Image blackBorderImage;
    [SerializeField] private Image rebirthImage;
    [SerializeField] private Image fireImage;
    [SerializeField] private float cooldown;
    [SerializeField] private float cooldownTimer;
    private Vector3 originalScale;
    // Start is called before the first frame update
    void Start()
    {
        rebirthButton.onClick.AddListener(() => Rebirth());
        blackBorderImage.fillAmount = 0;
        rebirthImage.fillAmount = 0;
        fireImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UICooldown();
    }

    private void Rebirth()
    {
        if (cooldownTimer <= 0)
        {
            rebirthButton.GetComponent<RectTransform>().DOScale(originalScale * 0.9f, 0.1f)
                .SetEase(Ease.OutBounce).OnComplete(() =>
                    rebirthButton.transform.DOScale(originalScale, 0.1f).SetEase(Ease.InOutQuad));
            cooldownTimer = cooldown;
        }
        else
        {
            rebirthButton.GetComponent<RectTransform>().DOShakePosition(0.5f, strength: new Vector3(5f, 5f, 0f), vibrato: 8, randomness: 70)
                .SetEase(Ease.OutQuad);
        }
    }

    private void UICooldown()
    {
        if (cooldownTimer < 0)
        {
            // Decrease the cooldown timer
            cooldownTimer += Time.deltaTime;

            // Normalize the cooldown for the fillAmount
            float normalizedCooldown =  (cooldownTimer / cooldown);

            // Update the cooldown bar fill and button state
            blackBorderImage.fillAmount = normalizedCooldown;
            rebirthImage.fillAmount = normalizedCooldown;
            fireImage.fillAmount = normalizedCooldown;
            rebirthButton.interactable = cooldownTimer <= 0;
        }
        else
        {
            
        }
    }
}
