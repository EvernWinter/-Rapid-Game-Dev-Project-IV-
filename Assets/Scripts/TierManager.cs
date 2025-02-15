using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitTier
{
    Common,
    Silver,
    Gold,
    Platinum
}

public enum MinionType
{
    SwordMan,
    Archer,
    Sheild,
    Priest,
    HorseMan,
}

public class TierManager : MonoBehaviour
{

    public static TierManager Instance;

    [field: Header("Current Black Tiers")]
    [field: SerializeField] public int BlackSwordManTier { get; private set; } = 0;
    [field: SerializeField] public int BlackPriestTier { get; private set; } = 0;
    [field: SerializeField] public int BlackHorsemanTier { get; private set; } = 0;
    [field: SerializeField] public int BlackArcherTier { get; private set; } = 0;
    [field: SerializeField] public int BlackShieldTier { get; private set; } = 0;
    
    [field: Header("Current White Tiers")]
    [field: SerializeField] public int WhiteSwordManTier { get; private set; } = 0;
    [field: SerializeField] public int WhitePriestTier { get; private set; } = 0;
    [field: SerializeField] public int WhiteHorsemanTier { get; private set; } = 0;
    [field: SerializeField] public int WhiteArcherTier { get; private set; } = 0;
    [field: SerializeField] public int WhiteShieldTier { get; private set; } = 0;
    
    public Dictionary<MinionType, UnitTier> unitTiers;



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
        unitTiers = new Dictionary<MinionType,UnitTier>
        {
            { MinionType.SwordMan, UnitTier.Common },
            { MinionType.Priest,  UnitTier.Common },
            { MinionType.HorseMan,  UnitTier.Common },
            { MinionType.Archer,  UnitTier.Common },
            { MinionType.Sheild,  UnitTier.Common }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Method to map UnitTier to integer
    public int GetTierAsInt(UnitTier tier)
    {
        return (int)tier;
    }

    // Method to map integer to UnitTier
    public UnitTier GetTierFromInt(int tierValue)
    {
        if (System.Enum.IsDefined(typeof(UnitTier), tierValue))
        {
            return (UnitTier)tierValue;
        }
        throw new System.ArgumentOutOfRangeException($"Tier value {tierValue} is not valid.");
    }
    
    public int GetWhiteTier(MinionType minion)
    {
        return minion switch
        {
            MinionType.SwordMan => WhiteSwordManTier,
            MinionType.Priest => WhitePriestTier,
            MinionType.HorseMan => WhiteHorsemanTier,
            MinionType.Archer => WhiteArcherTier,
            MinionType.Sheild => WhiteShieldTier,
            _ => 0
        };
    }

    public void UpgradeUnitTierWhite(MinionType minion)
    {
        int currentTier = GetWhiteTier(minion);
        int maxTier = 4;

        if (currentTier < maxTier)
        {
            switch (minion)
            {
                case MinionType.SwordMan:
                    WhiteSwordManTier++;
                    break;
                case MinionType.Priest:
                    WhitePriestTier++;
                    break;
                case MinionType.HorseMan:
                    WhiteHorsemanTier++;
                    break;
                case MinionType.Archer:
                    WhiteArcherTier++;
                    break;
                case MinionType.Sheild:
                    WhiteShieldTier++;
                    break;
            }

            Debug.Log($"{minion} upgraded to tier {currentTier + 1}");
        }
        else
        {
            Debug.Log($"{minion} is already at the maximum tier!");
        }
    }
    
    public void UpgradeUnitTier(MinionType minion)
    {
        UnitTier currentTier = unitTiers[minion];
        int nextTierValue = GetTierAsInt(currentTier) + 1;

        switch (minion)
        {
            case MinionType.SwordMan:
                if (System.Enum.IsDefined(typeof(UnitTier), nextTierValue))
                {
                    unitTiers[minion] = GetTierFromInt(nextTierValue);
                    BlackSwordManTier = nextTierValue;
                    Debug.Log($"{minion} upgraded to tier {unitTiers[minion]}");
                }
                else
                {
                    Debug.Log($"{minion} is already at the maximum tier!");
                }
                break;
            case MinionType.Priest:
                if (System.Enum.IsDefined(typeof(UnitTier), nextTierValue))
                {
                    unitTiers[minion] = GetTierFromInt(nextTierValue);
                    BlackPriestTier = nextTierValue;
                    Debug.Log($"{minion} upgraded to tier {unitTiers[minion]}");
                }
                else
                {
                    Debug.Log($"{minion} is already at the maximum tier!");
                }
                break;
            case MinionType.HorseMan:
                if (System.Enum.IsDefined(typeof(UnitTier), nextTierValue))
                {
                    unitTiers[minion] = GetTierFromInt(nextTierValue);
                    BlackHorsemanTier = nextTierValue;
                    Debug.Log($"{minion} upgraded to tier {unitTiers[minion]}");
                }
                else
                {
                    Debug.Log($"{minion} is already at the maximum tier!");
                }
                break;
            case MinionType.Archer:
                if (System.Enum.IsDefined(typeof(UnitTier), nextTierValue))
                {
                    unitTiers[minion] = GetTierFromInt(nextTierValue);
                    BlackArcherTier = nextTierValue;
                    Debug.Log($"{minion} upgraded to tier {unitTiers[minion]}");
                }
                else
                {
                    Debug.Log($"{minion} is already at the maximum tier!");
                }
                break;
            case MinionType.Sheild:
                if (System.Enum.IsDefined(typeof(UnitTier), nextTierValue))
                {
                    unitTiers[minion] = GetTierFromInt(nextTierValue);
                    BlackShieldTier = nextTierValue;
                    Debug.Log($"{minion} upgraded to tier {unitTiers[minion]}");
                }
                else
                {
                    Debug.Log($"{minion} is already at the maximum tier!");
                }
                break;
        }
    }
}

