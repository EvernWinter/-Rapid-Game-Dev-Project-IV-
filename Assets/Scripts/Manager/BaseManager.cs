using System;
using System.Collections;
using System.Collections.Generic;
using Microlight.MicroBar;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyType
{
    Sword,
    Priest,
    HorseMan
}

public class BaseManager : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] private float baseMaxHealth = 100f;
    [SerializeField] private float baseHealth;
    [SerializeField] private MicroBar healthBar;
    [SerializeField] private bool playerBase;

    [Header("Minion Settings")]
    [SerializeField] private GameObject[] playerMinionPrefabs;
    [SerializeField] private GameObject[] enemyMinionPrefabs;
    [SerializeField] private Transform minionSpawn;
    [SerializeField] private int[] mana;
    
    
    
    [Header("Cooldown Button")]
    [SerializeField] private Button[] buttons;
    [SerializeField] private TMP_Text[] manaText;
    private Dictionary<EnemyType, GameObject> enemyTypeToPrefab;
    [SerializeField] private float[] spawnCooldowns;
    [SerializeField] private Image[] cdBar;
    private float[] cooldownTimers;
    [SerializeField] private GameObject[] costPanels;
    [SerializeField] private GameObject[] cdPanels;
    
    [Header("Game Data")]
    private GameData data;
    
    [Header("Enemy Base Wave Settings")]
    [SerializeField] private List<Wave> waves; // Define waves in the inspector
    [SerializeField] private float waveStartDelay = 5f; // Delay before starting the first wave
    [SerializeField] private float enemySpawnInterval = 2f; // Interval between enemy spawns
    private Queue<WaveUnit> enemyQueue = new Queue<WaveUnit>();
    private bool isSpawningWave = false;

    private void Awake()
    {
        enemyTypeToPrefab = new Dictionary<EnemyType, GameObject>
        {
            { EnemyType.Sword, enemyMinionPrefabs[0] },
            { EnemyType.Priest, enemyMinionPrefabs[1] },
            { EnemyType.HorseMan, enemyMinionPrefabs[2] }
        };
    }

    void Start()
    {
        data = GameData.Instance;
        baseHealth = baseMaxHealth;
        healthBar.Initialize(baseMaxHealth);
        if (playerBase)
        {
            cooldownTimers = new float[buttons.Length];
            foreach (var cost in costPanels)
            {
                cost.SetActive(true);
            }
            foreach (var cd in cdPanels)
            {
                cd.SetActive(false);
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                int index = i; // Fix: Capture the current index for the lambda
                buttons[i].onClick.AddListener(() => SpawnMinion(index));
            }

            // Initialize mana text for each button
            for (int i = 0; i < manaText.Length; i++)
            {
                manaText[i].text = mana[i].ToString();
            }
        }
        else
        {
            StartCoroutine(HandleEnemyWaves());
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
                    // Decrease the cooldown timer
                    cooldownTimers[i] -= Time.deltaTime;

                    // Normalize the cooldown for the fillAmount
                    float normalizedCooldown = 1 - (cooldownTimers[i] / spawnCooldowns[i]);

                    // Update the cooldown bar fill and button state
                    cdBar[i].fillAmount = normalizedCooldown;
                    buttons[i].interactable = cooldownTimers[i] <= 0;
                    costPanels[i].SetActive(false);
                    cdPanels[i].SetActive(true);
                    Debug.Log($"Cooldown Timer[{i}]: {cooldownTimers[i]} | Normalized: {normalizedCooldown}");
                }
                else
                {
                    costPanels[i].SetActive(true);
                    cdPanels[i].SetActive(false);
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
    
    private IEnumerator HandleEnemyWaves()
    {
        yield return new WaitForSeconds(waveStartDelay);

        foreach (var wave in waves)
        {
            EnqueueWave(wave);

            while (enemyQueue.Count > 0)
            {
                SpawnEnemy(enemyQueue.Dequeue());
                yield return new WaitForSeconds(enemySpawnInterval);
            }

            yield return new WaitForSeconds(waveStartDelay); // Wait before the next wave
        }

        Debug.Log("All waves completed!");
    }
    
    private void EnqueueWave(Wave wave)
    {
        foreach (var unit in wave.units)
        {
            for (int i = 0; i < unit.count; i++)
            {
                enemyQueue.Enqueue(unit);
            }
        }
    }
    
    private void SpawnEnemy(WaveUnit unit)
    {
        if (enemyTypeToPrefab.TryGetValue(unit.unit, out var prefab))
        {
            Instantiate(prefab, minionSpawn.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError($"Prefab for {unit.unit} not found in dictionary!");
        }
    }
    
    [System.Serializable]
    public class Wave
    {
        public List<WaveUnit> units;
    }

    [System.Serializable]
    public class WaveUnit
    {
        public EnemyType unit;
        public int count;
    }
}
