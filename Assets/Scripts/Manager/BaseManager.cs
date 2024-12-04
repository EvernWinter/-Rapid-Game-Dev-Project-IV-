using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Microlight.MicroBar;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public enum CharacterType
{
    Sword,
    Archer,
    Shield,
    Priest,
    HorseMan,
}

public class BaseManager : MonoBehaviour
{
    [SerializeField] protected enum BaseState { Idle, Attack, Died }
    [SerializeField] protected BaseState currentState;

    [SerializeField] public SpineAnimation playerAnimation; 
    [SerializeField] public SkeletonAnimation skeletonAnimation;
    [Header("Base Settings")]
    [SerializeField] private float baseMaxHealth = 100f;
    [SerializeField] public float baseHealth;
    [SerializeField] private MicroBar healthBar;
    [SerializeField] public bool playerBase;

    [Header("Minion Settings")]
    [SerializeField] private GameObject[] playerMinionPrefabs;
    [SerializeField] private GameObject[] enemyMinionPrefabs;
    [SerializeField] private Transform minionSpawn;
    [SerializeField] private int[] manas;
    [SerializeField] private int enemiesRemainingInWave = 0;
    
    [Header("Cooldown Button")]
    [SerializeField] private Button[] buttons;
    [SerializeField] private TMP_Text[] manaText;
    private Dictionary<CharacterType, GameObject> enemyTypeToPrefab;
    [SerializeField] private float[] spawnCooldowns;
    [SerializeField] private Image[] cdBar;
    private float[] cooldownTimers;
    [SerializeField] private GameObject[] costPanels;
    [SerializeField] private GameObject[] cdPanels;
    [SerializeField] private GameObject[] noPanels;
    
    private Vector3[] originalScale;
    
    [Header("Game Data")]
    private GameData data;
    
    [Header("Enemy Base Wave Settings")]
    [SerializeField] private List<Wave> waves; // Define waves in the inspector
    [SerializeField] private float waveStartDelay = 3.5f; // Delay before starting the first wave
    [SerializeField] private float enemySpawnInterval = 1.5f; // Interval between enemy spawns
    private Queue<WaveUnit> enemyQueue = new Queue<WaveUnit>();
    private bool isSpawningWave = false;
    [SerializeField] private int currentWaveIndex = 0;
    [SerializeField] private bool isWaveActive = false;
    [SerializeField] private TMP_Text waveText;
    
    private float startTime;
    
    private Dictionary<CharacterType, float> enemyUnlockTimes;
    private Dictionary<CharacterType, float> enemySpawnWeights;

    [Header("Combat")]
    [SerializeField] protected TargetDetector _targetDetector;
    [SerializeField] protected int _attackDamage = 10;
    [SerializeField] protected float nextAttack;
    [SerializeField] protected float attackCooldownDuration = 1.5f;

    private void Awake()
    {    
        // Define the spawn weights
        enemySpawnWeights = new Dictionary<CharacterType, float>
        {
            { CharacterType.Sword, 0.35f },
            { CharacterType.Archer, 0.225f },
            { CharacterType.Shield, 0.175f },
            { CharacterType.Priest, 0.15f },
            { CharacterType.HorseMan, 0.1f }
        };

        // Define when each enemy type becomes available
        enemyUnlockTimes = new Dictionary<CharacterType, float>
        {
            { CharacterType.Sword, 0f },    // Available from the beginning
            { CharacterType.Archer, 15f }, // Available after 15 seconds
            { CharacterType.Shield, 30f }, // Available after 30 seconds
            { CharacterType.Priest, 45f }, // Available after 45 seconds
            { CharacterType.HorseMan, 60f } // Available after 60 seconds
        };

        enemyTypeToPrefab = new Dictionary<CharacterType, GameObject>
        {
            { CharacterType.Sword, enemyMinionPrefabs[0] },
            { CharacterType.Archer, enemyMinionPrefabs[1] },
            { CharacterType.Shield, enemyMinionPrefabs[2] },
            { CharacterType.Priest, enemyMinionPrefabs[3] },
            { CharacterType.HorseMan, enemyMinionPrefabs[4] }
        };
    }
    
    

    void Start()
    {
        data = GameData.Instance;
        baseHealth = baseMaxHealth;
        healthBar.Initialize(baseMaxHealth);
        if (playerBase)
        {
            originalScale = new Vector3[buttons.Length];
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
                originalScale[i] = buttons[i].transform.localScale;
            }

            // Initialize mana text for each button
            for (int i = 0; i < manaText.Length; i++)
            {
                manaText[i].text = manas[i].ToString();
            }
        }
        else
        {
            StartCoroutine(HandleEnemyWaves());
        }
        
