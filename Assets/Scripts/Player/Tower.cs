using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Tower Base")]
    [SerializeField] protected TowerData _towerData;
    [SerializeField] protected LayerMask _layerMask;
    protected GameObject _target;
    protected Animator _animator;
    [Header("Event Channels")]
    [SerializeField] private GameEventChannelSO gameEventChannel;

    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public virtual void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _towerData.towerRange, _layerMask);
        if (colliders.Length > 0)
        {
            _target = FindClosestOneEnemy(colliders).gameObject;
        }
    }

    private Collider FindClosestOneEnemy(Collider[] colliders)
    {
        int selectedIndex = 0;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < colliders.Length; i++)
        {
            float tmpDistance = Vector3.Distance(transform.position, colliders[i].transform.position);
            if (tmpDistance < minDistance)
            {
                selectedIndex = i;
                minDistance = tmpDistance;
            }
        }
        return colliders[selectedIndex];
    }

    public TowerData GetTowerData() => _towerData;

    public void SetUpData(TowerDataSO data)
    {
        this._towerData.towerSprite = data.image;
    }

    public bool TryUpgradeAttack()
    {
        if(GameManager.Instance.GetCoin() - _towerData.dmgUpgradePrice >= 0)
        {
            GameManager.Instance.PayCoin(_towerData.dmgUpgradePrice);
            _towerData.upgradesDone++;
            _towerData.dmgUpgradePrice *= 2;
            _towerData.towerDmg += _towerData.towerDmgIncreace;
            return true;
        }
        return false;
    }

    public bool TryUpgradeRange()
    {
        if (GameManager.Instance.GetCoin() - _towerData.rangeUpgradePrice >= 0)
        {
            GameManager.Instance.PayCoin(_towerData.rangeUpgradePrice);
            _towerData.upgradesDone++;
            _towerData.rangeUpgradePrice *= 2;
            _towerData.towerRange += _towerData.towerRangeIncreace;

            return true;
        }

        return false;
    }

    public bool TryUpgradeAttackSpeed()
    {
        if (GameManager.Instance.GetCoin() - _towerData.attackIntervalUpgradePrice >= 0)
        {
            GameManager.Instance.PayCoin(_towerData.attackIntervalUpgradePrice);
            _towerData.upgradesDone++;
            _towerData.attackIntervalUpgradePrice *= 2;
            _towerData.towerAttackInterveal -= _towerData.towerIntervealDecreace;

            return true;
        }
        return false;
    }

    private void OnMouseDown()
    {
        gameEventChannel.RaiseOnTowerSelected(this);
    }
}

[System.Serializable]
public class TowerData
{
    public string towerName;
    public Sprite towerSprite;
    public float towerDmg;
    public float towerRange;
    public float towerAttackInterveal;
    public float towerDmgIncreace;
    public float towerRangeIncreace;
    public float towerIntervealDecreace;
    public int dmgUpgradePrice;
    public int rangeUpgradePrice;
    public int attackIntervalUpgradePrice;
    public int upgradesDone = 0;

}
