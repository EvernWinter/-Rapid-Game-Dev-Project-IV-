using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion_Swordman : CharacterEntity
{
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

    // protected override void HandleState()
    // {
    //     switch (currentState)
    //     {
    //         case CharacterState.Run:
    //             Walk();
    //             break;
    //
    //         case CharacterState.Attack:
    //
    //             if (_targetDetector.enemiesInRange.Count > 0)
    //             {
    //                 Attack();
    //             }
    //             else
    //             {
    //                 currentState = CharacterState.Run;
    //             }
    //             break;
    //
    //         case CharacterState.Died:
    //             characterCollider.enabled = false;
    //             rb.velocity = Vector2.zero;
    //
    //             break;
    //     }
    // }

    protected override void Attack()
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
                    _targetDetector.enemiesInRange[0].CharacterHealthComponent.TakeDamage(this._attackDamage);
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
                    enemy.CharacterHealthComponent.TakeDamage(this._attackDamage);
                    _characterAnimator.OnAttack?.Invoke();
                    if (_characterSFX != null)
                    {
                        _characterSFX.OnAttack?.Invoke();
                    }
                }
            }
        }

    }
}
