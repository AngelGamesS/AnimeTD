using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO gameEventChannel;

    private Label waveHealth;
    private Label coinAmount;
    private Label level;
    private Label waveNumber;
    private ProgressBar levelProgressBar;
    private int _waveIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        waveHealth = root.Q<Label>("Health");
        coinAmount = root.Q<Label>("Coin");
        level = root.Q<Label>("Level");
        waveNumber = root.Q<Label>("WaveNumber");
        levelProgressBar = root.Q<ProgressBar>("ExpProgressBar");
        waveNumber.style.opacity = 0;

        gameEventChannel.OnGameWaveStatusChange.AddListener(HandleWaveChange);
        gameEventChannel.OnCurrentWaveHpChange.AddListener(HandleWaveHealthChange);
        gameEventChannel.OnLevelUp.AddListener(HandleLevelUp);
        gameEventChannel.OnCoinAmountUpdate.AddListener(HandleCoinUpdate);
        gameEventChannel.OnUpdateExpAmount.AddListener(HandleExpChange);

        HandleCoinUpdate(GameManager.Instance.GetCoin());
    }

    private void HandleWaveChange(bool status, int waveIndex)
    {
        if (status) { 
            waveNumber.text = $"Wave {_waveIndex}";
            StartCoroutine(LabelOpacityInOut(waveNumber));
        }
        else
            _waveIndex = waveIndex;
    }

    private IEnumerator LabelOpacityInOut(Label label)
    {
        while(label.style.opacity.value < 0.99f)
        {
            label.style.opacity = label.style.opacity.value + 0.02f;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(2f);
        while (label.style.opacity.value > 0.1f)
        {
            label.style.opacity = label.style.opacity.value - 0.02f;
            yield return new WaitForSeconds(0.02f);
        }
        waveNumber.style.opacity = 0;
    }

    private void HandleExpChange(float exp)
    {
        levelProgressBar.value = exp;
    }

    private void HandleCoinUpdate(int newAmount)
    {
        coinAmount.text = newAmount.ToString();
    }

    private void HandleLevelUp(int newLevel)
    {
        levelProgressBar.value = 0;
        level.text = $"level {newLevel}";
    }

    private void HandleWaveHealthChange(int currentHP)
    {
        waveHealth.text = $"{currentHP}HP";
    }
}
