using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierManager : MonoBehaviour
{

    public static TierManager Instance;
    
    [field: Header("Tier Stats")]
    [field: SerializeField] public List<CharacterStat> SwordManTierStats { get; private set; }
    [field: SerializeField] public List<CharacterStat> PriestTierStats { get; private set; }
    [field: SerializeField] public List<CharacterStat> HorseManTierStats { get; private set; }
    [field: SerializeField] public List<CharacterStat> ArcherTierStats { get; private set; }
    [field: SerializeField] public List<CharacterStat> ShieldTierStats { get; private set; }

    [field: Header("Modifier Stats")]
    [field: SerializeField] public CharacterStat SwordManModifier { get; private set; }
    [field: SerializeField] public CharacterStat PriestModifier { get; private set; }
    [field: SerializeField] public CharacterStat HorsemanModifier { get; private set; }
    [field: SerializeField] public CharacterStat ArcherModifier { get; private set; }
    [field: SerializeField] public CharacterStat ShieldModifier { get; private set; }

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
