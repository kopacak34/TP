using UnityEngine;

public class EsABullet : MonoBehaviour
{
    public int baseDamage = 20;
    public float damageMultiplier = 1f;
    public float speed = 10f;
    public float lifetime = 5f;

    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody chybí na střele!");
            return;
        }

        rb.useGravity = false;
        moveDirection = transform.forward;

        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Minion"))
        {
            MinionAI minionAI = other.GetComponent<MinionAI>();
            if (minionAI != null)
            {
                int finalDamage = Mathf.RoundToInt(baseDamage * damageMultiplier);
                minionAI.TakeDamage(finalDamage);
                Debug.Log($"Nepřítel dostal {finalDamage} damage! (x{damageMultiplier:F2})");
            }
            Destroy(gameObject);
        }
    }
}
