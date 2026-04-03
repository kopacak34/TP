using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    public float damage;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetDamage(float dmg)
    {
        damage = dmg; 
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        MinionAI enemy = target.GetComponent<MinionAI>();
        CanonAI canon = target.GetComponent<CanonAI>();
        FinalBossAI boss = target.GetComponent<FinalBossAI>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        if (canon != null)
        {
            canon.TakeDamage(damage);
        }
        if (boss != null)
        {
            boss.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
