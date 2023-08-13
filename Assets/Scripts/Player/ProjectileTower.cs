using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : MonoBehaviour
{
    public float range;
    public LayerMask layerMask;
    public GameObject projectile;
    public float attackInterval;
    public float attackDmg;
    private GameObject target;
    private float attackTimer;
    public Transform shootTransform;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0;
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
        if (target != null)
        {
            var proj = Instantiate(projectile, shootTransform.position, Quaternion.identity);
            proj.transform.LookAt(target.transform);
            proj.GetComponent<Projectile>().SetUp(attackDmg, target.transform.position);
            attackTimer = attackInterval;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FindTarget();
        RotateToTarget();
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

    private void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, layerMask);
        if (colliders.Length > 0)
        {
            target = FindClosestOneEnemy(colliders).gameObject;
        }
    }
    private void RotateToTarget()
    {
        if(target == null)
        {
            return;
        }
        Vector3 targetPos = target.transform.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);
    }


}
