using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public GameObject zonePrefab;
    private Vector3 target;
    private bool initialized = false;

    public void SetTarget(Vector3 position, GameObject zone)
    {
        target = position;
        zonePrefab = zone;
        initialized = true;

        transform.LookAt(target);
    }

    void Update()
    {
        if (!initialized) return;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        
        if (Vector3.Distance(transform.position, target) < 0.2f)
        {
            Debug.Log("🔥 Dopad – spouštím fire zone");

            if (zonePrefab != null)
                Instantiate(zonePrefab, transform.position, Quaternion.identity);
            else
                Debug.LogWarning("⚠️ zonePrefab není nastaven!");

            Destroy(gameObject);
        }
    }
}
