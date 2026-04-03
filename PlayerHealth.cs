using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public int gold = 0;
    public int xp = 0;
    public int level = 1;
    public int xpToNextLevel = 100;
    public float regenRate = 1f / 3;
    private float regenTimer = 0f;
    public int availableUpgrades = 0;
    public float AbilityHaste;
    public float AttackDamageArmor;
    public float AbillityPowerArmor;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI fpsText;

    public Image healthbar;
    public Image xpbar;

    private float startTime;

    void Start()
    {
        currentHealth = maxHealth;
        startTime = Time.time;
        UpdateUI();
    }

    void Update()
    {
        UpdateTimeUI();
        UpdateFPSUI();
        HandlePassiveRegen();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = currentHealth-(damage-AttackDamageArmor);
        

        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateUI();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        
        UpdateUI();
    }

    public void AddXP(int amount)
    {
        xp += amount;
        CheckLevelUp();
        UpdateUI();
    }

    private void CheckLevelUp()
    {
        while (xp >= xpToNextLevel)
        {
            xp -= xpToNextLevel;
            level++;
            xpToNextLevel = Mathf.FloorToInt(xpToNextLevel * 1.5f);

            maxHealth += 10;
            currentHealth += 10;

            availableUpgrades++;

           
        }
    }

    private void Die()
    {
        

        SceneManager.LoadScene("Menu");
    }

    public void UpdateUI()
    {
        if (healthText != null) healthText.text = $"{currentHealth}/{maxHealth}";
        if (goldText != null) goldText.text = $"Gold: {gold}";
        if (levelText != null) levelText.text = $"Lvl: {level}";
        if (healthbar != null) healthbar.fillAmount = currentHealth / (float)maxHealth;
        if (xpbar != null) xpbar.fillAmount = xp / (float)xpToNextLevel;
    }

    private void UpdateTimeUI()
    {
        if (timeText != null)
        {
            float elapsedTime = Time.time - startTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timeText.text = $"Čas: {minutes:D2}:{seconds:D2}";
        }
    }

    private void UpdateFPSUI()
    {
        if (fpsText != null)
        {
            float fps = 1.0f / Time.unscaledDeltaTime;
            fpsText.text = $"FPS: {Mathf.RoundToInt(fps)}";
        }
    }

    private void HandlePassiveRegen()
    {
        if (currentHealth > 0 && currentHealth < maxHealth)
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= 1f)
            {
                int healAmount = Mathf.FloorToInt(regenRate);
                Heal(healAmount);
                regenTimer = 0f;
            }
        }
        else
        {
            regenTimer = 0f;
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Nelze léčit – hráč je mrtvý.");
            return;
        }

        float oldHealth = currentHealth;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        float healedAmount = currentHealth - oldHealth;

        Debug.Log($"Vyléčil {healedAmount} HP! Zbývající HP: {currentHealth}");
        UpdateUI();
    }
    

}
