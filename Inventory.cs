using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public int maxInventorySize = 6;
    public List<Item> items = new List<Item>();

    public GameObject inventoryPanel; 
    public GameObject slotPrefab; 

    private List<Image> itemSlots = new List<Image>(); 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerateInventorySlots(); 
    }

    private void GenerateInventorySlots()
    {
        for (int i = 0; i < maxInventorySize; i++)
        {
            GameObject slot = Instantiate(slotPrefab, inventoryPanel.transform);
            Image slotImage = slot.GetComponent<Image>();
            itemSlots.Add(slotImage);
            slotImage.enabled = false;
        }
    }

    public bool AddItem(Item newItem)
    {
        if (items.Count >= maxInventorySize)
        {
            Debug.Log("Inventář je plný!");
            return false;
        }

        items.Add(newItem);
        UpdateUI();
        return true;
    }




    public bool HasRequiredItems(List<Item> requiredItems)
    {
        foreach (Item requiredItem in requiredItems)
        {
            if (!items.Contains(requiredItem))
            {
                return false; 
            }
        }
        return true; 
    }


    public void RemoveItems(List<Item> itemsToRemove)
    {
        foreach (Item item in itemsToRemove)
        {
            int count = items.FindAll(i => i == item).Count;  
            for (int i = 0; i < count; i++)
            {
                RemoveItemStats(item);  
                items.Remove(item);  
            }
        }
        UpdateUI();
    }

    private void RemoveItemStats(Item item)
    {
        PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        EsScript esScript = GameObject.FindGameObjectWithTag("Player").GetComponent<EsScript>();

        playerHealth.maxHealth -= item.bonusHealth;
        playerHealth.currentHealth -= item.bonusHealth;
        playerHealth.regenRate -= item.bonusRegen;
        esScript.fireRate -= item.bonusAttackSpeed;
        esScript.baseDamage -= item.bonusDamage;
       
        playerHealth.AttackDamageArmor -= item.bonusAttackDamageArmor;

        
        if (playerHealth.currentHealth > playerHealth.maxHealth)
            playerHealth.currentHealth = playerHealth.maxHealth;

        playerHealth.UpdateUI();
    }




    public bool UpgradeItem(Item baseItem)
    {
        if (baseItem.upgradedItem == null || baseItem.requiredItems.Count == 0)
        {
            Debug.Log("Tento item nelze upgradovat!");
            return false;
        }

        
        if (HasRequiredItems(baseItem.requiredItems))
        {
            
            RemoveItems(baseItem.requiredItems);

            
            items.Add(baseItem.upgradedItem);
            Debug.Log($"Item {baseItem.itemName} byl upgradován na {baseItem.upgradedItem.itemName}");

            UpdateUI(); 
            return true;
        }
        else
        {
            Debug.Log("Nemáš potřebné itemy na upgrade!");
            return false;
        }
    }


    public void UpdateUI()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (i < items.Count)
            {
                itemSlots[i].sprite = items[i].itemIcon; 
                itemSlots[i].enabled = true;
            }
            else
            {
                itemSlots[i].sprite = null;
                itemSlots[i].enabled = false;
            }
        }
    }
}
