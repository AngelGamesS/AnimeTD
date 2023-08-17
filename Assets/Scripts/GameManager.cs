using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool inWave = false;
    public SaveSystem saveSystem;
    public GameObject WinUI;
    [SerializeField] private int _wavHp = 20;
    [SerializeField] private int _playerLevel = 1;
    [SerializeField] private float _playerExp = 0;
    [SerializeField] private int _playerCoin = 0;
    public GameObject endPortal;

    [SerializeField] private float _myLevelExpToLevelUp = 1000f;
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

        var data = saveSystem.LoadFromJson();
        if(data != null)
        {
            _playerExp = data.playerExp;
            _playerLevel = data.playerLevel;
            _myLevelExpToLevelUp = data.myLevelExpToLevelUp;
            Invoke("InvokedUpdatePlayerData", 0.1f);
        }
    }

    private void InvokedUpdatePlayerData()
    {
        gameEventChannel.RaiseOnLevelUp(_playerLevel);
        gameEventChannel.RaiseOnUpdateExpAmount((_playerExp / _myLevelExpToLevelUp) * 100f);
    }

    private void HandleWinLose(bool status)
    {
        if (status && WinUI != null)
        {
            WinUI.SetActive(true);
            saveSystem.SaveIntoJson(_playerExp,_playerLevel,_myLevelExpToLevelUp);
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

        if(_playerExp / _myLevelExpToLevelUp > 1)
        {
            _playerLevel++;
            _playerExp = 0;
            _myLevelExpToLevelUp = _myLevelExpToLevelUp * 1.5f + _myLevelExpToLevelUp;
            gameEventChannel.RaiseOnLevelUp(_playerLevel);
            gameEventChannel.RaiseOnUpdateExpAmount((_playerExp / _myLevelExpToLevelUp) * 100f);

        }
        else
            gameEventChannel.RaiseOnUpdateExpAmount((_playerExp / _myLevelExpToLevelUp) * 100f);
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
    public void GainCoin(int price)
    {
        _playerCoin += price;
        gameEventChannel.RaiseOnCoinAmountUpdate(_playerCoin);
    }

    public void ChangeInWave(bool status)
    {
        inWave = status;
        //Not Changing Wave Index
        gameEventChannel.RaiseOnGameWaveStatusChange(inWave,-1);
    }

    public int GetHP() => _wavHp;

    public float GetExp() => ((_playerExp / _myLevelExpToLevelUp) * 100f);

    public int GetLevel() => _playerLevel;
}
