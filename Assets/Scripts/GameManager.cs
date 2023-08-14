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
        gameEventChannel.RaiseOnUpdateExpAmount(_playerExp);
        gameEventChannel.RaiseOnCoinAmountUpdate(_playerCoin);
    }

    public bool TryBuyTower(TowerDataSO selectedTower,Vector3 pos)
    {
        if(_playerCoin - selectedTower.price >= 0)
        {
            var tower = Instantiate(selectedTower.prefab, pos, Quaternion.identity);
            gameEventChannel.RaiseOnTowerPlaced(selectedTower);
           
            _playerCoin -= selectedTower.price;
            gameEventChannel.RaiseOnCoinAmountUpdate(_playerCoin);
            return true;
        }
        return false;
    }

    public void ChangeInWave(bool status)
    {
        inWave = status;
        gameEventChannel.RaiseOnGameWaveStatusChange(inWave);
    }
}
