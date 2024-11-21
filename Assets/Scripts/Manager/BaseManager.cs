using System.Collections;
using System.Collections.Generic;
using Microlight.MicroBar;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseManager : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] private float baseMaxHealth = 100f;
    [SerializeField] private float baseHealth;
    [SerializeField] private MicroBar healthBar;
    [SerializeField] private bool playerBase;

    [Header("Minion Settings")]
    [SerializeField] private Button[] buttons;
    [SerializeField] private TMP_Text[] manaText;
    [SerializeField] private GameObject[] playerMinionPrefabs;
    [SerializeField] private GameObject[] enemyMinionPrefabs;
    [SerializeField] private Transform minionSpawn;
    [SerializeField] private float[] spawnCooldowns;
    [SerializeField] private int[] mana;
    private float[] cooldownTimers;

    [Header("Game Data")]
    private GameData data;

    void Start()
    {
        data = GameData.Instance;
        baseHealth = baseMaxHealth;
        cooldownTimers = new float[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            if (playerBase)
            {
                int index = i;
                buttons[i].onClick.AddListener(() => SpawnMinion(index));
            }
        }

        for (int i = 0; i < manaText.Length; i++)
        {
            if (playerBase)
            {
                manaText[i].text = mana[i].ToString();
            }
        }
    }

    void Update()
    {
        // Update cooldown timers and button states
        if (playerBase)
        {
            for (int i = 0; i < cooldownTimers.Length; i++)
            {
                if (cooldownTimers[i] > 0)
                {
                    cooldownTimers[i] -= Time.deltaTime;
                    buttons[i].interactable = cooldownTimers[i] <= 0;
                }
            }
        }
    }
    
    private void SpawnMinion(int index)
    {
        if (playerBase)
        {
            if (cooldownTimers[index] <= 0 && playerMinionPrefabs[index] != null  && data.manaSystem.CurrentMana > mana[index])
            {
                Instantiate(playerMinionPrefabs[index], minionSpawn.position, Quaternion.identity);
                data.manaSystem.SpendMana(mana[index]);
                cooldownTimers[index] = spawnCooldowns[index];
            } 
        }
        else
        {
            if (enemyMinionPrefabs[index] != null)
            {
                Instantiate(enemyMinionPrefabs[index], minionSpawn.position, Quaternion.identity);
                cooldownTimers[index] = spawnCooldowns[index];
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
        baseHealth = Mathf.Clamp(baseHealth - damage, 0, baseMaxHealth);
        UpdateHealthBar();

        if (baseHealth <= 0)
        {
            OnBaseDestroyed();
        }
    }
    
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.UpdateBar(baseHealth, UpdateAnim.Damage);
        }
    }
    private void OnBaseDestroyed()
    {
        Debug.Log("Base destroyed! Game over!");
    }
}
