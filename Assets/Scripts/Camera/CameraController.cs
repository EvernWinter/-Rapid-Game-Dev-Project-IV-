using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            //transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);
            rb.velocity = new Vector2( -moveSpeed, rb.velocity.y);
        }
        else if(Input.GetKeyUp(KeyCode.A))
        {
            rb.velocity = Vector2.zero;
        }
        
        if(Input.GetKey(KeyCode.D))
        {
            //transform.position += new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            rb.velocity = Vector2.zero;
        }
    }
}
