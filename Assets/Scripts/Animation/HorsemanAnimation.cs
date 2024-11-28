using System.Collections;
using UnityEngine;

public class HorsemanAnimation : CharacterAnimationBase
{
    [SerializeField] private Transform _lanceOrigin;
    [SerializeField] private GameObject _lanceRenderer; // The object representing the lance
    [SerializeField] private float _thrustDistance = 1f; // Distance the lance moves forward
    [SerializeField] private float _thrustDuration = 0.2f; // Time it takes to thrust
    [SerializeField] private bool _isFacingRight = true; // Determines the character's facing direction

    private Coroutine _attackCoroutine;

    // Method to trigger the attack animation
    [ContextMenu("Play Attack Animation")]
    protected override void PlayAttackAnimation()
    {
        Debug.Log("Horseman Attack Animation Triggered");
        base.PlayAttackAnimation();

        if (_lanceRenderer == null)
        {
            Debug.LogError("Lance Renderer is missing!");
            return;
        }

        if (_attackCoroutine != null)
        {
            Debug.Log("Stopping existing attack animation coroutine");
            StopCoroutine(_attackCoroutine);
        }

        _attackCoroutine = StartCoroutine(AttackAnimationCoroutine());
    }

    private IEnumerator AttackAnimationCoroutine()
    {
        // Use lanceOrigin as the fixed reference
        Vector3 originalPosition = _lanceOrigin.localPosition;
        _lanceRenderer.transform.localPosition = originalPosition;

        Vector3 thrustOffset = _isFacingRight ? new Vector3(_thrustDistance, 0, 0) : new Vector3(-_thrustDistance, 0, 0);
        Vector3 targetPosition = originalPosition + thrustOffset;

        float elapsedTime = 0f;

        // Smoothly move the lance forward
        while (elapsedTime < _thrustDuration)
        {
            elapsedTime += Time.deltaTime;
            _lanceRenderer.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, Mathf.Clamp01(elapsedTime / _thrustDuration));
            yield return null;
        }

        // Ensure exact positioning after the forward motion
        _lanceRenderer.transform.localPosition = targetPosition;

        // Hold the lance in the extended position briefly
        yield return new WaitForSeconds(0.1f);

        // Smoothly retract the lance to its original position
        elapsedTime = 0f;
        while (elapsedTime < _thrustDuration)
        {
            elapsedTime += Time.deltaTime;
            _lanceRenderer.transform.localPosition = Vector3.Lerp(targetPosition, originalPosition, Mathf.Clamp01(elapsedTime / _thrustDuration));
            yield return null;
        }

        // Ensure exact reset to the original position
        _lanceRenderer.transform.localPosition = originalPosition;

        _attackCoroutine = null; // Clear coroutine reference when done
    }

    // Method to set the facing direction dynamically
    public void SetFacingDirection(bool isFacingRight)
    {
        _isFacingRight = isFacingRight;
    }
}
