using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class ProjectileTower : Tower
{
    [Header("Projectile Tower")]
    public GameObject projectile;
    [SerializeField] private DecalProjector _decalPrjector;
    private float attackTimer;
    public Transform shootTransform;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        attackTimer = 0;
        _decalPrjector.size = new Vector3(_towerData.towerRange*2, _towerData.towerRange * 2, _decalPrjector.size.z);
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        if (_target != null)
        {
            var proj = Instantiate(projectile, shootTransform.position, Quaternion.identity);
            proj.transform.LookAt(_target.transform);
            proj.GetComponent<Projectile>().SetUp(_towerData.towerDmg, _target.transform.position);
            attackTimer = _towerData.towerAttackInterveal;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FindTarget();
        RotateToTarget();
    }

    private void RotateToTarget()
    {
        if(_target == null)
        {
            return;
        }
        Vector3 targetPos = _target.transform.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position,_towerData.towerRange);
    }
}
