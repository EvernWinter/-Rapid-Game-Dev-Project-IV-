using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Vector3[] originalScale;
    
    private UnitTier selectedMinionTier; // The selected minion's current tier
    
    
    // Start is called before the first frame update
    void Start()
    {
        upgradeButtons = new Button[buttons.Length];
        originalScale = new Vector3[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            upgradeButtons[i] = buttons[i].GetComponent<Button>();
            upgradeButtons[i].onClick.AddListener(() => OnUpgradeButtonClick(index));
            originalScale[i] = buttons[i].transform.localScale;
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
        CheckMinionUpgradeConditions(0, TierManager.Instance.unitTiers[MinionType.SwordMan], upgradeSlivers[0], upgradeGolds[0], upgradePlatinums[0]);

        // Archer Upgrade Conditions (Common -> Silver -> Gold -> Platinum)
        CheckMinionUpgradeConditions(1, TierManager.Instance.unitTiers[MinionType.Archer], upgradeSlivers[1], upgradeGolds[1], upgradePlatinums[1]);
        
        // Sheild Upgrade Conditions (Common -> Silver -> Gold -> Platinum)
        CheckMinionUpgradeConditions(2, TierManager.Instance.unitTiers[MinionType.Sheild], upgradeSlivers[2], upgradeGolds[2], upgradePlatinums[2]);
        
        // Priest Upgrade Conditions (Common -> Silver -> Gold -> Platinum)
        CheckMinionUpgradeConditions(3, TierManager.Instance.unitTiers[MinionType.Priest], upgradeSlivers[3], upgradeGolds[3], upgradePlatinums[3]);
        
        // HorseMan Upgrade Conditions (Common -> Silver -> Gold -> Platinum)
        CheckMinionUpgradeConditions(4, TierManager.Instance.unitTiers[MinionType.HorseMan], upgradeSlivers[4], upgradeGolds[4], upgradePlatinums[4]);
    }

    private void CheckMinionUpgradeConditions(int buttonIndex, UnitTier unitTier, int sliverCost, int goldCost, int platinumCost)
    {
        int playerMoney = GameData.Instance.moneySystem.PlayerMoney;
        UnitTier currentTier = unitTier;

        switch (currentTier)
        {
            case UnitTier.Common:
                upgradeButtons[buttonIndex].gameObject.SetActive(playerMoney >= sliverCost);
                UIManager.Instance.DoSpawnButton( upgradeButtons[buttonIndex].GetComponent<RectTransform>(),originalScale[buttonIndex] ,buttonIndex);
                break;
            case UnitTier.Silver:
                upgradeButtons[buttonIndex].gameObject.SetActive(playerMoney >= goldCost);
                UIManager.Instance.DoSpawnButton( upgradeButtons[buttonIndex].GetComponent<RectTransform>(),originalScale[buttonIndex] ,buttonIndex);
                break;
            case UnitTier.Gold:
                upgradeButtons[buttonIndex].gameObject.SetActive(playerMoney >= platinumCost);
                UIManager.Instance.DoSpawnButton( upgradeButtons[buttonIndex].GetComponent<RectTransform>(),originalScale[buttonIndex] ,buttonIndex);
                break;
            default:
                upgradeButtons[buttonIndex].gameObject.SetActive(false);
                break;
        }
    }

    public void OnUpgradeButtonClick(int buttonIndex)
    {
        MinionType selectedMinion = (MinionType)buttonIndex;
        UnitTier currentTier = TierManager.Instance.unitTiers[selectedMinion];

        Debug.Log($"Upgrade button clicked for Minion: {selectedMinion} (Current Tier: {currentTier})");

        switch (currentTier)
        {
            case UnitTier.Common:
                if (GameData.Instance.moneySystem.PlayerMoney >= upgradeSlivers[buttonIndex])
                {
                    PerformUpgrade(UnitTier.Silver, upgradeSlivers[buttonIndex], selectedMinion);
                }
                else
                {
                    Debug.Log("Not enough money to upgrade to Silver.");
                }
                break;

            case UnitTier.Silver:
                if (GameData.Instance.moneySystem.PlayerMoney >= upgradeGolds[buttonIndex])
                {
                    PerformUpgrade(UnitTier.Gold, upgradeGolds[buttonIndex], selectedMinion);
                }
                else
                {
                    Debug.Log("Not enough money to upgrade to Gold.");
                }
                break;

            case UnitTier.Gold:
                if (GameData.Instance.moneySystem.PlayerMoney >= upgradePlatinums[buttonIndex])
                {
                    PerformUpgrade(UnitTier.Platinum, upgradePlatinums[buttonIndex], selectedMinion);
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

    private void PerformUpgrade(UnitTier newTier, int cost, MinionType minion)
    {
        GameData.Instance.moneySystem.SpendMoney(cost);
        TierManager.Instance.UpgradeUnitTier(minion);
        Debug.Log($"Successfully upgraded {minion} to {newTier}!");
    }
    
}
