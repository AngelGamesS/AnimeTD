using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private float dmg;
    private Vector3 targetPosition;
    public float speed;

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    public void SetUp(float dmg, Vector3 targetPosition)
    {
        this.dmg = dmg;
        this.targetPosition = targetPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().TakeDmg(dmg);
            Destroy(gameObject);
        }
    }
}
