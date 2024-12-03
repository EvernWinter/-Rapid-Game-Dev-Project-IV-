using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CharacterEntity : MonoBehaviour
{
    public HealthComponent CharacterHealthComponent;

    [SerializeField] protected enum CharacterState { Run, Attack, Died }
    [SerializeField] protected CharacterState currentState;
    [SerializeField] public enum CharacterSide { Ally, Enemy }
    [SerializeField] public CharacterSide characterSide;

    [SerializeField] public enum UnitType { Swordman, Priest, Horseman, Shield, Archer }

    [SerializeField] public UnitType unitType { get; private set; }


    //[SerializeField] protected CharacterType _characterType;

    [Header("Information")] 
    [SerializeField] protected string _entityName;
    [SerializeField] protected string _entityDescription;
    [SerializeField] public BaseManager baseManager;
    
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CapsuleCollider2D characterCollider;
    [SerializeField] protected SpriteRenderer sprRndr;

    [Header("Character Tier")]
    [SerializeField] protected int _characterTierNumber = 0;
    [SerializeField] protected CharacterTier _characterTier;
    [SerializeField] protected List<CharacterStat> _characterTierStat;
    public CharacterTier CharacterCurrentTier => _characterTier;
    [SerializeField] public int CharacterTierNumber => _characterTierNumber;
    
    [Header("Movement")]
    [SerializeField] protected float _moveSpeed = 10f;
    public float MoveSpeed { get; protected set; }

    [Header("Deployment")] 
    [SerializeField] protected int _deploymentCost = 10;
    [SerializeField] protected bool _isOnField = false;
    [SerializeField] protected bool _isDeployed = false;
    public bool IsOnField => _isOnField;

    [Header("Combat")] 
    [SerializeField] protected TargetDetector _targetDetector;
    [SerializeField] protected int _attackDamage = 10;
    [SerializeField] protected int _defense = 10;
    [SerializeField] protected bool _isAOE = false;
    [SerializeField] public int Defense => _defense;

    [SerializeField] protected float nextAttack;
    [SerializeField] protected float attackCooldownDuration = 1.5f;

    [Header("Animation")]
    [SerializeField] protected CharacterAnimationBase _characterAnimator;
    public CharacterAnimationBase CharacterAnimation => _characterAnimator;

    
    [Header("Audio")]
    [SerializeField] protected MinionSFX _characterSFX;
    public MinionSFX CharacterSFX => _characterSFX;

    [Header("GameObject Prefab")]
    [SerializeField] private GameObject rebirthOrb;

    protected virtual void OnEnable()
    {
        if (GetComponentInChildren<CharacterAnimationBase>() != null)
        {
            if (_characterAnimator == null)
            {
                _characterAnimator = GetComponentInChildren<CharacterAnimationBase>();
            }
            CharacterHealthComponent.OnDamageTaken += () => _characterAnimator.OnDamaged?.Invoke();
            CharacterHealthComponent.OnDamageTaken += () => DamagedFeedback();
        }
        else
        {
            Debug.LogWarning($"There is no animator for {name}");
        }
        
        if (GetComponentInChildren<MinionSFX>() != null)
        {
            if (_characterSFX == null)
            {
                _characterSFX = GetComponentInChildren<MinionSFX>();
            }
        }
        else
        {
            Debug.LogWarning($"There is no sfx for {name}");
        }
    }

    public virtual void OnDeploy()
    {
        switch (unitType)
        {
            case UnitType.Swordman:
                _characterTierNumber = TierManager.Instance.SwordManTier;
                
                _moveSpeed = TierManager.Instance.SwordManTierStats[_characterTierNumber].Speed;
                _attackDamage = TierManager.Instance.SwordManTierStats[_characterTierNumber].Attack;
                CharacterHealthComponent.SetMaxHP(TierManager.Instance.SwordManTierStats[_characterTierNumber].MaxHP);
                _defense = TierManager.Instance.SwordManTierStats[_characterTierNumber].Defense;
                attackCooldownDuration = TierManager.Instance.SwordManTierStats[_characterTierNumber].AttackRate;
                break;
            case UnitType.Archer:
                _characterTierNumber = TierManager.Instance.ArcherTier;
                
                _moveSpeed = TierManager.Instance.ArcherTierStats[_characterTierNumber].Speed;
                _attackDamage = TierManager.Instance.ArcherTierStats[_characterTierNumber].Attack;
                CharacterHealthComponent.SetMaxHP(TierManager.Instance.ArcherTierStats[_characterTierNumber].MaxHP);
                _defense = TierManager.Instance.ArcherTierStats[_characterTierNumber].Defense;
                attackCooldownDuration = TierManager.Instance.ArcherTierStats[_characterTierNumber].AttackRate;
                break;
            case UnitType.Priest:
                _characterTierNumber = TierManager.Instance.PriestTier;
                
                _moveSpeed = TierManager.Instance.PriestTierStats[_characterTierNumber].Speed;
                _attackDamage = TierManager.Instance.PriestTierStats[_characterTierNumber].Attack;
                CharacterHealthComponent.SetMaxHP(TierManager.Instance.PriestTierStats[_characterTierNumber].MaxHP);
                _defense = TierManager.Instance.PriestTierStats[_characterTierNumber].Defense;
                attackCooldownDuration = TierManager.Instance.PriestTierStats[_characterTierNumber].AttackRate;
                break;
            case UnitType.Shield:
                _characterTierNumber = TierManager.Instance.ShieldTier;
                
                _moveSpeed = TierManager.Instance.ShieldTierStats[_characterTierNumber].Speed;
                _attackDamage = TierManager.Instance.ShieldTierStats[_characterTierNumber].Attack;
                CharacterHealthComponent.SetMaxHP(TierManager.Instance.ShieldTierStats[_characterTierNumber].MaxHP);
                _defense = TierManager.Instance.ShieldTierStats[_characterTierNumber].Defense;
                attackCooldownDuration = TierManager.Instance.ShieldTierStats[_characterTierNumber].AttackRate;
                break;
            case UnitType.Horseman:
                _characterTierNumber = TierManager.Instance.HorsemanTier;
                
                _moveSpeed = TierManager.Instance.HorseManTierStats[_characterTierNumber].Speed;
                _attackDamage = TierManager.Instance.HorseManTierStats[_characterTierNumber].Attack;
                CharacterHealthComponent.SetMaxHP(TierManager.Instance.HorseManTierStats[_characterTierNumber].MaxHP);
                _defense = TierManager.Instance.HorseManTierStats[_characterTierNumber].Defense;
                attackCooldownDuration = TierManager.Instance.HorseManTierStats[_characterTierNumber].AttackRate;
                break;
        }
        CharacterHealthComponent.SetHP(CharacterHealthComponent.MaxHP);

        _isDeployed = true;
    }   

    protected virtual void HandleState()
    {
        if (!_isDeployed)
        {
            OnDeploy();
        }
        
        switch (currentState)
        {
            case CharacterState.Run:
                Walk();
                break;

            case CharacterState.Attack:

                if (_targetDetector.enemiesInRange.Count > 0 || _targetDetector.baseManagerInRange != null)
                {
                    Attack();
                }
                else
                {
                    currentState = CharacterState.Run;
                }
                break;

            case CharacterState.Died:
                characterCollider.enabled = false;
                rb.velocity = Vector2.zero;

                break;
        }
    }

    protected void CreateRebirthOrb()
    {
        GameObject orb = Instantiate(rebirthOrb);
        orb.transform.position = gameObject.transform.position;

        switch (unitType)
        {
            case UnitType.Swordman:
                orb.GetComponent<RebirthOrb>().unitType = RebirthOrb.UnitType.Swordman;
                break;

            case UnitType.Priest:
                orb.GetComponent<RebirthOrb>().unitType = RebirthOrb.UnitType.Pirest;
                break;

            case UnitType.Horseman:
                orb.GetComponent<RebirthOrb>().unitType = RebirthOrb.UnitType.Horseman;
                break;

            case UnitType.Shield:
                orb.GetComponent<RebirthOrb>().unitType = RebirthOrb.UnitType.Shield;
                break;

            case UnitType.Archer:
                orb.GetComponent<RebirthOrb>().unitType = RebirthOrb.UnitType.Archer;
                break;
        }
    }

    protected virtual void Attack()
    {
        rb.velocity = Vector2.zero;

        if (Time.time > nextAttack)
        {
            nextAttack = Time.time + attackCooldownDuration;

            if (!_isAOE)
            {
                // Single target attack
                if(_targetDetector.baseManagerInRange != null && _targetDetector.baseManagerInRange.GetComponent<BaseManager>().baseHealth > 0)
                {
                    _targetDetector.baseManagerInRange.TakeDamage(this._attackDamage);
                    _characterAnimator.OnAttack?.Invoke();
                    if (_characterSFX != null)
                    {
                        _characterSFX.OnAttack?.Invoke();
                    }
                }
                else if (_targetDetector.enemiesInRange.Count > 0)
                {
                    _targetDetector.enemiesInRange[0].CharacterHealthComponent.TakeDamage(this._attackDamage, _targetDetector.enemiesInRange[0]._defense);
                    _characterAnimator.OnAttack?.Invoke();
                    if (_characterSFX != null)
                    {
                        _characterSFX.OnAttack?.Invoke();
                    }
                }
            }
            else
            {
                if (_targetDetector.baseManagerInRange != null && _targetDetector.baseManagerInRange.GetComponent<BaseManager>().baseHealth > 0)
                {
                    _targetDetector.baseManagerInRange.TakeDamage(this._attackDamage);
                }

                // AOE attack, hit all enemies in range
                foreach (var enemy in _targetDetector.enemiesInRange)
                {
                    enemy.CharacterHealthComponent.TakeDamage(this._attackDamage, _targetDetector.enemiesInRange[0]._defense);
                    _characterAnimator.OnAttack?.Invoke();
                    if (_characterSFX != null)
                    {
                        _characterSFX.OnAttack?.Invoke();
                    }
                }
            }
        }

        
    }
    
    protected virtual void DamagedFeedback(float knockbackFroce = 0.6f)
    {
        _characterSFX.OnDamaged?.Invoke();
        
        if (rb != null)
        {
            if (unitType == UnitType.Shield)
            {
                knockbackFroce *= 0.5f;
            }
            else if (unitType == UnitType.Priest || unitType == UnitType.Archer)
            {
                knockbackFroce *= 2.5f;
            }
            else if (unitType == UnitType.Horseman)
            {
                knockbackFroce *= 0.8f;
            }

            // Determine the direction of knockback
            Vector2 knockbackDirection = characterSide == CharacterSide.Ally ? Vector2.left : Vector2.right;
        
            // Start coroutine for smooth knockback
            StartCoroutine(SmoothKnockback(knockbackDirection, knockbackFroce, 0.2f)); // Adjust force and duration as needed
        }
        else
        {
            Debug.LogWarning($"Rigidbody2D is missing for {name}");
        }
    }

// Coroutine to smoothly move the character back
    private IEnumerator SmoothKnockback(Vector2 direction, float force, float duration)
    {
        Vector2 originalPosition = rb.position;
        Vector2 targetPosition = originalPosition + direction * force;

        float elapsedTime = 0f;

        // Smoothly move the Rigidbody2D to the target position
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            rb.position = Vector2.Lerp(originalPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }

        rb.position = targetPosition;

        // // Optionally move back to the original position for a recoil effect
        // elapsedTime = 0f;
        // while (elapsedTime < duration / 2)
        // {
        //     elapsedTime += Time.deltaTime;
        //     rb.position = Vector2.Lerp(targetPosition, originalPosition, elapsedTime / (duration / 2));
        //     yield return null;
        // }

        //rb.position = originalPosition;
    }

    protected virtual void OnDisable()
    {
        CharacterHealthComponent.OnDamageTaken = null;
    }
    
    protected virtual void Awake()
    {
        CharacterHealthComponent.SetHP(CharacterHealthComponent.MaxHP);
    }
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        OnDeployed();
        currentState = CharacterState.Run;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        MoveSpeed = _moveSpeed;
        UpdateEntity();

    }

    protected virtual void UpdateEntity()
    {
        if (CharacterHealthComponent.CurrentHP <= 0)
        {
            currentState = CharacterState.Died;
            Die();
        }
    }
    
    public virtual void Die()
    {
        // Implement what happens when the character dies
        Debug.Log(gameObject.name + " died.");
        if (characterSide == CharacterSide.Enemy)
        {
            baseManager.OnEnemyDeath();
        }
        else
        {
            CreateRebirthOrb();
        }
        
        _characterSFX.OnDead?.Invoke();
        
        Destroy(gameObject);
        
        //Destroy(gameObject, 4f);
    }

    //Trigger when player or enemy AI deploy this unit
    protected virtual void OnDeployed()
    {
        if (characterSide == CharacterSide.Ally)
        {
            _isOnField = true;
            if (GameData.Instance.manaSystem.HasEnoughMana(_deploymentCost))
            {
                //_isOnField = true;
            }
            else
            {
                Debug.Log($"Player does not have enough money to spawn {_entityName}");
            }
        }
        else if (characterSide == CharacterSide.Enemy)
        {
            _isOnField = true;
        }

        //Decrease currency on deploy equal to _deploymentCost
    }

    #region Combat Functions

    protected virtual void Walk()
    {
        if (characterSide == CharacterSide.Ally)
        {
            rb.velocity = new Vector2(MoveSpeed, rb.velocity.y);
        }
        else if (characterSide == CharacterSide.Enemy)
        {
            rb.velocity = new Vector2(-MoveSpeed, rb.velocity.y);
        }

        if (_targetDetector.enemiesInRange.Count > 0 || _targetDetector.baseManagerInRange != null)
        {
            currentState = CharacterState.Attack;
        }
    }
    #endregion
    
    public enum CharacterTier
    {
        T0,
        T1,
        T2,
        T3,
        T4
    }

}

