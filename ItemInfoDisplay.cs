using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI bonusStatsText;
    public TextMeshProUGUI requiredItemsText;

    public void DisplayItemInfo(Item item)
    {
        itemNameText.text = item.itemName;
        priceText.text = $"Cena: {item.price} Gold";

        string stats = "";
        if (item.bonusHealth > 0) stats += $"+{item.bonusHealth} HP\n";
        if (item.bonusRegen > 0) stats += $"+{item.bonusRegen} Regen\n";
        if (item.bonusAttackSpeed > 0) stats += $"+{item.bonusAttackSpeed} Attack Speed\n";
        if (item.bonusDamage > 0) stats += $"+{item.bonusDamage} Damage\n";
        
        
        if (item.bonusAttackDamageArmor > 0) stats += $"+{item.bonusAttackDamageArmor} AD Armor\n";
        bonusStatsText.text = stats;

        string requirements = "";
        if (item.requiredItems != null && item.requiredItems.Count > 0)
        {
            foreach (var reqItem in item.requiredItems)
            {
                requirements += $"{reqItem.itemName}\n";
            }
        }
        else
        {
            requirements = "Žádné požadavky.";
        }
        requiredItemsText.text = requirements;
    }
}

