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
