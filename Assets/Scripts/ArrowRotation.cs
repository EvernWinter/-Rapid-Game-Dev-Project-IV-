using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        // Cache the Rigidbody2D component
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check if the arrow has any velocity
        if (_rigidbody.velocity.sqrMagnitude > 0.01f)
        {
            // Calculate the angle based on the arrow's velocity
            float rotationAngle = Mathf.Atan2(_rigidbody.velocity.y, _rigidbody.velocity.x) * Mathf.Rad2Deg;

            // Apply the rotation to the arrow
            transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        }
    }
}