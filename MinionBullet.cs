using UnityEngine;
using System.Collections;

public class MinionBullet : MonoBehaviour
{
    public static int baseDamage = 10; 
    public static float damageIncreaseInterval = 30f;
    public static int damageIncreaseAmount = 5;

    public int damage;
    public float speed = 10f;
    private Vector3 direction;
    private Rigidbody rb;
    private bool hasTarget = false;

    public void SetTarget(Transform target)
    {
        if (target == null) return;

        Vector3 targetPosition = target.position;
        targetPosition.y += 2f;

        direction = (targetPosition - transform.position).normalized;
        hasTarget = true;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            
            return;
        }

        
        damage = baseDamage;

        rb.useGravity = false;
        Destroy(gameObject, 5f);
    }

    void FixedUpdate()
    {
        if (!hasTarget) return;
        rb.velocity = direction * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
               
            }

            Destroy(gameObject);
        }
    }

    
    public static void StartDamageScaling(MonoBehaviour context)
    {
        context.StartCoroutine(DamageIncreaseLoop());
    }

    private static IEnumerator DamageIncreaseLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(damageIncreaseInterval);
            baseDamage += damageIncreaseAmount;
            
        }
    }
}
