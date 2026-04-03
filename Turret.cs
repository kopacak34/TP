using System.Linq;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float range = 10f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    void Update()
    {
        Transform closestEnemy = FindClosestEnemy();
        if (closestEnemy == null) return;

        float distanceToEnemy = Vector3.Distance(transform.position, closestEnemy.position);
        if (distanceToEnemy <= range)
        {
            if (fireCountdown <= 0f)
            {
                Shoot(closestEnemy);
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Minion");
        if (enemies.Length == 0) return null;

        return enemies
            .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
            .FirstOrDefault()?.transform;
    }

    void Shoot(Transform target)
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(target);
        }
    }
}
