using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class TargetDetector : MonoBehaviour
{
    public List<CharacterEntity> enemiesInRange = new List<CharacterEntity>();  // List to store enemies in attack range
    public float detectionRadius = 5f; // Radius for detecting enemies

    private CircleCollider2D detectionCollider;
    private bool isAlly;

    void Start()
    {
        // Set up the detection collider
        detectionCollider = GetComponent<CircleCollider2D>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRadius;

        // Determine if this entity is an ally or enemy based on component
        isAlly = GetComponent<AllyEntity>() != null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other object is an enemy based on this entity type
        CharacterEntity characterEntity = other.GetComponent<CharacterEntity>();
        if (characterEntity != null)
        {
            if (isAlly && characterEntity.GetComponent<EnemyEntity>() != null)
            {
                // This entity is an ally, and the other is an enemy
                enemiesInRange.Add(characterEntity);
                Debug.Log("Enemy entered range: " + other.name);
            }
            else if (!isAlly && characterEntity.GetComponent<AllyEntity>() != null)
            {
                // This entity is an enemy, and the other is an ally
                enemiesInRange.Add(characterEntity);
                Debug.Log("Enemy entered range: " + other.name);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        CharacterEntity characterEntity = other.GetComponent<CharacterEntity>();
        if (characterEntity != null && enemiesInRange.Contains(characterEntity))
        {
            enemiesInRange.Remove(characterEntity);
            Debug.Log("Enemy exited range: " + other.name);
        }
    }

    // Optional: Get the nearest enemy in range (if needed)
    public CharacterEntity GetNearestEnemy()
    {
        CharacterEntity nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (CharacterEntity enemy in enemiesInRange)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}
