using System.Collections;
using UnityEngine;

public class PriestAnimation : CharacterAnimationBase
{
    [SerializeField] private Transform wandTransform; // Reference to the wand's Transform
    [SerializeField] private ParticleSystem wandEffect; // Visual particle effect for wand attacks
    [SerializeField] private float wandLiftHeight = 0.2f; // Height to lift the wand
    [SerializeField] private float wandLiftDuration = 0.2f; // Duration of the wand lift animation

    private Vector3 _originalWandPosition;

    protected override void Awake()
    {
        base.Awake();
        
        if (wandTransform != null)
        {
            _originalWandPosition = wandTransform.localPosition;
        }
    }

    [ContextMenu("Play Animation")]
    protected override void PlayAttackAnimation()
    {
        base.PlayAttackAnimation(); // Optionally log or trigger a generic attack

        if (wandEffect != null)
        {
            //wandEffect.Play(); // Emit particles for the wand effect
        }

        if (wandTransform != null)
        {
            StartCoroutine(WandLiftAnimation());
        }
    }

    private IEnumerator WandLiftAnimation()
    {
        // Move wand upward
        Vector3 targetPosition = _originalWandPosition + new Vector3(0f, wandLiftHeight, 0f);
        float elapsedTime = 0f;

        while (elapsedTime < wandLiftDuration)
        {
            elapsedTime += Time.deltaTime;
            wandTransform.localPosition = Vector3.Lerp(_originalWandPosition, targetPosition, elapsedTime / wandLiftDuration);
            yield return null;
        }

        // Return wand to original position
        elapsedTime = 0f;
        while (elapsedTime < wandLiftDuration)
        {
            elapsedTime += Time.deltaTime;
            wandTransform.localPosition = Vector3.Lerp(targetPosition, _originalWandPosition, elapsedTime / wandLiftDuration);
            yield return null;
        }

        wandTransform.localPosition = _originalWandPosition;
    }
}