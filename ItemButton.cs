using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemButton : MonoBehaviour
{
    public Item item;
    
    public Button buyButton;
    public ItemInfoDisplay infoDisplay;
    private PlayerHealth playerHealth;
    private EsScript esScript;
    private InventoryManager inventoryManager;

    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        esScript = GameObject.FindGameObjectWithTag("Player").GetComponent<EsScript>();
        inventoryManager = InventoryManager.instance;
        

        buyButton.onClick.AddListener(BuyOrUpgradeItem);

        EventTrigger trigger = GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener((eventData) => {
            if (infoDisplay != null && item != null)
                infoDisplay.DisplayItemInfo(item);
        });
        trigger.triggers.Add(entry);
    }

    void BuyOrUpgradeItem()
    {
        if (item.requiredItems.Count > 0) 
        {
            UpgradeItem();
        }
        else
        {
            BuyItem();
        }
    }

    void BuyItem()
    {
        if (playerHealth.gold >= item.price)
        {
            if (inventoryManager.AddItem(item))
            {
                playerHealth.gold -= item.price;
                ApplyItemEffects(item);
                
                playerHealth.UpdateUI();
            }
        }
        else
        {
            Debug.Log("Nedostatek zlata!");
        }
    }

    void UpgradeItem()
    {
        if (playerHealth.gold >= item.price)
        {
            if (inventoryManager.UpgradeItem(item))
            {
                playerHealth.gold -= item.price;
                ApplyItemEffects(item.upgradedItem);
                
                playerHealth.UpdateUI();
            }
        }
        else
        {
            Debug.Log("Nedostatek zlata na upgrade!");
        }
    }

    void ApplyItemEffects(Item appliedItem)
    {
        playerHealth.maxHealth += appliedItem.bonusHealth;
        playerHealth.currentHealth += appliedItem.bonusHealth;
        playerHealth.regenRate += appliedItem.bonusRegen;
        esScript.fireRate += appliedItem.bonusAttackSpeed;
        esScript.baseDamage += appliedItem.bonusDamage;
        
        playerHealth.AttackDamageArmor += appliedItem.bonusAttackDamageArmor;
    }
}
