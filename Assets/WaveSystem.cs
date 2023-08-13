using System.Collections;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] private GameObject spawnPoint;
    public WaveSO[] waves;
    private Wave currentWave;

    public int currentWaveIntervalIndex = 0;
    private bool readyToCountDown = true;

    private void Start()
    {
        currentWave = waves[0].wave;
        InitializeWaveIntervals();
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
            }

        }
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
        if (currentWaveIntervalIndex < currentWave.waveIntervals.Length)
        {
            WaveInterval currentWaveInterval = currentWave.waveIntervals[currentWaveIntervalIndex];

            foreach (GameObject enemyPrefab in currentWaveInterval.enemies)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
                enemy.transform.SetParent(spawnPoint.transform);
                currentWaveInterval.enemiesLeft--;

                yield return new WaitForSeconds(currentWaveInterval.timeToNextEnemy);
            }
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