[Serializable]
public class HealthComponent
{
    public float MaxHP => _maxHP;
    [SerializeField] private float _maxHP;
    
    public float CurrentHP => _currentHP;
    [SerializeField] private float _currentHP;
    
    public bool IsInvulnerable => _isInvulnerable;
    [SerializeField] bool _isInvulnerable;

    public Action OnDamageTaken;

    public void SetHP(float hpValue)
    {
        _currentHP = hpValue;
    }
    
    public void Heal(float healValue)
    {
        _currentHP = Mathf.Clamp(_currentHP += healValue, 0, _maxHP);
    }

    public void TakeDamage(float damageValue, int defense = 0)
    {
        if (_isInvulnerable)
        {
            return;
        }

        // Calculate the effective damage, limiting the defense reduction to a maximum of 80% of the damage value
        float maxReduction = damageValue * 0.8f; // 80% of the damage
        float reducedDamage = damageValue - Mathf.Min(defense, maxReduction);

        // Ensure damage doesn't go negative
        reducedDamage = Mathf.Max(reducedDamage, 0);

        // Apply the damage
        _currentHP = Mathf.Clamp(_currentHP - reducedDamage, 0, _maxHP);

        OnDamageTaken?.Invoke();
    }

    

    public void BecomeInvulnerable()
    {
        _isInvulnerable = true;
    }

    public void BecomeVulnerable()
    {
        _isInvulnerable = false;
    }
    
    public void SetMaxHP(int value)
    {
        _maxHP = value;
    }
}

[Serializable]
public class CharacterStat
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public int MaxHP { get; private set; }
    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public int Defense { get; private set; }
    [field: SerializeField] public float AttackRate { get; private set; } = 1.5f;

    /// <summary>
    /// Changes the Speed by the specified amount.
    /// </summary>
    /// <param name="amount">The amount to change Speed by.</param>
    public void ChangeSpeed(float amount)
    {
        Speed = amount;
    }

    /// <summary>
    /// Changes the MaxHP by the specified amount.
    /// </summary>
    /// <param name="amount">The amount to change MaxHP by.</param>
    public void ChangeMaxHP(int amount)
    {
        MaxHP = amount;
    }

    /// <summary>
    /// Changes the Attack by the specified amount.
    /// </summary>
    /// <param name="amount">The amount to change Attack by.</param>
    public void ChangeAttack(int amount)
    {
        Attack = amount;
    }
}

/// <summary>
/// Changes the Defense by the specified amount.
/// </summary>
/// <param name="amount


