using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UnitTier
{
    Common,
    Silver,
    Gold,
    Platinum
}

public enum MinionType
{
    SwordMan,
    Priest,
    HorseMan
}


public class RewardManager : MonoBehaviour
{
    [Header("Upgrade")]
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private BaseManager baseManager;
    [SerializeField] private int[] upgradeSlivers;
    [SerializeField] private int[] upgradeGolds;
    [SerializeField] private int[] upgradePlatinums;
    [SerializeField] private Button[] upgradeButtons;
    
    private MinionType selectedMinion; // The selected minion type (SwordMan, Priest, HorseMan)
    private UnitTier selectedMinionTier; // The selected minion's current tier
    
    
    // Start is called before the first frame update
    void Start()
    {
        upgradeButtons = new Button[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            upgradeButtons[i] = buttons[i].GetComponent<Button>();
            upgradeButtons[i].onClick.AddListener(() => OnUpgradeButtonClick(index));
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckUpgradeConditions();
    }

    private void CheckUpgradeConditions()
    {
        // SwordMan Upgrade Conditions (Common -> Silver -> Gold -> Platinum)
        CheckMinionUpgradeConditions(0, baseManager.unitTiers[MinionType.SwordMan], upgradeSlivers[0], upgradeGolds[0], upgradePlatinums[0]);

        // Priest Upgrade Conditions (Common -> Silver -> Gold -> Platinum)
        CheckMinionUpgradeConditions(1, baseManager.unitTiers[MinionType.Priest], upgradeSlivers[1], upgradeGolds[1], upgradePlatinums[1]);

        // HorseMan Upgrade Conditions (Common -> Silver -> Gold -> Platinum)
        CheckMinionUpgradeConditions(2, baseManager.unitTiers[MinionType.HorseMan], upgradeSlivers[2], upgradeGolds[2], upgradePlatinums[2]);
    }

    private void CheckMinionUpgradeConditions(int buttonIndex, UnitTier unitTier, int sliverCost, int goldCost, int platinumCost)
    {
        int playerMoney = GameData.Instance.moneySystem.PlayerMoney;
        UnitTier currentTier = unitTier;

        switch (currentTier)
        {
            case UnitTier.Common:
                upgradeButtons[buttonIndex].gameObject.SetActive(playerMoney >= sliverCost);
                break;
            case UnitTier.Silver:
                upgradeButtons[buttonIndex].gameObject.SetActive(playerMoney >= goldCost);
                break;
            case UnitTier.Gold:
                upgradeButtons[buttonIndex].gameObject.SetActive(playerMoney >= platinumCost);
                break;
            default:
                upgradeButtons[buttonIndex].gameObject.SetActive(false);
                break;
        }
    }

    public void OnUpgradeButtonClick(int buttonIndex)
    {
        selectedMinion = (MinionType)buttonIndex;
        UnitTier currentTier = baseManager.unitTiers[selectedMinion];

        Debug.Log($"Upgrade button clicked for Minion: {selectedMinion} (Current Tier: {currentTier})");

        switch (currentTier)
        {
            case UnitTier.Common:
                if (GameData.Instance.moneySystem.PlayerMoney >= upgradeSlivers[buttonIndex])
                {
                    PerformUpgrade(UnitTier.Silver, upgradeSlivers[buttonIndex]);
                }
                else
                {
                    Debug.Log("Not enough money to upgrade to Silver.");
                }
                break;

            case UnitTier.Silver:
                if (GameData.Instance.moneySystem.PlayerMoney >= upgradeGolds[buttonIndex])
                {
                    PerformUpgrade(UnitTier.Gold, upgradeGolds[buttonIndex]);
                }
                else
                {
                    Debug.Log("Not enough money to upgrade to Gold.");
                }
                break;

            case UnitTier.Gold:
                if (GameData.Instance.moneySystem.PlayerMoney >= upgradePlatinums[buttonIndex])
                {
                    PerformUpgrade(UnitTier.Platinum, upgradePlatinums[buttonIndex]);
                }
                else
                {
                    Debug.Log("Not enough money to upgrade to Platinum.");
                }
                break;

            case UnitTier.Platinum:
                Debug.Log($"{selectedMinion} is already at the highest tier.");
                break;
        }
    }

    private void PerformUpgrade(UnitTier newTier, int cost)
    {
        GameData.Instance.moneySystem.SpendMoney(cost);
        baseManager.unitTiers[selectedMinion] = newTier;
        Debug.Log($"Successfully upgraded {selectedMinion} to {newTier}!");
    }
    
}
