using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class DamageBoost : MonoBehaviour
{
    public EsScript shooter;  
    public float baseMultiplier = 1.5f;
    public float boostDuration = 10f;
    public float boostCooldown = 15f;
    private float lastBoostTime = -Mathf.Infinity;

    private float originalDamage;
    private bool isBoostActive = false;
    public float damageMultiplier = 1f;
    public int abilityUpgradeLevel = 0;
    public int maxAbilityLevel = 5;

    private PlayerHealth playerHealth;

    public Image imagecooldown;
    public TextMeshProUGUI Blevel;


    void Start()
    {
        if (shooter == null)
        {
            Debug.LogError("Chybí reference na EsScript!");
            return;
        }

        originalDamage = shooter.baseDamage;  
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth komponenta nenalezena na hráči!");
        }

        imagecooldown.fillAmount = 0f;
        
       
    }

    void Update()
    {
        float cooldownRemaining = Mathf.Clamp(boostCooldown - (Time.time - lastBoostTime), 0f, boostCooldown);
        imagecooldown.fillAmount = cooldownRemaining / boostCooldown;

        if (Gamepad.current != null)
        {
            if (Gamepad.current.leftTrigger.isPressed && Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                TryUpgradeAbility();
            }
            else if (Gamepad.current.buttonEast.wasPressedThisFrame && !isBoostActive)
            {
                if (Time.time - lastBoostTime >= boostCooldown)
                {
                    StartCoroutine(BoostDamage());
                    lastBoostTime = Time.time;
                }
                else
                {
                    Debug.Log("Damage boost je na cooldownu!");
                }
            }
        }
    }

    private void TryUpgradeAbility()
    {
        if (abilityUpgradeLevel >= maxAbilityLevel)
        {
            Debug.Log("Ability je na MAX úrovni!");
            return;
        }

        if (playerHealth != null && playerHealth.availableUpgrades > 0)
        {
            abilityUpgradeLevel++;
            damageMultiplier *= 1.2f;  
            playerHealth.availableUpgrades--;
            Blevel.text = $"Lvl: {abilityUpgradeLevel}";

            
            

            Debug.Log($"Ability vylepšena na úroveň {abilityUpgradeLevel}/{maxAbilityLevel}. Násobitel damage: {damageMultiplier:F2}x");
        }
        else
        {
            Debug.Log("Nelze vylepšit abilitu – žádné dostupné upgrady.");
        }
    }

    private IEnumerator BoostDamage()
    {
        float preBoostDamage = shooter.baseDamage;
        float boostedDamage = preBoostDamage * baseMultiplier * damageMultiplier;

        isBoostActive = true;
        shooter.baseDamage = boostedDamage; 
        Debug.Log($"Boost damage aktivní! Damage z {preBoostDamage:F2} -> {shooter.baseDamage:F2}");

        yield return new WaitForSeconds(boostDuration);

        shooter.baseDamage = preBoostDamage;
        isBoostActive = false;
        Debug.Log($"Boost damage skončil. Damage vrácen na {shooter.baseDamage:F2}");
    }

    
}
