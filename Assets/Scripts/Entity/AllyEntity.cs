using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyEntity : CharacterEntity
{
    protected override void OnDeployed()
    {
        if (GameData.Instance.manaSystem.HasEnoughMana(_deploymentCost))
        {
            base.OnDeployed();
        }
        else
        {
            Debug.Log($"Player does not have enough money to spawn {_entityName}");
        }
    }

    protected override void Walk()
    {
        // Walk to the right
        rb.velocity = new Vector2(MoveSpeed, rb.velocity.y);

        if (_targetDetector.enemiesInRange.Count > 0)
        {
            currentState = CharacterState.Attack;
        }

        base.Walk();
    }
}
