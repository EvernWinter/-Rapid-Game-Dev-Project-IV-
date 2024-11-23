using System.Collections;
using UnityEngine;

public class SwordManAnimation : CharacterAnimationBase
{
    [SerializeField] private GameObject _swordRenderer;

    [SerializeField] private float _rotationAngle = 30f; // Rotation angle during attack
    [SerializeField] private float _rotationDuration = 0.2f; // Time it takes to rotate
    [SerializeField] private bool _isFacingRight = true; // Determines the character's facing direction

    private Coroutine _attackCoroutine;

    protected override void PlayAttackAnimation()
    {
        base.PlayAttackAnimation();

        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine); // Stop any ongoing attack animation coroutine
        }

        _attackCoroutine = StartCoroutine(AttackAnimationCoroutine());
    }

    private IEnumerator AttackAnimationCoroutine()
    {
        // Calculate the target rotation based on the facing direction
        float targetAngle = _isFacingRight ? _rotationAngle : -_rotationAngle;
        float extraDownAngle = _isFacingRight ? -10f : 10f; // Slightly below the original position
        float elapsedTime = 0f;

        // Store the original rotation for restoration
        Quaternion originalRotation = _swordRenderer.transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        Quaternion extraDownRotation = Quaternion.Euler(0f, 0f, extraDownAngle);

        // Step 1: Rotate to the target angle (attack position)
        while (elapsedTime < _rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            _swordRenderer.transform.localRotation = Quaternion.Lerp(originalRotation, targetRotation, elapsedTime / _rotationDuration);
            yield return null;
        }

        _swordRenderer.transform.localRotation = targetRotation;

        // Hold the sword at the attack position for a brief moment
        yield return new WaitForSeconds(0.1f);

        // Step 2: Rotate slightly below the original position (extra down angle)
        elapsedTime = 0f;
        while (elapsedTime < _rotationDuration / 2) // Shorter duration for downward swing
        {
            elapsedTime += Time.deltaTime;
            _swordRenderer.transform.localRotation = Quaternion.Lerp(targetRotation, extraDownRotation, elapsedTime / (_rotationDuration / 2));
            yield return null;
        }

        _swordRenderer.transform.localRotation = extraDownRotation;

        // Step 3: Smoothly rotate back to the original position
        elapsedTime = 0f;
        while (elapsedTime < _rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            _swordRenderer.transform.localRotation = Quaternion.Lerp(extraDownRotation, originalRotation, elapsedTime / _rotationDuration);
            yield return null;
        }

        _swordRenderer.transform.localRotation = originalRotation;

        // Clear the coroutine reference when done
        _attackCoroutine = null;
    }






    // Call this method to set the facing direction dynamically
    public void SetFacingDirection(bool isFacingRight)
    {
        _isFacingRight = isFacingRight;
    }
}
