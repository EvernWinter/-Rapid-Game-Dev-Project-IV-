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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseUpgrade()
    {
        uiManager.PauseGame();
        upgradePanel.SetActive(true);

        // Create a list of all possible upgrade types
        List<UpgradeType> availableUpgrades = new List<UpgradeType>((UpgradeType[])System.Enum.GetValues(typeof(UpgradeType)));
        Shuffle(availableUpgrades);
    
        foreach (var button in buttons)
        {
            UpgradeType selectedUpgrade;

            while (true)
            {
                // Randomly select an upgrade type from the shuffled list
                int randIndex = Random.Range(0, availableUpgrades.Count);
                selectedUpgrade = availableUpgrades[randIndex];

                // Check conditions for upgrade eligibility
                if (CanAssignUpgrade(selectedUpgrade))
                {
                    // Set the upgrade type and update the button
                    Reward reward = button.GetComponent<Reward>();
                    reward.upgradeType = selectedUpgrade;
                    reward.UpdateButton();
                
                    // Remove assigned upgrade from the list
                    availableUpgrades.RemoveAt(randIndex); 
                    break; // Exit loop once upgrade is assigned
                }
            }

            Debug.Log($"Button {button.name} selected upgrade {selectedUpgrade}");
        }
        /*foreach (var button in buttons)
        {
            UpgradeType selectedUpgrade;
            
            while (true)
            {
                // Randomly select an upgrade type from the list
                int randIndex = Random.Range(0, availableUpgrades.Count);
                selectedUpgrade = availableUpgrades[randIndex];

                // Check conditions before assigning the upgrade type
                //-------------------------------------For Contain Max Type----------------------------------------//
                if (selectedUpgrade == UpgradeType.4 &&
                    player.GetComponent<PlayerController>().reservePositions.Count > 0)
                {
                    button.GetComponent<Reward>().upgradeType = selectedUpgrade;
                    button.GetComponent<Reward>().UpdateButton();
                    availableUpgrades.RemoveAt(randIndex);
                    break; // Exit the while loop once an upgrade is assigned
                }
                else if (selectedUpgrade == UpgradeType.5 &&
                         player.GetComponent<PlayerController>().ShootingCooldown >= 0.5f)
                {
                    button.GetComponent<Reward>().upgradeType = selectedUpgrade;
                    button.GetComponent<Reward>().UpdateButton();
                    availableUpgrades.RemoveAt(randIndex);
                    break; // Exit the while loop once an upgrade is assigned
                }
                //-------------------------------------For Contain Max Type----------------------------------------//
                //-------------------------------------For Unlimit Type----------------------------------------//
                else if (selectedUpgrade == UpgradeType.1 || selectedUpgrade == UpgradeType.2 || selectedUpgrade == UpgradeType.3)
                {
                    button.GetComponent<Reward>().upgradeType = selectedUpgrade;
                    button.GetComponent<Reward>().UpdateButton();
                    availableUpgrades.RemoveAt(randIndex);
                    break; // Exit the while loop once an upgrade is assigned
                }
                // If conditions are met or no special condition is required, assign and remove the upgrade type
            }
            Debug.Log($"Button {button.name} selected upgrade {selectedUpgrade}");
        }*/
    }
    
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    
    // Helper method to check if the upgrade can be assigned
    private bool CanAssignUpgrade(UpgradeType selectedUpgrade)
    {
        switch (selectedUpgrade)
        {
            case UpgradeType.SwordManSilver:
            case UpgradeType.PriestSilver:
            case UpgradeType.HorseManSilver:
                return true; // Silver upgrades are always available

            case UpgradeType.SwordManGold:
                return baseManager.HasUpgrade(UpgradeType.SwordManSilver);

            case UpgradeType.PriestGold:
                return baseManager.HasUpgrade(UpgradeType.PriestSilver);

            case UpgradeType.HorseManGold:
                return baseManager.HasUpgrade(UpgradeType.HorseManSilver);

            case UpgradeType.SwordManPlatinum:
                return baseManager.HasUpgrade(UpgradeType.SwordManGold);

            case UpgradeType.PriestPlatinum:
                return baseManager.HasUpgrade(UpgradeType.PriestGold);

            case UpgradeType.HorseManPlatinum:
                return baseManager.HasUpgrade(UpgradeType.HorseManGold);

            default:
                return false; // Any unhandled upgrade type is invalid
        }
    }
}
