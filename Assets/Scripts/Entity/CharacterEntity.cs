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

    [Header("Information")] 
    [SerializeField] protected string _entityName;
    [SerializeField] protected string _entityDescription;

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

        if (_isOnField)
        {
            HandleState();
        }
    }
    
    protected virtual void Die()
    {
        // Implement what happens when the character dies
        Debug.Log(gameObject.name + " died.");
        //Destroy(gameObject);
        Destroy(gameObject, 4f);
    }

    //Trigger when player or enemy AI deploy this unit
    protected virtual void OnDeployed()
    {
        _isOnField = true;
        
        //Decrease currency on deploy equal to _deploymentCost
    }

    protected virtual void HandleState()
    {
        switch (currentState)
        {
            case CharacterState.Run:
                Walk();
                break;

            case CharacterState.Attack:
                Attack();
                break;

            case CharacterState.Died:
                characterCollider.enabled = false;
                rb.velocity = Vector2.zero;
                
                break;
        }
    }

    #region Combat Functions

    protected virtual void Walk()
    {

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
                if (_targetDetector.enemiesInRange.Count > 0)
                {
                    _targetDetector.enemiesInRange[0].CharacterHealthComponent.TakeDamage(this._attackDamage);
                }
            }
            else
            {
                // AOE attack, hit all enemies in range
                foreach (var enemy in _targetDetector.enemiesInRange)
                {
                    enemy.CharacterHealthComponent.TakeDamage(this._attackDamage);
                }
            }
        }
        
    }


    #endregion
    

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

