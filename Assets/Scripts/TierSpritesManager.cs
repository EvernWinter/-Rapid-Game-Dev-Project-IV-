using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierSpritesManager : MonoBehaviour
{
    public static TierSpritesManager Instance;
    
    [field: Header("White Tier Sprite")]
    [field: SerializeField] public List<TierSprite> WhiteSwordManSprites{ get; private set; }
    [field: SerializeField] public List<TierSprite> WhitePriestSprites { get; private set; }
    [field: SerializeField] public List<TierSprite> WhiteHorseManSprites { get; private set; }
    [field: SerializeField] public List<TierSprite> WhiteArcherSprites { get; private set; }
    [field: SerializeField] public List<TierSprite> WhiteShieldSprites { get; private set; }

    [field: Header("Black Tier Sprite")]
    [field: SerializeField] public List<TierSprite> BlackSwordManSprites{ get; private set; }
    [field: SerializeField] public List<TierSprite> BlackPriestSprites { get; private set; }
    [field: SerializeField] public List<TierSprite> BlackHorseManSprites { get; private set; }
    [field: SerializeField] public List<TierSprite> BlackArcherSprites { get; private set; }
    [field: SerializeField] public List<TierSprite> BlackShieldSprites { get; private set; }

    void Awake()
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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class TierSprite
{
    [field: SerializeField] public Sprite _normalCharacterSprite { get; private set; }
    [field: SerializeField] public Sprite _damagedCharacterSprite { get; private set; }
    [field: SerializeField] public Sprite _weaponSprite { get; private set; }
}
