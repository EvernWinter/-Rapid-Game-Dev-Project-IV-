using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isThisBulletForAlly = false;
    public float damage = 0f;
        
    private void OnTriggerEnter2D(Collider2D other)
    {
        if((other.gameObject.layer == LayerMask.NameToLayer("Enemy")) && !isThisBulletForAlly)
        {
            if(other.gameObject.GetComponent<CharacterEntity>() != null)
            other.gameObject.GetComponent<CharacterEntity>().CharacterHealthComponent.TakeDamage(damage);

            if (other.gameObject.GetComponent<BaseManager>() != null)
                other.gameObject.GetComponent<BaseManager>().TakeDamage(damage);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ally") && isThisBulletForAlly)
        {
            if (other.gameObject.GetComponent<CharacterEntity>() != null)
                other.gameObject.GetComponent<CharacterEntity>().CharacterHealthComponent.TakeDamage(damage);

            if (other.gameObject.GetComponent<BaseManager>() != null)
                other.gameObject.GetComponent<BaseManager>().TakeDamage(damage);
        }
    }
}
