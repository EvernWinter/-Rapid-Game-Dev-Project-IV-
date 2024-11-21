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
        if (_damagedCoroutine != null)
        {
            StopCoroutine(_damagedCoroutine); // Stop any currently running coroutine
        }

        _damagedCoroutine = StartCoroutine(DamagedAnimationCoroutine());
    }

    private IEnumerator DamagedAnimationCoroutine()
    {
        if (_characterRenderer != null && _damagedSprite != null)
        {
            _characterRenderer.sprite = _damagedSprite;
        }

        yield return new WaitForSeconds(_damagedDuration);

        if (_characterRenderer != null && _normalSprite != null)
        {
            _characterRenderer.sprite = _normalSprite;
        }

        _damagedCoroutine = null; // Clear the reference once the coroutine finishes
    }

    // You can expose this method to trigger damage animation from outside the script
    public void TriggerDamagedAnimation()
    {
        PlayDamagedAnimation();
    }
}