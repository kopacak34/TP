using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class EsA : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spreadAngle = 5f;
    public float spreadDistance = 0.5f;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    public float damageMultiplier = 1f;
    public int abilityUpgradeLevel = 0;
    public int maxAbilityLevel = 5;

    public Image imagecooldown;

    private PlayerHealth playerHealth;

    public float fireCooldown = 10f;
    private float lastFireTime;

    public TextMeshProUGUI Alevel;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth komponenta nenalezena na hráči!");
        }

        lastFireTime = Time.time - fireCooldown;

        if (imagecooldown != null)
        {
            imagecooldown.fillAmount = 0f;
        }

        
    }

    void Update()
    {
        float cooldownRemaining = Mathf.Clamp(fireCooldown - (Time.time - lastFireTime), 0f, fireCooldown);
        if (imagecooldown != null)
        {
            imagecooldown.fillAmount = cooldownRemaining / fireCooldown;
        }

        if (Gamepad.current != null)
        {
            if (Gamepad.current.leftTrigger.isPressed && Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                TryUpgradeAbility();
            }
            else if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                
                ShootBurst();
            }
        }
    }

    void TryUpgradeAbility()
    {
        if (abilityUpgradeLevel >= maxAbilityLevel)
        {
            Debug.Log("Ability je na MAX úrovni!");
            return;
        }

        if (playerHealth != null && playerHealth.availableUpgrades > 0)
        {
            damageMultiplier *= 1.2f;
            abilityUpgradeLevel++;
            playerHealth.availableUpgrades--;
            Alevel.text = $"Lvl: {abilityUpgradeLevel}";

            

            Debug.Log($"Ability vylepšena na úroveň {abilityUpgradeLevel}/{maxAbilityLevel}, damage násobič: {damageMultiplier:F2}x");
        }
        else
        {
            Debug.Log("Nelze vylepšit abilitu – žádné dostupné upgrady.");
        }
    }

    void ShootBurst()
    {
        if (Time.time - lastFireTime < fireCooldown)
        {
            Debug.Log("Střelba je na cooldownu!");
            return;
        }

        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("bulletPrefab nebo firePoint není nastaven!");
            return;
        }

        for (int i = -2; i <= 2; i++)
        {
            animator.SetTrigger("esA");
            float xOffset = spreadDistance * i;
            float angleOffset = spreadAngle * i;

            Vector3 spawnPosition = firePoint.position + firePoint.right * xOffset;
            Quaternion spreadRotation = Quaternion.Euler(0, angleOffset, 0);

            GameObject bulletInstance = Instantiate(bulletPrefab, spawnPosition, firePoint.rotation * spreadRotation);

            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = bulletInstance.transform.forward * bulletSpeed;
            }
            else
            {
                Debug.LogError("Bullet nemá Rigidbody!");
            }

            EsABullet bullet = bulletInstance.GetComponent<EsABullet>();
            if (bullet != null)
            {
                bullet.damageMultiplier = damageMultiplier;
            }
        }

        lastFireTime = Time.time;
    }

    
}
