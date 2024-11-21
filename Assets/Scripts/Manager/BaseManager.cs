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
    
    [Header("Enemy Base Wave Settings")]
    [SerializeField] private List<Wave> waves; // Define waves in the inspector
    [SerializeField] private float waveStartDelay = 5f; // Delay before starting the first wave
    [SerializeField] private float enemySpawnInterval = 2f; // Interval between enemy spawns
    private Queue<WaveUnit> enemyQueue = new Queue<WaveUnit>();
    private bool isSpawningWave = false;

    void Start()
    {
        data = GameData.Instance;
        baseHealth = baseMaxHealth;
        cooldownTimers = new float[buttons.Length];
        if (playerBase)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                int index = i;
                buttons[i].onClick.AddListener(() => SpawnMinion(index));
            }

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
        if (unit.prefab != null)
        {
            Instantiate(unit.prefab, minionSpawn.position, Quaternion.identity);
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
        public GameObject prefab;
        public int count;
    }
}
