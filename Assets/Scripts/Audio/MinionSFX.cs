using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MinionSFX : MonoBehaviour
{

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _attackSFX;
    [SerializeField] private AudioClip _damagedSFX;
    [SerializeField] private AudioClip _deadSFX;
    [SerializeField] public AudioClip[] _specialSFX;
    
    public Action OnAttack;
    public Action OnDamaged;
    public Action OnDead;

    void Awake()
    {
        OnAttack += () => _audioSource.PlayOneShot(_attackSFX);
        OnDamaged += () => _audioSource.PlayOneShot(_deadSFX);
        OnDead += () => _audioSource.PlayOneShot(_deadSFX);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
