using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource minionSpawnSound1;
    [SerializeField] private AudioSource minionSpawnSound2;
    [SerializeField] private AudioSource minionDiedSound;
    [SerializeField] private AudioSource rebirthSound1;
    [SerializeField] private AudioSource rebirthSound2;

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlayMinionDiedSound()
    {
        minionDiedSound.Play();
    }

    public void PlayRebirthSound()
    {
        rebirthSound1.Play();
        rebirthSound2.Play();
    }

    public void PlayMinionSpawnSound()
    {
        int rnd = Random.Range(0, 2);

        if(rnd == 0)
        {
            minionSpawnSound1.Play();
        }
        else if(rnd == 1)
        {
            minionSpawnSound2.Play();
        }
    }
}
