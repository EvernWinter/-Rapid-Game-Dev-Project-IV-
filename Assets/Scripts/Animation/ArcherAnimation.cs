using System.Collections;
using UnityEngine;

public class ArcherAnimation : CharacterAnimationBase
{
    [SerializeField] private GameObject _bowRenderer;

    [SerializeField] private float _aimRotationAngle = 35f; // Rotation angle during aiming
    [SerializeField] private float _rotationDuration = 0.2f; // Time it takes to rotate
    [SerializeField] private bool _isFacingRight = true; // Determines the character's facing direction

    private Coroutine _aimCoroutine;

    protected override void PlayAttackAnimation()
    {
        base.PlayAttackAnimation();

        if (_aimCoroutine != null)
        {
            StopCoroutine(_aimCoroutine); // Stop any ongoing aiming animation coroutine
        }

        // Trigger the aiming animation before attack
        _aimCoroutine = StartCoroutine(AimedStageAnimationCoroutine());
    }

    private IEnumerator AimedStageAnimationCoroutine()
    {
        // Calculate the target rotation based on the facing direction
        float targetAngle = _isFacingRight ? _aimRotationAngle : -_aimRotationAngle;
        float elapsedTime = 0f;

        // Store the original rotation for restoration
        Quaternion originalRotation = _bowRenderer.transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        // Step 1: Rotate to the target angle (aimed position)
        while (elapsedTime < _rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            _bowRenderer.transform.localRotation = Quaternion.Lerp(originalRotation, targetRotation, elapsedTime / _rotationDuration);
            yield return null;
        }

        _bowRenderer.transform.localRotation = targetRotation;

        // Hold the bow at the aimed position for a brief moment
        yield return new WaitForSeconds(0.5f); // Customize the hold duration as needed

        // Step 2: Rotate back to the original position
        elapsedTime = 0f;
        while (elapsedTime < _rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            _bowRenderer.transform.localRotation = Quaternion.Lerp(targetRotation, originalRotation, elapsedTime / _rotationDuration);
            yield return null;
        }

        _bowRenderer.transform.localRotation = originalRotation;

        // Clear the coroutine reference when done
        _aimCoroutine = null;
    }

    // Call this method to set the facing direction dynamically
    public void SetFacingDirection(bool isFacingRight)
    {
        _isFacingRight = isFacingRight;
    }
}
