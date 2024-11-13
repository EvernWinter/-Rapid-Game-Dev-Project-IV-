using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType
{
    
}

public enum UpgradeRarity
{
    
}
public class Reward : MonoBehaviour
{
    [SerializeField] public UpgradeType upgradeType;
    [SerializeField] private Image upgradeImage;
    [SerializeField] private Sprite[] upgradeSprite;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void UpdateButton()
    {
        switch (upgradeType)
        {
            case UpgradeType.1:
                upgradeImage.sprite = upgradeSprite[0];
                break;
            case UpgradeType.2:
                upgradeImage.sprite = upgradeSprite[1];
                break;
            case UpgradeType.3:
                upgradeImage.sprite = upgradeSprite[2];
                break;
            case UpgradeType.4:
                upgradeImage.sprite = upgradeSprite[3];
                break;
            case UpgradeType.5:
                upgradeImage.sprite = upgradeSprite[4];
                break;
        }
    }*/
    /*public void ChooseUpgrade()
    {
        switch (upgradeType)
        {
            case UpgradeType.1:
                
                break;
            case UpgradeType.2:
                
                break;
            case UpgradeType.3:
                
                break;
            case UpgradeType.4:
                
                break;
            case UpgradeType.5:
                
                break;
        }
    }*/
}
