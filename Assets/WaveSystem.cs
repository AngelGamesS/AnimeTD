using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] private GameObject spawnPoint;
    public WaveSO[] waves;
    private Wave currentWave;

    private int currentWaveIndex;
    public int currentWaveIntervalIndex = 0;
    private bool readyToCountDown = false;
    private List<Enemy> _enemyWave = new List<Enemy>();
    [Header("Event Channels")]
    [SerializeField] private GameEventChannelSO gameEventChannel;
    private bool _spawningWave = false;


    private void Start()
    {
        currentWaveIndex = 0;
        currentWave = waves[currentWaveIndex].wave;
        InitializeWaveIntervals();
        gameEventChannel.OnGameWaveStatusChange.AddListener(UpdateInWave);
    }

    private void Update()
    {
        if (readyToCountDown)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0)
        {
            readyToCountDown = false;
            if (currentWaveIntervalIndex < currentWave.waveIntervals.Length)
            {
                countdown = currentWave.waveIntervals[currentWaveIntervalIndex].timeToNextWaveInterval;
                StartCoroutine(SpawnWave());
            }
            else
            {
                countdown = Mathf.Infinity;
            }
            
        }

        if (currentWaveIntervalIndex < currentWave.waveIntervals.Length && currentWave.waveIntervals[currentWaveIntervalIndex].enemiesLeft == 0)
        {
            currentWaveIntervalIndex++;
            if (currentWaveIntervalIndex < currentWave.waveIntervals.Length)
            {
                readyToCountDown = true;
            }
            else
            {
                countdown = Mathf.Infinity;
                currentWaveIndex++;
            }

        }
    }

    private void WaveFinished()
    {
        if(currentWaveIndex > waves.Length - 1)
            gameEventChannel.RaiseOnWinLose(true);
        else
            gameEventChannel.RaiseOnGameWaveStatusChange(false,currentWaveIndex + 1);
    }

    private void InitializeWaveIntervals()
    {
        foreach (WaveInterval waveInterval in currentWave.waveIntervals)
        {
            waveInterval.enemiesLeft = waveInterval.enemies.Length;
        }
        currentWave.waveIntervals[currentWave.waveIntervals.Length - 1].timeToNextWaveInterval = Mathf.Infinity;
    }

    private IEnumerator SpawnWave()
    {
        _spawningWave = true;
        if (currentWaveIntervalIndex < currentWave.waveIntervals.Length)
        {
            WaveInterval currentWaveInterval = currentWave.waveIntervals[currentWaveIntervalIndex];

            foreach (GameObject enemyPrefab in currentWaveInterval.enemies)
            {
                GameObject enemyObj = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
                enemyObj.transform.forward = spawnPoint.transform.forward;
                enemyObj.transform.SetParent(spawnPoint.transform);

                Enemy enemy = enemyObj.GetComponent<Enemy>();
                if(enemy != null)
                {
                    _enemyWave.Add(enemy);
                    enemy.OnDeath += HandleEnemyDeath;
                }
                
                
                currentWaveInterval.enemiesLeft--;

                yield return new WaitForSeconds(currentWaveInterval.timeToNextEnemy);
            }
        }
        _spawningWave = false;
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        _enemyWave.Remove(enemy);
        _enemyWave.RemoveAll(en => en == null);
        if (_enemyWave.Count == 0 && countdown == Mathf.Infinity && !readyToCountDown && !_spawningWave)
        {
            WaveFinished();
        }
    }

    public void UpdateInWave(bool inWave, int waveIndex)
    {
        readyToCountDown = inWave;
        if(inWave)
        {
            currentWave = waves[currentWaveIndex].wave;
            InitializeWaveIntervals();
            countdown = 0;
            currentWaveIntervalIndex = 0;
        }
    }
}

[System.Serializable]
public class WaveInterval
{
    public GameObject[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWaveInterval;

    [HideInInspector] public int enemiesLeft;
}

[System.Serializable]
public class Wave
{
    public WaveInterval[] waveIntervals;
}
