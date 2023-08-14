using System.Collections;
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
    [Header("Event Channels")]
    [SerializeField] private GameEventChannelSO gameEventChannel;


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
                Invoke("WaveFinished", 5f);
            }

        }
    }

    private void WaveFinished()
    {
        gameEventChannel.RaiseOnGameWaveStatusChange(false);
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

    public void UpdateInWave(bool inWave)
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
