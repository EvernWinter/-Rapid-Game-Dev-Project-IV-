using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Minion_Archer : CharacterEntity
{
    [SerializeField] private GameObject arrowPrefab;
    //[SerializeField] private float shootForce;
    [SerializeField] float initialSpeed = 10f;
    [SerializeField] private Transform target;

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

            if(_targetDetector.baseManagerInRange != null && _targetDetector.baseManagerInRange.GetComponent<BaseManager>().baseHealth > 0)
            {
                target = _targetDetector.baseManagerInRange.transform;
            }
            else if(_targetDetector.enemiesInRange[0].transform != null)
            {
                target = _targetDetector.enemiesInRange[0].transform;
            }
            
            GameObject arrow = Instantiate(arrowPrefab, gameObject.transform.position, Quaternion.identity);
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            arrow.GetComponent<Bullet>().damage = _attackDamage;

            bool isThisBulletForAlly = (characterSide == CharacterSide.Enemy);
            arrow.GetComponent<Bullet>().isThisBulletForAlly = isThisBulletForAlly;

            float distance = Vector2.Distance(gameObject.transform.position, target.position);
            float requiredSpeed = Mathf.Sqrt(distance * 9.8f);

            float angle = 0;

            if (characterSide == CharacterSide.Ally)
            {
                angle = 45f * Mathf.Deg2Rad; // Convert to radians
            }
            else if(characterSide == CharacterSide.Enemy)
            {
                angle = 135f * Mathf.Deg2Rad; // Convert to radians
            }
           


            Vector2 velocity = new Vector2(
                requiredSpeed * Mathf.Cos(angle),  // X component
                requiredSpeed * Mathf.Sin(angle)   // Y component
            );

            // Apply the velocity to the arrow
            rb.velocity = velocity;

            // Rotate the arrow to face the direction it's moving
            float rotationAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        }
    }
}
