using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : CharacterEntity
{
    protected override void Walk()
    {
        // Walk to the left
        rb.velocity = new Vector2(-MoveSpeed, rb.velocity.y);

        if (_targetDetector.enemiesInRange.Count > 0)
        {
            currentState = CharacterState.Attack;
        }

        base.Walk();
    }
}
