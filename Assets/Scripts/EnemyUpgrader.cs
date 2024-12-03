using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUpgrader : MonoBehaviour
{
    [SerializeField]
    private float minUpgradeInterval = 30f; // Minimum interval for upgrading
    [SerializeField]
    private float maxUpgradeInterval = 60f; // Maximum interval for upgrading

    private float timer;

    void Start()
    {
        SetRandomUpgradeInterval(); // Initialize the timer with a random interval
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            UpgradeRandomWhiteUnit(); // Upgrade a random white unit
            SetRandomUpgradeInterval(); // Set a new random interval
        }
    }

    private void SetRandomUpgradeInterval()
    {
        timer = Random.Range(minUpgradeInterval, maxUpgradeInterval); // Randomize the timer
        Debug.Log($"Next upgrade in {timer:F2} seconds.");
    }

    private void UpgradeRandomWhiteUnit()
    {
        // Get a list of all White minion types
        List<MinionType> whiteMinions = new List<MinionType>
        {
            MinionType.SwordMan,
            MinionType.Priest,
            MinionType.HorseMan,
            MinionType.Archer,
            MinionType.Sheild
        };

        // Filter out minions already at the maximum tier
        whiteMinions.RemoveAll(minion => TierManager.Instance.GetWhiteTier(minion) >= 4);

        if (whiteMinions.Count > 0)
        {
            // Choose a random minion from the remaining list
            MinionType selectedMinion = whiteMinions[Random.Range(0, whiteMinions.Count)];

            // Upgrade the selected minion
            TierManager.Instance.UpgradeUnitTierWhite(selectedMinion);
        }
        else
        {
            Debug.Log("All white units are already at the maximum tier!");
        }
    }
}