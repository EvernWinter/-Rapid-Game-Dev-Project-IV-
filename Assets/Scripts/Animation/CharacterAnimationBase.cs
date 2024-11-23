using System;
using System.Collections;
using UnityEngine;

public class CharacterAnimationBase : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _characterRenderer;

    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _damagedSprite;

    public Action OnAttack;
    public Action OnDamaged;

    [SerializeField] private float _damagedDuration = 1f; // Time in seconds to display the damaged sprite
    [SerializeField] private float _hitRotationAngle = 10f; // Maximum angle for the hit effect
    [SerializeField] private float _hitRotationDuration = 0.2f; // Time to complete the rotation effect

    private Coroutine _damagedCoroutine; // Keeps track of the currently running coroutine

    void Awake()
    {
        OnDamaged += PlayDamagedAnimation;
        OnAttack += PlayAttackAnimation;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Optional: Initialization logic if needed
    }

    // Update is called once per frame
    void Update()
    {
        // Optional: Logic for testing or animations
    }

    protected virtual void PlayAttackAnimation()
    {
        // Implementation for attack animation if needed
    }

    protected virtual void PlayDamagedAnimation()
    {
        Debug.Log($"{name} was damaged");
        
        if (_damagedCoroutine != null)
        {
            return;
            //StopCoroutine(_damagedCoroutine); // Stop any currently running coroutine
        }

        _damagedCoroutine = StartCoroutine(DamagedAnimationCoroutine());
    }

    private IEnumerator DamagedAnimationCoroutine()
    {
        if (_characterRenderer != null && _damagedSprite != null)
        {
            _characterRenderer.sprite = _damagedSprite;
        }

        yield return StartCoroutine(PlayHitReactionEffect());
        
        yield return new WaitForSeconds(_damagedDuration);

        if (_characterRenderer != null && _normalSprite != null)
        {
            _characterRenderer.sprite = _normalSprite;
        }

        _damagedCoroutine = null; // Clear the reference once the coroutine finishes
    }
    
    private IEnumerator PlayHitReactionEffect()
    {
        // Rotate the sprite to simulate a "hit" reaction
        float elapsedTime = 0f;
        Quaternion originalRotation = transform.localRotation;
        Quaternion hitRotation = Quaternion.Euler(0f, 0f, _hitRotationAngle);

        // Rotate to the hit angle
        while (elapsedTime < _hitRotationDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(originalRotation, hitRotation, elapsedTime / (_hitRotationDuration / 2));
            yield return null;
        }

        elapsedTime = 0f;

        // Rotate back to the original position
        while (elapsedTime < _hitRotationDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(hitRotation, originalRotation, elapsedTime / (_hitRotationDuration / 2));
            yield return null;
        }

        transform.localRotation = originalRotation; // Ensure the rotation resets completely
    }


    // You can expose this method to trigger damage animation from outside the script
    public void TriggerDamagedAnimation()
    {
        PlayDamagedAnimation();
    }
}