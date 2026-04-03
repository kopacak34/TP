using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class FireRateBoost : MonoBehaviour
{
    public EsScript shooter;
    public float baseFireRateMultiplier = 3f;
    public float boostDuration = 10f;

    private bool isBoostActive = false;
    private float preBoostFireRate = 1f;

    public float fireRateMultiplier = 1f;
    public int abilityUpgradeLevel = 0;
    public int maxAbilityLevel = 5;

    private PlayerHealth playerHealth;

    public float boostCooldown = 15f;
    private float lastBoostTime = -Mathf.Infinity;

    public Image imagecooldown;
    public TextMeshProUGUI Xlevel;

    void Start()
    {
        if (shooter == null)
        {
            Debug.LogError("Chybí reference na EsScript!");
            return;
        }

        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth komponenta nenalezena na hráči!");
        }

        if (imagecooldown != null)
        {
            imagecooldown.fillAmount = 0f;
        }

        
    }

    void Update()
    {
        float cooldownRemaining = Mathf.Clamp(boostCooldown - (Time.time - lastBoostTime), 0f, boostCooldown);
        if (imagecooldown != null)
        {
            imagecooldown.fillAmount = cooldownRemaining / boostCooldown;
        }

        if (Gamepad.current != null)
        {
            if (Gamepad.current.leftTrigger.isPressed && Gamepad.current.buttonWest.wasPressedThisFrame)
            {
                TryUpgradeAbility();
            }
            else if (Gamepad.current.buttonWest.wasPressedThisFrame && !isBoostActive)
            {
                if (Time.time - lastBoostTime >= boostCooldown)
                {
                    StartCoroutine(BoostFireRate());
                    lastBoostTime = Time.time;
                }
                else
                {
                    Debug.Log("Boost rychlosti střelby je na cooldownu!");
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
            fireRateMultiplier *= 1.2f; 
            playerHealth.availableUpgrades--;
            Xlevel.text = $"Lvl: {abilityUpgradeLevel}";

            

            Debug.Log($"Ability vylepšena na úroveň {abilityUpgradeLevel}/{maxAbilityLevel}. Násobitel: {fireRateMultiplier:F2}x");
        }
        else
        {
            Debug.Log("Nelze vylepšit – žádné dostupné upgrady.");
        }
    }

    private IEnumerator BoostFireRate()
    {
        preBoostFireRate = shooter.fireRate;
        float boostedFireRate = preBoostFireRate * fireRateMultiplier;

        isBoostActive = true;
        shooter.fireRate = boostedFireRate;
        Debug.Log($"Boost aktivní! Rychlost střelby z {preBoostFireRate:F2} -> {shooter.fireRate:F2}");

        yield return new WaitForSeconds(boostDuration);

        shooter.fireRate = preBoostFireRate;
        isBoostActive = false;
        Debug.Log($"Boost skončil. Rychlost střelby vrácena na {shooter.fireRate:F2}");
    }

    
}
