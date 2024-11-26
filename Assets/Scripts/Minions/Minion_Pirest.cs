using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Spine.Unity.Examples.BasicPlatformerController;

public class Minion_Pirest : CharacterEntity
{
    [SerializeField] private float healPoint = 0f;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void UpdateEntity()
    {
        if (_isOnField)
        {
            HandleState();
        }

        base.UpdateEntity();
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case CharacterState.Run:
                Walk();
                break;

            case CharacterState.Attack:

                if (_targetDetector.enemiesInRange.Count > 0)
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

    private void Attack()
    {
        rb.velocity = Vector2.zero;

        if (Time.time > nextAttack)
        {
            nextAttack = Time.time + attackCooldownDuration;

            if (!_isAOE)
            {
                // Single target attack
                if (_targetDetector.alliesInRange.Count > 0)
                {
                    _targetDetector.alliesInRange[0].CharacterHealthComponent.Heal(this.healPoint);
                    
                }
                else if (_targetDetector.enemiesInRange.Count > 0)
                {
                    //_targetDetector.enemiesInRange[0].CharacterHealthComponent.TakeDamage(this._attackDamage);
                }
            }
            else
            {
                if (_targetDetector.alliesInRange.Count > 0)
                {
                    bool attackInitiated = false;
                    
                    foreach (var ally in _targetDetector.alliesInRange)
                    {
                        ally.CharacterHealthComponent.Heal(this.healPoint);
                        attackInitiated = true;
                    }

                    if (attackInitiated)
                    {
                        _characterAnimator.OnAttack?.Invoke();
                    }
                }
                else
                {
                    // AOE attack, hit all enemies in range
                    foreach (var enemy in _targetDetector.enemiesInRange)
                    {
                        //enemy.CharacterHealthComponent.TakeDamage(this._attackDamage);
                    }
                }
            }
        }

    }
}
