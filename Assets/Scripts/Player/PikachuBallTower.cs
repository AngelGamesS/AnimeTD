using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PikachuBallTower : MonoBehaviour
{
    [Range(1.1f,2f)] public float dmgIncreasePeracentage;
    public float range;
    public float checkInterval;
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] private DecalProjector _decalPrjector;
    private float _timer;
    private List<Tower> towers = new List<Tower>();
    // Start is called before the first frame update
    void Start()
    {
        _decalPrjector.size = new Vector3(range * 2, range * 2, _decalPrjector.size.z);
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= checkInterval)
        {
            _timer = 0;
            CheckTowersInRange();
        }
    }

    private void CheckTowersInRange()
    {
        var potentialTowers = Physics.OverlapSphere(transform.position,range, _layerMask);

        for (int i = 0; i < potentialTowers.Length; i++)
        {
            Tower tower = potentialTowers[i].GetComponent<Tower>();
            if(tower != null && !towers.Contains(tower))
            {
                tower.GetTowerData().towerDmg *= dmgIncreasePeracentage;
                towers.Add(tower);
            }
        }
    }

    private void OnMouseEnter()
    {
        if (_decalPrjector.gameObject != null)
            _decalPrjector.gameObject.SetActive(true);
    }
    private void OnMouseExit()
    {
        if (_decalPrjector.gameObject != null)
            _decalPrjector.gameObject.SetActive(false);
    }

}
