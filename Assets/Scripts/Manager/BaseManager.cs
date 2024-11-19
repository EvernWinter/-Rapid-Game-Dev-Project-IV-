using System.Collections;
using System.Collections.Generic;
using Microlight.MicroBar;
using UnityEngine;
using UnityEngine.UI;

public class BaseManager : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] private float baseMaxHealth = 100f;
    [SerializeField] private float baseHealth;
    [SerializeField] private MicroBar healthBar;

    [Header("Minion Settings")]
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject[] minionPrefabs;
    [SerializeField] private Transform minionSpawn;
    [SerializeField] private float[] spawnCooldowns;
    private float[] cooldownTimers;

    [Header("Game Data")]
    [SerializeField] private GameData gameData;

    void Start()
    {
        baseHealth = baseMaxHealth;
        cooldownTimers = new float[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => SpawnMinion(index));
        }
    }

    void Update()
    {
        // Update cooldown timers and button states
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if (cooldownTimers[i] > 0)
            {
                cooldownTimers[i] -= Time.deltaTime;
                buttons[i].interactable = cooldownTimers[i] <= 0;
            }
        }
    }
    
    private void SpawnMinion(int index)
    {
        if (cooldownTimers[index] <= 0 && minionPrefabs[index] != null)
        {
            Instantiate(minionPrefabs[index], minionSpawn.position, Quaternion.identity);
            cooldownTimers[index] = spawnCooldowns[index];
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
            
        }
    }
    private void OnBaseDestroyed()
    {
        Debug.Log("Base destroyed! Game over!");
        // Add game over logic here, e.g., notify GameManager
    }
}
