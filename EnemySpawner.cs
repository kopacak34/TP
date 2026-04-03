using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class EnemySpawner : MonoBehaviour
{
    public GameObject RangePrefab; 
   
    public Transform player; 
    public float spawnRadius = 10f; 
    public int enemiesPerWave = 5; 
    public float timeBetweenWaves = 5f;

    private int waveNumber = 1;

    void Start()
    {
        StartCoroutine(SpawnWaves());
        MinionBullet.StartDamageScaling(this);
        

    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            SpawnWave();
            waveNumber++;
        }
    }

    void SpawnWave()
    {
        int spawned = 0;
        int attempts = 0;
        int maxAttempts = enemiesPerWave * 10;

        while (spawned < enemiesPerWave && attempts < maxAttempts)
        {
            attempts++;
            Vector3? spawnPos = GetRandomSpawnPosition();

            if (spawnPos.HasValue)
            {
                Instantiate(RangePrefab, spawnPos.Value, Quaternion.identity);
                spawned++;
            }
        }
    }

    Vector3? GetRandomSpawnPosition()
    {
        for (int i = 0; i < 10; i++) 
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 randomPos = new Vector3(randomCircle.x, 0, randomCircle.y) + player.position;

            NavMeshHit hit;
            float maxDistance = 2f;

            
            if (NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return null;
    }

}