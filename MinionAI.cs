using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MinionAI : MonoBehaviour
{
    public GameObject orbPrefab;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform head;

    private string playerTag = "Player";
    private float followRange = 1000f;
    private float attackRange = 15f;
    private float fireRate = 1.5f;
    private float attackDelay = 1f;
    private float dropRate = 0.03f;

    
    public static float baseHealth = 100f;
    public static float healthIncreaseInterval = 30f;
    public static float healthIncreaseAmount = 20f;
    private static bool isHealthScalingStarted = false;

    public float currentHealth;

    private Transform player;
    private NavMeshAgent agent;
    private Coroutine shootingCoroutine = null;
    private bool isWaitingToShoot = false;

    void Start()
    {
        
        currentHealth = baseHealth;

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            
        }

        FindPlayer();

        
        if (!isHealthScalingStarted)
        {
            isHealthScalingStarted = true;
            StartCoroutine(HealthScalingLoop());
        }
    }

    IEnumerator HealthScalingLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(healthIncreaseInterval);
            baseHealth += healthIncreaseAmount;
            
        }
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        RotateTowardsPlayer();

        if (distanceToPlayer < attackRange)
        {
            agent.isStopped = true;

            if (shootingCoroutine == null && !isWaitingToShoot)
            {
                isWaitingToShoot = true;
                StartCoroutine(StartShootingWithDelay());
            }
        }
        else
        {
            agent.isStopped = distanceToPlayer < followRange ? false : true;
            agent.SetDestination(player.position);

            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null;
            }
            isWaitingToShoot = false;
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Hráč s tagem '" + playerTag + "' nebyl nalezen!");
        }
    }

    IEnumerator StartShootingWithDelay()
    {
        yield return new WaitForSeconds(attackDelay);

        if (player != null && Vector3.Distance(transform.position, player.position) < attackRange)
        {
            shootingCoroutine = StartCoroutine(ShootAtPlayer());
        }
        else
        {
            isWaitingToShoot = false;
        }
    }

    IEnumerator ShootAtPlayer()
    {
        while (player != null && Vector3.Distance(transform.position, player.position) < attackRange)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
        shootingCoroutine = null;
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            MinionBullet bulletScript = bullet.GetComponent<MinionBullet>();
            if (bulletScript != null)
            {
                bulletScript.SetTarget(player);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        
        DropOrb();
        Destroy(gameObject);

        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.AddXP(10);
                playerHealth.AddGold(14);
            }
        }
    }

    private void DropOrb()
    {
        if (orbPrefab != null && Random.value < dropRate)
        {
            Instantiate(orbPrefab, transform.position, Quaternion.identity);
        }
    }
}
