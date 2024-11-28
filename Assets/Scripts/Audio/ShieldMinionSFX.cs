using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMinionSFX : MinionSFX
{
    [SerializeField] private CharacterEntity _characterEntity;
    
    protected override void Awake()
    {
        OnAttack += () => _audioSource.PlayOneShot(_attackSFX);
        OnDamaged += () => Invoke("PlayDamagedSFX", 0.15f);
        OnDead += () => _audioSource.PlayOneShot(_deadSFX);
    }

    public void PlayDamagedSFX()
    {
        _audioSource.pitch = Random.Range(0.78f, 1f);
        switch (_characterEntity.CharacterCurrentTier)
        {
            case CharacterEntity.CharacterTier.T0:
                _audioSource.PlayOneShot(_specialSFX[0]);
                break;
            default:
                _audioSource.PlayOneShot(_damagedSFX);
                break;
        }
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
