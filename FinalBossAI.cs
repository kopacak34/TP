using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinalBossAI : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject fireZonePrefab;
    public GameObject aoeEffectPrefab;

    public Transform firePoint;

    public float health = 1000f;
    public float attackRange = 40f;

    private Transform player;
    private NavMeshAgent agent;

   

    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
            Debug.LogError("Hráč nebyl nalezen!");

        StartCoroutine(AttackRotationLoop());
    }

    void Update()
    {
        if (player == null || isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);
        agent.SetDestination(player.position);
        agent.isStopped = (distance < attackRange);

        if (distance < attackRange)
            RotateTowardsPlayer();
    }

    void RotateTowardsPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }
    }

    IEnumerator AttackRotationLoop()
    {
        while (!isDead)
        {
            yield return new WaitUntil(() => player != null && Vector3.Distance(transform.position, player.position) <= attackRange);

            yield return StartCoroutine(AOEAttack());
            yield return new WaitForSeconds(5f);

            yield return StartCoroutine(FireballAttack());
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator AOEAttack()
    {
        if (isDead) yield break;

        Debug.Log("Boss provádí AOE útok!");

        if (aoeEffectPrefab != null)
        {
            GameObject aoeEffect = Instantiate(aoeEffectPrefab, transform.position, Quaternion.identity);
            Destroy(aoeEffect, 3f);
        }

        yield return new WaitForSeconds(0.5f);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth ph = hit.GetComponent<PlayerHealth>();
                if (ph != null)
                    ph.TakeDamage(30f);
            }
        }
    }

    IEnumerator FireballAttack()
    {
        if (isDead) yield break;

        yield return new WaitForSeconds(0.001f);

        if (fireballPrefab != null && player != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            Fireball fb = fireball.GetComponent<Fireball>();
            if (fb != null)
            {
                fb.SetTarget(player.position, fireZonePrefab);
            }
        }
    }

    public void TakeDamage(float dmg)
    {
        if (isDead) return;

        health -= dmg;
        Debug.Log($"Boss dostal {dmg} dmg, zbývá {health}");

        if (health <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;
        SceneManager.LoadScene("Final");
        isDead = true;
        Debug.Log("Boss byl poražen!");
        Destroy(gameObject);
    }
}
