using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Item", menuName = "Shop/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int price;
    public float bonusHealth;
    public float bonusRegen;
    public float bonusAttackSpeed;
    public float bonusDamage;
    public float bonusAttackDamageArmor;
    public Sprite itemIcon;
    [Header("Upgrade System")]
    public List<Item> requiredItems; 
    public Item upgradedItem; 
}
