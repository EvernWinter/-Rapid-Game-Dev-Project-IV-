using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebirthOrb : MonoBehaviour
{
    public enum UnitType { Swordman, Pirest, Horseman, Shield, Archer }
    public UnitType unitType;

    public float rebirthOrb_LifeTime = 10f;
    public float rebirthOrb_DestroyTime = 0f;

    [SerializeField] private GameObject[] minionPrefabs;

    private void Start()
    {
        rebirthOrb_DestroyTime = Time.time + rebirthOrb_LifeTime;
        RebirthManager.Instance.AddRebirthOrb(gameObject);
        //RebirthManager.Instance.rebirthOrbs.Add(gameObject);
    }

    private void Update()
    {
        if (Time.time > rebirthOrb_DestroyTime)
        {
            RebirthManager.Instance.RemoveRebirthOrb(gameObject);
            //RebirthManager.Instance.rebirthOrbs.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void Rebirth()
    {
        switch (unitType)
        {
            case UnitType.Swordman:
                GameObject minion_Swordman = Instantiate(minionPrefabs[0]);
                minion_Swordman.transform.position = gameObject.transform.position;
                break;

            case UnitType.Pirest:
                GameObject minion_Pirest = Instantiate(minionPrefabs[1]);
                minion_Pirest.transform.position = gameObject.transform.position;
                break;

            case UnitType.Horseman:
                GameObject minion_Horseman = Instantiate(minionPrefabs[2]);
                minion_Horseman.transform.position = gameObject.transform.position;
                break;

            case UnitType.Shield:
                GameObject minion_Shield = Instantiate(minionPrefabs[3]);
                minion_Shield.transform.position = gameObject.transform.position;
                break;

            case UnitType.Archer:
                GameObject minion_Archer = Instantiate(minionPrefabs[4]);
                minion_Archer.transform.position = gameObject.transform.position;
                break;
        }
        Destroy(gameObject);
    }
}
