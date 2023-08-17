using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Tower Base")]
    [SerializeField] protected TowerData _towerData;
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] protected Tower evolution;
    [SerializeField] protected Material evolutionMat;
    protected GameObject _target;
    protected Animator _animator;
    [Header("Event Channels")]
    [SerializeField] private GameEventChannelSO gameEventChannel;

    public System.Action OnRangeUpdate;
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

    public bool TryUpgradeAttack(bool shouldPay = true)
    {
        if(GameManager.Instance.GetCoin() - _towerData.dmgUpgradePrice >= 0)
        {
            if (shouldPay)
            {
                GameManager.Instance.PayCoin(_towerData.dmgUpgradePrice);
                _towerData.upgradesDone++;
                _towerData.attackUpgradesDone++;
                _towerData.dmgUpgradePrice *= 2;
                _towerData.towerDmg += _towerData.towerDmgIncreace;
            }
            else
            {
                _towerData.dmgUpgradePrice *= 2;
                _towerData.towerDmg += _towerData.towerDmgIncreace;
            }
            TryEvloving();
            return true;
        }
        return false;
    }

    public bool TryUpgradeRange(bool shouldPay = true)
    {
        if (GameManager.Instance.GetCoin() - _towerData.rangeUpgradePrice >= 0)
        {
            if (shouldPay)
            {
                GameManager.Instance.PayCoin(_towerData.rangeUpgradePrice);
                _towerData.upgradesDone++;
                _towerData.rangeUpgradesDone++;
                _towerData.rangeUpgradePrice *= 2;
                _towerData.towerRange += _towerData.towerRangeIncreace;
            }
            else
            {
                _towerData.rangeUpgradePrice *= 2;
                _towerData.towerRange += _towerData.towerRangeIncreace;
            }
            OnRangeUpdate?.Invoke();
            TryEvloving();
            return true;
        }

        return false;
    }

    public bool TryUpgradeAttackSpeed(bool shouldPay = true)
    {
        if (GameManager.Instance.GetCoin() - _towerData.attackIntervalUpgradePrice >= 0)
        {
            if (shouldPay)
            {
                GameManager.Instance.PayCoin(_towerData.attackIntervalUpgradePrice);
                _towerData.upgradesDone++;
                _towerData.attackSpeedUpgradesDone++;
                _towerData.attackIntervalUpgradePrice *= 2;
                _towerData.towerAttackInterveal -= _towerData.towerIntervealDecreace;
            }
            else
            {
                _towerData.attackIntervalUpgradePrice *= 2;
                _towerData.towerAttackInterveal -= _towerData.towerIntervealDecreace;
            }
            TryEvloving();
            return true;
        }
        return false;
    }

    private void TryEvloving()
    {
        if (evolution == null) return;

        if (_towerData.upgradesDone == 5)
        {
            StartCoroutine(EvolveEffect(Vector3.one * 1.5f));
        }
    }

    private IEnumerator EvolveEffect(Vector3 targetSize)
    {
        Color start = evolutionMat.color;

        Renderer[] children;
        children = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in children)
        {
            var mats = new Material[rend.materials.Length];
            for (var j = 0; j < rend.materials.Length; j++)
            {
                mats[j] = evolutionMat;
            }
            rend.materials = mats;
        }
        while (targetSize.x - transform.localScale.x  > 0)
        {
            transform.localScale += Vector3.one * Time.deltaTime * 1.5f;
            yield return new WaitForSeconds(0.02f);
        }
        Evolve();
    }

    void ChangeMaterial(Material newMat)
    {
        Renderer[] children;
        children = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in children)
        {
            var mats = new Material[rend.materials.Length];
            for (var j = 0; j < rend.materials.Length; j++)
            {
                mats[j] = newMat;
            }
            rend.materials = mats;
        }
    }
    private void Evolve()
    {
        var evolutionGo = Instantiate(evolution.gameObject, transform.position, Quaternion.identity);
        evolutionGo.GetComponent<Tower>().SetUpAfterEvolve(_towerData);
        gameEventChannel.RaiseOnOnPokemonEvolve(this);
        Destroy(gameObject);
    }

    public void SetUpAfterEvolve(TowerData data)
    {
        _towerData.attackUpgradesDone = data.attackUpgradesDone;
        _towerData.rangeUpgradesDone = data.rangeUpgradesDone;
        _towerData.attackSpeedUpgradesDone = data.attackSpeedUpgradesDone;

        for (int i = 0; i < data.attackUpgradesDone; i++)
        {
            TryUpgradeAttack(false);
        }

        for (int i = 0; i < data.rangeUpgradesDone; i++)
        {
            TryUpgradeRange(false);
        }

        for (int i = 0; i < data.attackSpeedUpgradesDone; i++)
        {
            TryUpgradeAttackSpeed(false);
        }
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
    public int attackUpgradesDone = 0;
    public int rangeUpgradesDone = 0;
    public int attackSpeedUpgradesDone = 0;

}
