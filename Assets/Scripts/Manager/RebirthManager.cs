using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RebirthManager : MonoBehaviour
{
    [Header("Rebirth Setttings")]
    [SerializeField] private GameObject rebirthButton;
    [SerializeField] private Image blackBorderImage;
    [SerializeField] private Image rebirthImage;
    [SerializeField] private RectTransform fireImage;
    [SerializeField] private float cooldown;
    [SerializeField] private float cooldownTimer;
    private Vector3 originalScale;

    [SerializeField] public List<GameObject> rebirthOrbs;
    private Vector3 originalPos;
    public static RebirthManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rebirthButton.GetComponent<Button>().onClick.AddListener(() => Rebirth());
        originalPos = new Vector3(10, -70, 0);
        blackBorderImage.fillAmount = 0;
        rebirthImage.fillAmount = 0;
        originalScale = rebirthButton.transform.localScale;
        fireImage.transform.localScale = Vector3.zero;
    }

    void Update()
    {
        UICooldown();
    }

    private void Rebirth()
    {
        // Ensure the cooldown is complete before triggering the rebirth action
        if (cooldownTimer >= cooldown)
        {
            
            // Correctly scale the button using DOScale
            rebirthButton.GetComponent<RectTransform>().DOScale(originalScale * 0.9f, 0.1f)
                .SetEase(Ease.OutBounce)
                .OnComplete(() =>
                    rebirthButton.GetComponent<RectTransform>().DOScale(originalScale, 0.1f)
                        .SetEase(Ease.InOutQuad));

            // Reset cooldownTimer after rebirth
            cooldownTimer = 0;

            foreach(GameObject rebirthOrb in rebirthOrbs)
            {
                rebirthOrb.GetComponent<RebirthOrb>().Rebirth();
            }

            rebirthOrbs.Clear();
        }
        else
        {
            
            // Button shake animation when cooldown hasn't completed
            UIManager.Instance.ShakeButton(rebirthButton.GetComponent<RectTransform>(), originalPos);
            UIManager.Instance.DoNotified("Ultimate on cooldown!");
        }
    }

    public void AddRebirthOrb(GameObject orb)
    {
        rebirthOrbs.Add(orb);
    }

    public void RemoveRebirthOrb(GameObject orb)
    {
        rebirthOrbs.Remove(orb);
    }

    private void UICooldown()
    {
        
        if (cooldownTimer < cooldown) // Check if the cooldown timer is less than the full cooldown time
        {
            // Increase the cooldown timer
            cooldownTimer += Time.deltaTime;

            // Normalize the cooldown for the fillAmount
            float normalizedCooldown = cooldownTimer / cooldown; // This normalizes the cooldown so it fills from 0 to 1

            // For the black border image, start at 0.4 and increase to 1
            blackBorderImage.fillAmount = Mathf.Clamp(0.3f + normalizedCooldown * (1 - 0.3f), 0.3f, 1);

            // Update the rebirthImage normally from 0 to 1
            rebirthImage.fillAmount = normalizedCooldown;
            
            // Disable button when cooldown is finished
            //rebirthButton.GetComponent<Button>().interactable = cooldownTimer >= cooldown;
           
            Color blackColor = blackBorderImage.color;
            blackColor.a = 0.8f; // Alpha goes back to 0 (invisible)
            blackBorderImage.color = blackColor;
            
            fireImage.DOScale(Vector3.zero, 0.05f).SetEase(Ease.InOutQuad);
        }
        else
        {
            fireImage.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBounce);
            // Once the cooldown is finished, ensure all the UI elements are filled
            blackBorderImage.fillAmount = 1;
            rebirthImage.fillAmount = 1;
            Color blackColor = blackBorderImage.color;
            blackColor.a = 0f; // Alpha goes back to 0 (invisible)
            blackBorderImage.color = blackColor;
        }
    }
}
