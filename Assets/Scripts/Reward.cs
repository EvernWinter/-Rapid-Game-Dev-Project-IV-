using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType
{
    SwordManSilver,
    PriestSilver,
    HorseManSilver,
    SwordManGold,
    PriestGold,
    HorseManGold,
    SwordManPlatinum,
    PriestPlatinum,
    HorseManPlatinum,
}

public enum UpgradeRarity
{
    Common,
    Rare,
    Legendary,
}
public class Reward : MonoBehaviour
{
    [SerializeField] public UpgradeType upgradeType;
    [SerializeField] private Image upgradeImage;
    [SerializeField] private Sprite[] upgradeSprite;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public UpgradeRarity GetRarity()
    {
        switch (upgradeType)
        {
            case UpgradeType.SwordManSilver:
            case UpgradeType.PriestSilver:
            case UpgradeType.HorseManSilver:
                return UpgradeRarity.Common;

            case UpgradeType.SwordManGold:
            case UpgradeType.PriestGold:
            case UpgradeType.HorseManGold:
                return UpgradeRarity.Rare;

            case UpgradeType.SwordManPlatinum:
            case UpgradeType.PriestPlatinum:
            case UpgradeType.HorseManPlatinum:
                return UpgradeRarity.Legendary;

            default:
                return UpgradeRarity.Common; // Default case, if needed
        }
    }

    public void UpdateButton()
    {
        // Cast the enum value to an integer and use it as the index for the sprite array
        int spriteIndex = (int)upgradeType;

        // Ensure the index is within bounds to avoid errors
        if (spriteIndex >= 0 && spriteIndex < upgradeSprite.Length)
        {
            upgradeImage.sprite = upgradeSprite[spriteIndex];
        }
        else
        {
            Debug.LogWarning($"Invalid sprite index: {spriteIndex}. Check UpgradeType or upgradeSprite array.");
        }
    }
    
    public void ChooseUpgrade()
    {
        // Use the enum value as an index to identify the upgrade logic dynamically
        int upgradeIndex = (int)upgradeType;

        switch (upgradeIndex)
        {
            case 0: // SwordManSilver
            case 1: // PriestSilver
            case 2: // HorseManSilver
                Debug.Log("Chosen upgrade is Common rarity.");
                break;

            case 3: // SwordManGold
            case 4: // PriestGold
            case 5: // HorseManGold
                Debug.Log("Chosen upgrade is Rare rarity.");
                break;

            case 6: // SwordManPlatinum
            case 7: // PriestPlatinum
            case 8: // HorseManPlatinum
                Debug.Log("Chosen upgrade is Legendary rarity.");
                break;

            default:
                Debug.LogWarning($"Upgrade type {upgradeType} is not handled.");
                break;
        }
    }
    
}