        startTime = Time.time;
    }

    void Update()
    {
        // Update cooldown timers and button states
        HandleState();
        UpdateUI();
    }

    private void HandleState()
    {

        switch (currentState)
        {
            case BaseState.Idle:

                if (_targetDetector.enemiesInRange.Count > 0 || _targetDetector.baseManagerInRange != null)
                {
                    currentState = BaseState.Attack;
                }

                break;

            case BaseState.Attack:

                if (_targetDetector.enemiesInRange.Count > 0 || _targetDetector.baseManagerInRange != null)
                {
                    Attack();
                    skeletonAnimation.AnimationState.SetAnimation(0, "Order", false);
                }
                else
                {
                    currentState = BaseState.Idle;
                }
                break;

            case BaseState.Died:
                //characterCollider.enabled = false;

                break;
        }
    }

    private void Attack()
    {
        if (Time.time > nextAttack)
        {
            nextAttack = Time.time + attackCooldownDuration;

            if (_targetDetector.baseManagerInRange != null && _targetDetector.baseManagerInRange.GetComponent<BaseManager>().baseHealth > 0)
            {
                _targetDetector.baseManagerInRange.TakeDamage(this._attackDamage);
                /*_characterAnimator.OnAttack?.Invoke();
                if (_characterSFX != null)
                {
                    _characterSFX.OnAttack?.Invoke();
                }*/
            }
            else if (_targetDetector.enemiesInRange.Count > 0)
            {
                _targetDetector.enemiesInRange[0].CharacterHealthComponent.TakeDamage(this._attackDamage, _targetDetector.enemiesInRange[0].Defense);
                /*_characterAnimator.OnAttack?.Invoke();
                if (_characterSFX != null)
                {
                    _characterSFX.OnAttack?.Invoke();
                }*/
            }
        }


    }

    private void UpdateUI()
    {
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
                    costPanels[i].SetActive(false);
                    cdPanels[i].SetActive(true);
                    //Debug.Log($"Cooldown Timer[{i}]: {cooldownTimers[i]} | Normalized: {normalizedCooldown}");
                }
                else
                {
                    costPanels[i].SetActive(true);
                    cdPanels[i].SetActive(false);
                }
                
                if ((data.manaSystem.CurrentMana < manas[i] ||  cooldownTimers[i] > 0) && buttons[i] != null)
                {
                    noPanels[i].SetActive(true);
                }
                else
                {
                    noPanels[i].SetActive(false);
                }
                
            }
            
        }
    }
    private void SpawnMinion(int index)
    {
        if (playerBase)
        {
            if (cooldownTimers[index] <= 0 && playerMinionPrefabs[index] != null  && data.manaSystem.CurrentMana > manas[index])
            {
                CharacterEntity spawnedCharacter = Instantiate(playerMinionPrefabs[index], minionSpawn.position, Quaternion.identity).GetComponent<CharacterEntity>();
                data.manaSystem.SpendMana(manas[index]);
                buttons[index].GetComponent<RectTransform>().DOScale(originalScale[index] * 0.9f, 0.1f)
                    .SetEase(Ease.OutBounce).OnComplete(() =>
                        buttons[index].transform.DOScale(originalScale[index], 0.1f).SetEase(Ease.InOutQuad));
                cooldownTimers[index] = spawnCooldowns[index];
                skeletonAnimation.AnimationState.SetAnimation(0, "Order", false);
                spawnedCharacter.OnDeploy();
            }
            else
            {
                UIManager.Instance.ShakeButton(buttons[index].GetComponent<RectTransform>(), Vector3.zero);
                if (GameData.Instance.manaSystem.CurrentMana < manas[index] && cooldownTimers[index] <= 0)
                {
                    UIManager.Instance.DoNotified("Not enough Mana!");
                }
                else
                {
                    UIManager.Instance.DoNotified("Minion on cooldown!");
                }
            }
        }
    }
    private CharacterType GetRandomEnemyType(float elapsedTime)
    {
        // Filter available enemy types based on elapsed time
        var availableEnemies = new List<CharacterType>();
        foreach (var kvp in enemyUnlockTimes)
        {
            if (elapsedTime >= kvp.Value)
            {
                availableEnemies.Add(kvp.Key);
            }
        }

        if (availableEnemies.Count == 0)
        {
            Debug.LogError("No enemies available to spawn!");
            return CharacterType.Sword; // Default fallback
        }

        // Calculate total weight for available enemies
        float totalWeight = 0f;
        foreach (var enemy in availableEnemies)
        {
            totalWeight += enemySpawnWeights[enemy];
        }

        // Perform weighted random selection
        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var enemy in availableEnemies)
        {
            cumulativeWeight += enemySpawnWeights[enemy];
            if (randomValue <= cumulativeWeight)
            {
                return enemy;
            }
        }

        // Fallback (shouldn't occur if weights are set correctly)
        return CharacterType.Sword;
    }
    
    
    public void TakeDamage(float damage)
    {
        baseHealth = Mathf.Clamp(baseHealth - damage, 0, baseMaxHealth);
        UpdateHealthBar();

        if (baseHealth <= 0)
        {
            if (playerBase)
            {
                UIManager.Instance.Lose();
            }
            else
            {
                UIManager.Instance.Win();
            }
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
        Destroy(gameObject);
    }
    
    // private IEnumerator HandleEnemyWaves()
    // {
    //     yield return new WaitForSeconds(waveStartDelay);
    //
    //     while (baseHealth > 0 )
    //     {
    //         if (!isWaveActive)
    //         {
    //             isWaveActive = true;
    //             
    //             var currentWave = waves[Random.Range(0, waves.Count)];
    //             EnqueueWave(currentWave);
    //
    //             while (enemyQueue.Count > 0)
    //             {
    //                 SpawnEnemy(enemyQueue.Dequeue());
    //                 yield return new WaitForSeconds(enemySpawnInterval);
    //             }
    //
    //             
    //
    //             isWaveActive = false;
    //             yield return new WaitForSeconds(waveStartDelay);
    //         }
    //     }
    //
    //     Debug.Log("All waves completed!");
    // }
    
    private IEnumerator HandleEnemyWaves()
    {
        yield return new WaitForSeconds(waveStartDelay);

        while (baseHealth > 0)
        {
            if (!isWaveActive)
            {
                isWaveActive = true;

                var currentWave = waves[Random.Range(0, waves.Count)];
                EnqueueWave(currentWave);

                while (enemyQueue.Count > 0)
                {
                    SpawnEnemy(enemyQueue.Dequeue());

                    // Randomize spawn interval between 1.5 and 5 seconds
                    float randomSpawnInterval = Random.Range(1.5f, 5f);
                    yield return new WaitForSeconds(randomSpawnInterval);
                }

                isWaveActive = false;
                yield return new WaitForSeconds(waveStartDelay);
            }
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
        // skeletonAnimation.AnimationState.SetAnimation(0, "Order", false);
        // if (enemyTypeToPrefab.TryGetValue(unit.unit, out var prefab))
        // {
        //     GameObject enemy = Instantiate(prefab, minionSpawn.position, Quaternion.identity);
        //     enemiesRemainingInWave++; // Increase the count when spawning an enemy
        //     enemy.GetComponent<CharacterEntity>().baseManager = this;
        //     // Assuming enemies have a script that calls this method when they die
        // }
        // else
        // {
        //     Debug.LogError($"Prefab for {unit.unit} not found in dictionary!");
        // }
        
        skeletonAnimation.AnimationState.SetAnimation(0, "Order", false);

        float elapsedTime = Time.time - startTime; // Calculate elapsed time

        // Get a random enemy type based on weights and time-based unlocks
        CharacterType randomEnemyType = GetRandomEnemyType(elapsedTime);

        if (enemyTypeToPrefab.TryGetValue(randomEnemyType, out var prefab))
        {
            GameObject enemy = Instantiate(prefab, minionSpawn.position, Quaternion.identity);
            enemiesRemainingInWave++; // Increase the count when spawning an enemy
            enemy.GetComponent<CharacterEntity>().baseManager = this;
        }
        else
        {
            Debug.LogError($"Prefab for {randomEnemyType} not found in dictionary!");
        }
    }
    
    
    public void OnEnemyDeath()
    {
        enemiesRemainingInWave--;
        GameData.Instance.moneySystem.AddMoney(2);
        GameData.Instance.manaSystem.AddMana(250);
    }
    
    
    [System.Serializable]
    public class Wave
    {
        public List<WaveUnit> units;
    }

    [System.Serializable]
    public class WaveUnit
    {
        public CharacterType unit;
        public int count;
    }
    
    
}
