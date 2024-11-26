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

    [Header("Information")] 
    [SerializeField] protected string _entityName;
    [SerializeField] protected string _entityDescription;
    [SerializeField] public BaseManager baseManager;
    
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CapsuleCollider2D characterCollider;
    [SerializeField] protected SpriteRenderer sprRndr;
    
    [Header("Movement")]
    [SerializeField] protected float _moveSpeed = 10f;
    public float MoveSpeed { get; protected set; }

    [Header("Deployment")] 
    [SerializeField] protected int _deploymentCost = 10;
    [SerializeField] protected bool _isOnField = false;
    public bool IsOnField => _isOnField;

    [Header("Combat")] 
    [SerializeField] protected TargetDetector _targetDetector;
    [SerializeField] protected int _attackDamage = 10;
    [SerializeField] protected int _defense = 10;
    [SerializeField] protected bool _isAOE = false;

    [SerializeField] protected float nextAttack;
    [SerializeField] protected float attackCooldownDuration = 1.5f;

    [Header("Animation")]
    [SerializeField] protected CharacterAnimationBase _characterAnimator;
    public CharacterAnimationBase CharacterAnimation => _characterAnimator;

    
    [Header("Audio")]
    [SerializeField] protected MinionSFX _characterSFX;
    public MinionSFX CharacterSFX => _characterSFX;
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
        
    }
    
    private void DamagedFeedback()
    {
        if (rb != null)
        {
            // Determine the direction of knockback
            Vector2 knockbackDirection = characterSide == CharacterSide.Ally ? Vector2.left : Vector2.right;
        
            // Start coroutine for smooth knockback
            StartCoroutine(SmoothKnockback(knockbackDirection, 0.5f, 0.2f)); // Adjust force and duration as needed
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

        // Optionally move back to the original position for a recoil effect
        elapsedTime = 0f;
        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.deltaTime;
            rb.position = Vector2.Lerp(targetPosition, originalPosition, elapsedTime / (duration / 2));
            yield return null;
        }

        rb.position = originalPosition;
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

    public void TakeDamage(float damageValue)
    {
        if (_isInvulnerable)
        {
            return;
        }
        
        OnDamageTaken?.Invoke();
        _currentHP = Mathf.Clamp(_currentHP -= damageValue, 0, _maxHP);
        
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



