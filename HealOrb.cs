using UnityEngine;

public class HealOrb : MonoBehaviour
{
    private float healPercentage = 0.05f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                float healAmount = playerHealth.maxHealth * healPercentage;
                playerHealth.Heal(Mathf.CeilToInt(healAmount));
                Destroy(gameObject);
            }
        }
    }
}

