using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SpawnModes
{
    Fixed,
    Random
}

public class Spawner : MonoBehaviour
{
    public static Action OnWaveCompleted;
    

    [SerializeField] private SpawnModes spawnMode = SpawnModes.Fixed;
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private float delayBtwWaves = 1f;
    [SerializeField] private int rangeWidth = 5;

    [SerializeField] private float delayBtwSpawns;
    

    [SerializeField] private float minRandomDelay;
    [SerializeField] private float maxRandomDelay;


    [SerializeField] private ObjectPooler enemyWave1Pooler;
    [SerializeField] private ObjectPooler enemyWave2Pooler;
    [SerializeField] private ObjectPooler enemyWave3Pooler;

    
    private float spawnTimer;
    private int enemiesSpawned;
    private int enemiesRamaining;
    
    private Waypoint waypoint;

    private void Start()
    {
        waypoint = GetComponent<Waypoint>();

        enemiesRamaining = enemyCount;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0)
        {
            spawnTimer = GetSpawnDelay();
            if (enemiesSpawned < enemyCount)
            {
                enemiesSpawned++;
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject newInstance = GetPooler().GetInstanceFromPool();
        Enemy enemy = newInstance.GetComponent<Enemy>();
        enemy.Waypoint = waypoint;
        enemy.ResetEnemy();

        enemy.transform.localPosition = transform.position;
        newInstance.SetActive(true);
    }

    private float GetSpawnDelay()
    {
        float delay = 0f;
        if (spawnMode == SpawnModes.Fixed)
        {
            delay = delayBtwSpawns;
        }
        else
        {
            delay = GetRandomDelay();
        }

        return delay;
    }
    
    private float GetRandomDelay()
    {
        float randomTimer = Random.Range(minRandomDelay, maxRandomDelay);
        return randomTimer;
    }

    private ObjectPooler GetPooler()
    {
        int currentWave = LevelManager.Instance.CurrentWave;

        int waveRange = Mathf.FloorToInt((currentWave - 1) / rangeWidth) % 3;

        switch (waveRange)
        {
            case 0:
                return enemyWave1Pooler;
            case 1:
                return enemyWave2Pooler;
            case 2:
                return enemyWave3Pooler;
            default:
                return null;
        }
    }


    private IEnumerator NextWave()
    {
        yield return new WaitForSeconds(delayBtwWaves);
        enemiesRamaining = enemyCount;
        spawnTimer = 0f;
        enemiesSpawned = 0;
    }
    
    private void RecordEnemy(Enemy enemy)
    {
        enemiesRamaining--;
        if (enemiesRamaining <= 0)
        {
            OnWaveCompleted?.Invoke();
            StartCoroutine(NextWave());
        }
    }
    
    private void OnEnable()
    {
        Enemy.OnEndReached += RecordEnemy;
        EnemyHealth.OnEnemyKilled += RecordEnemy;
    }

    private void OnDisable()
    {
        Enemy.OnEndReached -= RecordEnemy;
        EnemyHealth.OnEnemyKilled -= RecordEnemy;
    }
}
