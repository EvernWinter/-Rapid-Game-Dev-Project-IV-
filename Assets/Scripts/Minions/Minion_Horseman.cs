using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion_Horseman : CharacterEntity
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
}
