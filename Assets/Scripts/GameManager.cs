using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool inWave = false;
    [SerializeField] private int _wavHp = 20;
    [SerializeField] private int _playerLevel = 1;
    [SerializeField] private float _playerExp = 0;
    [SerializeField] private int _playerCoin = 0;
    public GameObject endPortal;
    [Header("Event Channels")]
    [SerializeField] private GameEventChannelSO gameEventChannel;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void TakeDamage()
    {
        _wavHp--;
        gameEventChannel.RaiseOnCurrentWaveHpChange(_wavHp);
    }

    public void HandleEnemyDeath(float earnExp, int earnCoin) 
    {
        _playerExp += earnExp;
        _playerCoin += earnCoin;
        gameEventChannel.RaiseOnUpdateExpAmount(_playerExp);
        gameEventChannel.RaiseOnCoinAmountUpdate(_playerCoin);
    }
}
