using System.Collections;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] private GameObject spawnPoint;
    public Wave wave;

    private int currentWaveIntervalIndex = 0;
    private bool readyToCountDown = true;

    private void Start()
    {
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
            countdown = wave.waveIntervals[currentWaveIntervalIndex].timeToNextWaveInterval;
            StartCoroutine(SpawnWave());
        }

        if (wave.waveIntervals[currentWaveIntervalIndex].enemiesLeft == 0)
        {
            readyToCountDown = true;
            currentWaveIntervalIndex++;
            if (currentWaveIntervalIndex < wave.waveIntervals.Length)
            {
                countdown = wave.waveIntervals[currentWaveIntervalIndex].timeToNextWaveInterval;
            }
            
        }
    }

    private void InitializeWaveIntervals()
    {
        foreach (WaveInterval waveInterval in wave.waveIntervals)
        {
            waveInterval.enemiesLeft = waveInterval.enemies.Length;
        }
    }

    private IEnumerator SpawnWave()
    {
        if (currentWaveIntervalIndex < wave.waveIntervals.Length)
        {
            WaveInterval currentWaveInterval = wave.waveIntervals[currentWaveIntervalIndex];

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
