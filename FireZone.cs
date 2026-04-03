using UnityEngine;
using System.Collections;

public class FireZone : MonoBehaviour
{
    public float damagePerTick = 20f;
    public float tickInterval = 0.5f;
    public float duration = 10f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DamageOverTime(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(DamageOverTime(other)); 
        }
    }

    IEnumerator DamageOverTime(Collider player)
    {
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        while (player != null && ph != null && player.bounds.Intersects(GetComponent<Collider>().bounds))
        {
            ph.TakeDamage(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
    }
}
