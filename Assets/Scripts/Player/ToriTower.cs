using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToriTower : MonoBehaviour
{
    public int coinGain;
    public float interval;
    private float _timer;

    private void Start()
    {
        _timer = 0;
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        Vector3 startAngles = transform.localEulerAngles;
        float angle = transform.localEulerAngles.x;
        angle = (angle > 180) ? angle - 360 : angle;
        while (angle <= 10)
        {
            transform.rotation = Quaternion.Euler(transform.localEulerAngles.x + Time.fixedDeltaTime * 50f, transform.localEulerAngles.y, transform.localEulerAngles.z);
            angle = transform.localEulerAngles.x;
            yield return new WaitForSeconds(0.02f);
        }
        while (angle >= -10)
        {
            transform.rotation = Quaternion.Euler(transform.localEulerAngles.x - Time.fixedDeltaTime * 50f, transform.localEulerAngles.y, transform.localEulerAngles.z);
            angle = transform.localEulerAngles.x;
            angle = (angle > 180) ? angle - 360 : angle;
            yield return new WaitForSeconds(0.02f);
        }
        while (angle <= 0)
        {
            transform.rotation = Quaternion.Euler(transform.localEulerAngles.x + Time.fixedDeltaTime * 50f, transform.localEulerAngles.y, transform.localEulerAngles.z);
            angle = transform.localEulerAngles.x;
            angle = (angle > 180) ? angle - 360 : angle;
            yield return new WaitForSeconds(0.02f);
        }
        transform.localEulerAngles = startAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.inWave)
        {
            _timer += Time.deltaTime;
            if(_timer >= interval)
            {
                _timer = 0;
                GameManager.Instance.GainCoin(coinGain);
            }
        }
    }
}
