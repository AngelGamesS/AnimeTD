using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO gameEventChannel;

    Label waveHealth;
    Label coinAmount;
    Label level;
    ProgressBar levelProgressBar;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        waveHealth = root.Q<Label>("Health");
        coinAmount = root.Q<Label>("Coin");
        level = root.Q<Label>("Level");
        levelProgressBar = root.Q<ProgressBar>("ExpProgressBar");

        gameEventChannel.OnCurrentWaveHpChange.AddListener(HandleWaveHealthChange);
        gameEventChannel.OnLevelUp.AddListener(HandleLevelUp);
        gameEventChannel.OnCoinAmountUpdate.AddListener(HandleCoinUpdate);
    }

    private void HandleCoinUpdate(int newAmount)
    {
        coinAmount.text = coinAmount.ToString();
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
