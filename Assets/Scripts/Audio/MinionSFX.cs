using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MinionSFX : MonoBehaviour
{

    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _attackSFX;
    [SerializeField] protected AudioClip _damagedSFX;
    [SerializeField] protected AudioClip _deadSFX;
    [SerializeField] public AudioClip[] _specialSFX;

    public Action OnAttack;
    public Action OnDamaged;
    public Action OnDead;

    protected virtual void Awake()
    {
        OnAttack += () => _audioSource.PlayOneShot(_attackSFX);
        OnDamaged += () => _audioSource.PlayOneShot(_deadSFX);
        OnDead += () => _audioSource.PlayOneShot(_deadSFX);
    }
}

