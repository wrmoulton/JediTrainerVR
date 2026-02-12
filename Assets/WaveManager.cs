using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string name = "Wave";
        public int count = 5;
        public float spawnInterval = 0.6f;
        public float hpMultiplier = 1f;
        public float speedMultiplier = 1f;
    }

    public Transform playerTarget;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public List<Wave> waves = new List<Wave>();
    public float timeBetweenWaves = 2.0f;

    public bool spawnBossAtEnd = false;
    public GameObject bossPrefab;

    int aliveCount = 0;

    void Start()
    {
        Debug.Log($"WaveManager Start. waves={waves.Count}, enemyPrefab={(enemyPrefab?enemyPrefab.name:"NULL")}, spawns={(spawnPoints==null?0:spawnPoints.Length)}");

        if (waves.Count > 0)
            StartCoroutine(RunAllWaves());
    }

    IEnumerator RunAllWaves()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            yield return StartCoroutine(RunSingleWave(waves[i], i + 1));
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        if (spawnBossAtEnd && bossPrefab)
        {
            SpawnEnemy(bossPrefab, 1f, 1f);
            while (aliveCount > 0) yield return null;
        }

        Debug.Log("All waves complete!");
    }

    IEnumerator RunSingleWave(Wave wave, int waveNumber)
    {
        Debug.Log($"Starting {wave.name} ({waveNumber}/{waves.Count}) count={wave.count}");

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(enemyPrefab, wave.hpMultiplier, wave.speedMultiplier);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        while (aliveCount > 0) yield return null;
        Debug.Log($"{wave.name} complete!");
    }

    void SpawnEnemy(GameObject prefab, float hpMult, float speedMult)
    {
        if (!prefab)
        {
            Debug.LogError("SpawnEnemy: prefab is NULL");
            return;
        }
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("SpawnEnemy: spawnPoints is empty");
            return;
        }

        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject go = Instantiate(prefab, sp.position, sp.rotation);
        Debug.Log($"Spawned: {go.name} at {go.transform.position}");

        // Assign target
        var chase = go.GetComponent<FlyingChase>();
        if (chase)
        {
            chase.target = playerTarget;
            chase.moveSpeed *= speedMult;
        }
        else
        {
            Debug.LogWarning("Spawned enemy has no FlyingChase on root.");
        }

        // Hook health
        var hp = go.GetComponent<EnemyHealth>();
        if (hp)
        {
            hp.maxHp *= hpMult;
            hp.hp = hp.maxHp;

            hp.OnDied -= OnEnemyDied;
            hp.OnDied += OnEnemyDied;
        }
        else
        {
            Debug.LogWarning("Spawned enemy has no EnemyHealth on root.");
        }

        aliveCount++;
        Debug.Log($"aliveCount = {aliveCount}");
    }

    void OnEnemyDied(EnemyHealth h)
    {
        if (h != null) h.OnDied -= OnEnemyDied;
        aliveCount = Mathf.Max(0, aliveCount - 1);
        Debug.Log($"Enemy died. aliveCount = {aliveCount}");
    }
}
