using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image icon;

    public void SetItem(Sprite newIcon)
    {
        icon.sprite = newIcon;
        icon.enabled = newIcon != null;
    }
}

