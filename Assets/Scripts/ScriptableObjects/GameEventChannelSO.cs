using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[CreateAssetMenu(fileName = "GameEventChannel", menuName = "EventChannels/GameEventChannel")]
public class GameEventChannelSO : ScriptableObject
{
    public UnityEvent<bool> OnWinLose;
    public UnityEvent<int> OnCurrentWaveHpChange;
    public UnityEvent<int> OnCoinAmountUpdate;
    public UnityEvent<int> OnLevelUp;
    public UnityEvent<float> OnUpdateExpAmount;

    public void RaiseOnWinLose(bool status) => OnWinLose?.Invoke(status);
    public void RaiseOnCurrentWaveHpChange(int cuurentAmount) => OnCurrentWaveHpChange?.Invoke(cuurentAmount);
    public void RaiseOnCoinAmountUpdate(int coins) => OnCoinAmountUpdate?.Invoke(coins);

    public void RaiseOnLevelUp(int newLevel) => OnLevelUp?.Invoke(newLevel);
    public void RaiseOnUpdateExpAmount(float currentExpAmountinPercentage) => OnUpdateExpAmount?.Invoke(currentExpAmountinPercentage);
}