using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public int coin = 15;
    public float exp = 15;
    public GameObject deathEffect;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera myCam; 
    private NavMeshAgent navMeshAgent;
    public Action<Enemy> OnDeath;
    
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(GameManager.Instance.endPortal.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EndPortal>() != null)
        {
            GameManager.Instance.TakeDamage();
            Destroy(gameObject);
        }
    }

    public void TakeDmg(float dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            GameManager.Instance.HandleEnemyDeath(exp, coin);
            Destroy(gameObject);
            if(deathEffect != null)
            {
                var go = Instantiate(deathEffect, transform.position, transform.rotation);
                Destroy(go, 5f);
            }
        }
    }
    private void OnDestroy()
    {
        OnDeath?.Invoke(this);
    }
    public void ChangeView(bool enemy)
    {
        if (enemy)
            myCam.Priority = 100;
        else
            myCam.Priority = 0;
    }
}
