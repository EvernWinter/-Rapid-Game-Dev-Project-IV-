using System;
using System.Collections;
using UnityEngine;

public class CharacterAnimationBase : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _characterRenderer;

    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _damagedSprite;
    
    [SerializeField] protected Sprite _weaponSprite;

    public Action OnAttack;
    public Action OnDamaged;

    [SerializeField] private float _damagedDuration = 1f; // Time in seconds to display the damaged sprite
    [SerializeField] private float _hitRotationAngle = 10f; // Maximum angle for the hit effect
    [SerializeField] private float _hitRotationDuration = 0.2f; // Time to complete the rotation effect

    private Coroutine _damagedCoroutine; // Keeps track of the currently running coroutine

    protected virtual void Awake()
    {
        OnDamaged += PlayDamagedAnimation;
        OnAttack += PlayAttackAnimation;
    }
    
    // Start is called before the first frame update
    protected void Start()
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
        Debug.LogWarning($"{transform.parent.name} start an attack");
        // Implementation for attack animation if needed
    }

    protected virtual void PlayDamagedAnimation()
    {
        Debug.Log($"{transform.parent.name} was damaged");
        
        if (_damagedCoroutine != null)
        {
            return;
            //StopCoroutine(_damagedCoroutine); // Stop any currently running coroutine
        }

        _damagedCoroutine = StartCoroutine(DamagedAnimationCoroutine());
    }

    private IEnumerator DamagedAnimationCoroutine()
    {
        Debug.Log($"{transform.parent.name} was damaged (Inner)");
        
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
        
    [SerializeField] private bool _ignoreCharacterSpriteByTier; // If true, character sprites will not be updated by tier

    public virtual void SetUpSpriteAccordingToTier(CharacterEntity characterEntity)
    {
        CharacterEntity.CharacterSide side = characterEntity.characterSide;
        CharacterEntity.UnitType type = characterEntity.unitType;
        int tier = characterEntity.CharacterTierNumber;
        TierSpritesManager spriteList = TierSpritesManager.Instance;

        switch (type)
        {
            case CharacterEntity.UnitType.Swordman:
                if (!_ignoreCharacterSpriteByTier)
                {
                    _normalSprite = side == CharacterEntity.CharacterSide.Ally
                        ? spriteList.BlackSwordManSprites[tier]._normalCharacterSprite
                        : spriteList.WhiteSwordManSprites[0]._normalCharacterSprite;
                    _damagedSprite = side == CharacterEntity.CharacterSide.Ally
                        ? spriteList.BlackSwordManSprites[tier]._damagedCharacterSprite
                        : spriteList.WhiteSwordManSprites[0]._damagedCharacterSprite;
                }
                _weaponSprite = side == CharacterEntity.CharacterSide.Ally
                    ? spriteList.BlackSwordManSprites[tier]._weaponSprite
                    : spriteList.WhiteSwordManSprites[0]._weaponSprite;
                break;

            case CharacterEntity.UnitType.Archer:
                if (!_ignoreCharacterSpriteByTier)
                {
                    _normalSprite = side == CharacterEntity.CharacterSide.Ally
                        ? spriteList.BlackArcherSprites[tier]._normalCharacterSprite
                        : spriteList.WhiteArcherSprites[0]._normalCharacterSprite;
                    _damagedSprite = side == CharacterEntity.CharacterSide.Ally
                        ? spriteList.BlackArcherSprites[tier]._damagedCharacterSprite
                        : spriteList.WhiteArcherSprites[0]._damagedCharacterSprite;
                }
                _weaponSprite = side == CharacterEntity.CharacterSide.Ally
                    ? spriteList.BlackArcherSprites[tier]._weaponSprite
                    : spriteList.WhiteArcherSprites[0]._weaponSprite;
                break;

            case CharacterEntity.UnitType.Priest:
                if (!_ignoreCharacterSpriteByTier)
                {
                    _normalSprite = side == CharacterEntity.CharacterSide.Ally
                        ? spriteList.BlackPriestSprites[tier]._normalCharacterSprite
                        : spriteList.WhitePriestSprites[0]._normalCharacterSprite;
                    _damagedSprite = side == CharacterEntity.CharacterSide.Ally
                        ? spriteList.BlackPriestSprites[tier]._damagedCharacterSprite
                        : spriteList.WhitePriestSprites[0]._damagedCharacterSprite;
                }
                _weaponSprite = side == CharacterEntity.CharacterSide.Ally
                    ? spriteList.BlackPriestSprites[tier]._weaponSprite
                    : spriteList.WhitePriestSprites[0]._weaponSprite;
                break;

            case CharacterEntity.UnitType.Shield:
                if (!_ignoreCharacterSpriteByTier)
                {
                    _normalSprite = side == CharacterEntity.CharacterSide.Ally
                        ? spriteList.BlackShieldSprites[tier]._normalCharacterSprite
                        : spriteList.WhiteShieldSprites[0]._normalCharacterSprite;
                    _damagedSprite = side == CharacterEntity.CharacterSide.Ally
                        ? spriteList.BlackShieldSprites[tier]._damagedCharacterSprite
                        : spriteList.WhiteShieldSprites[0]._damagedCharacterSprite;
                }
                _weaponSprite = side == CharacterEntity.CharacterSide.Ally
                    ? spriteList.BlackShieldSprites[tier]._weaponSprite
                    : spriteList.WhiteShieldSprites[0]._weaponSprite;
                break;

            case CharacterEntity.UnitType.Horseman:
                if (!_ignoreCharacterSpriteByTier)
                {
                    _normalSprite = side == CharacterEntity.CharacterSide.Ally
                        ? spriteList.BlackHorseManSprites[tier]._normalCharacterSprite
                        : spriteList.WhiteHorseManSprites[0]._normalCharacterSprite;
                    _damagedSprite = side == CharacterEntity.CharacterSide.Ally
                        ? spriteList.BlackHorseManSprites[tier]._damagedCharacterSprite
                        : spriteList.WhiteHorseManSprites[0]._damagedCharacterSprite;
                }
                _weaponSprite = side == CharacterEntity.CharacterSide.Ally
                    ? spriteList.BlackHorseManSprites[tier]._weaponSprite
                    : spriteList.WhiteHorseManSprites[0]._weaponSprite;
                break;

            default:
                Debug.LogWarning($"Unhandled UnitType: {type}");
                break;
        }

    }

    // New: Set the normal sprite
    public void SetNormalSprite(Sprite normalSprite)
    {
        _normalSprite = normalSprite;

        // Immediately update the renderer if this is the current state
        if (_characterRenderer.sprite == _normalSprite || _damagedCoroutine == null)
        {
            _characterRenderer.sprite = _normalSprite;
        }
    }

    // New: Set the damaged sprite
    public void SetDamagedSprite(Sprite damagedSprite)
    {
        _damagedSprite = damagedSprite;

        // If the damaged animation is currently running, update the sprite
        if (_damagedCoroutine != null)
        {
            _characterRenderer.sprite = _damagedSprite;
        }
    }
}