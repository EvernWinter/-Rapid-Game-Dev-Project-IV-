using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;
    
    [field: SerializeField] public ManaSystem manaSystem { get; private set; }
    [field: SerializeField] public MoneySystem moneySystem { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            manaSystem = new ManaSystem(0, 1500, 100);  
            moneySystem = new MoneySystem(100);
        }
        else
        {
            Destroy(this);
        }
    }
    
    void Start()
    {

    }

    void Update()
    {
        if (manaSystem != null)
        {
            manaSystem.RegenerateMana(Time.deltaTime);
            //Debug.Log("Current Mana: " + manaSystem.CurrentMana);
        }
        else
        {
            Debug.LogError("ManaSystem is null!");
        }
    }
}

[Serializable]
public class ManaSystem
{
    [SerializeField] private int _currentMana;
    [SerializeField] private int _maxMana;
    [SerializeField] private int _manaRegenRate;

    public ManaSystem(int currentMana, int maxMana, int manaRegenRate)
    {
        _currentMana = currentMana;
        _maxMana = maxMana;
        _manaRegenRate = manaRegenRate;
    }

    public int CurrentMana => _currentMana;
    public int MaxMana => _maxMana;

    public void SpendMana(int amount)
    {
        if (amount <= _currentMana)
        {
            _currentMana -= amount;
            Debug.Log("Mana spent: " + amount);
        }
        else
        {
            Debug.Log("Not enough mana.");
        }
    }

    public void RegenerateMana(float deltaTime)
    {
        float manaToRegenerate = Mathf.RoundToInt(_manaRegenRate * deltaTime);
        //Debug.Log($"Regen Rate: {_manaRegenRate}, DeltaTime: {deltaTime}, Mana to Regen: {manaToRegenerate}");

        _currentMana += Mathf.RoundToInt(_manaRegenRate * deltaTime);
        _currentMana = Mathf.Clamp(_currentMana, 0, _maxMana);

        //Debug.Log($"Current Mana after Regen: {_currentMana}");
    }


    public void SetMaxMana(int newMax)
    {
        _maxMana = newMax;
        _currentMana = Mathf.Clamp(_currentMana, 0, _maxMana);
    }
    
    public bool HasEnoughMana(int amount)
    {
        return _currentMana >= amount;
    }
}

[Serializable]
public class MoneySystem
{
    [SerializeField] private int _playerMoney;

    public MoneySystem(int initialMoney)
    {
        _playerMoney = initialMoney;
    }

    public int PlayerMoney => _playerMoney;

    public void AddMoney(int amount)
    {
        _playerMoney += amount;
        Debug.Log("Money added: " + amount);
    }

    public void SpendMoney(int amount)
    {
        if (amount <= _playerMoney)
        {
            _playerMoney -= amount;
            Debug.Log("Money spent: " + amount);
        }
        else
        {
            Debug.Log("Not enough money.");
        }
    }
    
    public bool HasEnoughMoney(int amount)
    {
        return _playerMoney >= amount;
    }
}
