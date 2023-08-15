using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool inWave = false;
    public GameObject WinUI;
    [SerializeField] private int _wavHp = 20;
    [SerializeField] private int _playerLevel = 1;
    [SerializeField] private float _playerExp = 0;
    [SerializeField] private int _playerCoin = 0;
    public GameObject endPortal;

    [SerializeField] private float MyLeveExpToLevelUp = 1000f;
    [Header("Event Channels")]
    [SerializeField] private GameEventChannelSO gameEventChannel;


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        gameEventChannel.OnWinLose.AddListener(HandleWinLose);
    }

    private void HandleWinLose(bool status)
    {
        if (status)
        {
            WinUI.SetActive(true);
        }
    }

    public int GetCoin() => _playerCoin;

    public void TakeDamage()
    {
        _wavHp--;
        gameEventChannel.RaiseOnCurrentWaveHpChange(_wavHp);
    }

    public void HandleEnemyDeath(float earnExp, int earnCoin) 
    {
        _playerExp += earnExp;
        _playerCoin += earnCoin;

        gameEventChannel.RaiseOnCoinAmountUpdate(_playerCoin);

        if(_playerExp / MyLeveExpToLevelUp > 1)
        {
            _playerLevel++;
            _playerExp = 0;
            MyLeveExpToLevelUp = MyLeveExpToLevelUp * 1.5f + MyLeveExpToLevelUp;
            gameEventChannel.RaiseOnLevelUp(_playerLevel);
            gameEventChannel.RaiseOnUpdateExpAmount((_playerExp / MyLeveExpToLevelUp) * 100f);

        }
        else
            gameEventChannel.RaiseOnUpdateExpAmount((_playerExp / MyLeveExpToLevelUp) * 100f);
    }

    public bool TryBuyTower(TowerDataSO selectedTower,Vector3 pos)
    {
        if(_playerCoin - selectedTower.price >= 0)
        {
            var tower = Instantiate(selectedTower.prefab, pos, Quaternion.identity);
            gameEventChannel.RaiseOnTowerPlaced(selectedTower);

            PayCoin(selectedTower.price);
            return true;
        }
        return false;
    }

    public void PayCoin(int price)
    {
        _playerCoin -= price;
        gameEventChannel.RaiseOnCoinAmountUpdate(_playerCoin);
    }

    public void ChangeInWave(bool status)
    {
        inWave = status;
        //Not Changing Wave Index
        gameEventChannel.RaiseOnGameWaveStatusChange(inWave,-1);
    }
}
