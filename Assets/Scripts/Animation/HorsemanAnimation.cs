using System.Collections;
using UnityEngine;

public class HorsemanAnimation : CharacterAnimationBase
{
    [SerializeField] private GameObject _lanceRenderer; // The object representing the lance
    [SerializeField] private float _thrustDistance = 1.5f; // Distance the lance moves forward
    [SerializeField] private float _thrustDuration = 0.2f; // Time it takes to thrust
    [SerializeField] private bool _isFacingRight = true; // Determines the character's facing direction

    private Coroutine _attackCoroutine;

    // Method to trigger the attack animation
    [ContextMenu("Play Attack Animation")]
    protected override void PlayAttackAnimation()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine); // Stop any ongoing attack animation coroutine
        }

        _attackCoroutine = StartCoroutine(AttackAnimationCoroutine());
    }

    private IEnumerator AttackAnimationCoroutine()
    {
        // Calculate the lance's original and target positions
        Vector3 originalPosition = _lanceRenderer.transform.localPosition;
        Vector3 thrustOffset = _isFacingRight ? new Vector3(_thrustDistance, 0, 0) : new Vector3(-_thrustDistance, 0, 0);
        Vector3 targetPosition = originalPosition + thrustOffset;

        float elapsedTime = 0f;

        // Smoothly move the lance forward
        while (elapsedTime < _thrustDuration)
        {
            elapsedTime += Time.deltaTime;
            _lanceRenderer.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / _thrustDuration);
            yield return null;
        }

        _lanceRenderer.transform.localPosition = targetPosition;

        // Hold the lance in the extended position briefly
        yield return new WaitForSeconds(0.1f);

        // Smoothly retract the lance to its original position
        elapsedTime = 0f;
        while (elapsedTime < _thrustDuration)
        {
            elapsedTime += Time.deltaTime;
            _lanceRenderer.transform.localPosition = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / _thrustDuration);
            yield return null;
        }

        _lanceRenderer.transform.localPosition = originalPosition;
        _attackCoroutine = null; // Clear the coroutine reference when done
    }

    // Method to set the facing direction dynamically
    public void SetFacingDirection(bool isFacingRight)
    {
        _isFacingRight = isFacingRight;
    }
}
