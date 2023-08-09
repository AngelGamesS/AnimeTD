using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    private NavMeshAgent navMeshAgent;
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
}
