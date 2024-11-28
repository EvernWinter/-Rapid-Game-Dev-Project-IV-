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
        base.Attack();
    }
}
