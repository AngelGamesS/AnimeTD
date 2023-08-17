using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO gameEventChannel;
    public GameObject cardsPanel;
    private Label waveHealth;
    private Label coinAmount;
    private Label level;
    private Label waveNumber;
    private ProgressBar levelProgressBar;

    private VisualElement towerDetails;
    private VisualElement upgradeAttack;
    private VisualElement upgradeRange;
    private VisualElement upgradeSpeed;
    private VisualElement towerImg;
    private VisualElement closeBTN;
    private Label towerName;
    private Label attackText;
    private Label rangeText;
    private Label attackSpeedText;

    private Tower _selectedTower;
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

        towerName = root.Q<Label>("TowerName");
        attackText = root.Q<Label>("AttackText");
        rangeText = root.Q<Label>("RangeText");
        attackSpeedText = root.Q<Label>("AttackSpeedText");
        towerImg = root.Q<VisualElement>("TowerImg");
        towerDetails = root.Q<VisualElement>("TowerDetails");
        upgradeAttack = root.Q<VisualElement>("UpgradeAttack");
        upgradeRange = root.Q<VisualElement>("UpgradeRange");
        upgradeSpeed = root.Q<VisualElement>("UpgradeSpeed");
        closeBTN = root.Q<VisualElement>("CloseBTN");

        upgradeAttack.AddManipulator(new Clickable(evt => HandleUpgrade(0)));
        upgradeRange.AddManipulator(new Clickable(evt => HandleUpgrade(1)));
        upgradeSpeed.AddManipulator(new Clickable(evt => HandleUpgrade(2)));
        closeBTN.AddManipulator(new Clickable(evt => CloseSelectedPanel()));

        gameEventChannel.OnGameWaveStatusChange.AddListener(HandleWaveChange);
        gameEventChannel.OnCurrentWaveHpChange.AddListener(HandleWaveHealthChange);
        gameEventChannel.OnLevelUp.AddListener(HandleLevelUp);
        gameEventChannel.OnCoinAmountUpdate.AddListener(HandleCoinUpdate);
        gameEventChannel.OnUpdateExpAmount.AddListener(HandleExpChange);
        gameEventChannel.OnTowerSelected.AddListener(HandleTowerSelected);

        gameEventChannel.OnPokemonEvolve.AddListener(HandlePokemonEvolve);

        HandleCoinUpdate(GameManager.Instance.GetCoin());
        HandleWaveHealthChange(GameManager.Instance.GetHP());
        HandleExpChange(GameManager.Instance.GetExp());
        HandleLevelUp(GameManager.Instance.GetLevel());
    }

    private void HandlePokemonEvolve(Tower tower)
    {
        Invoke("CloseSelectedPanel", 0.1f);
    }

    private void CloseSelectedPanel()
    {
        towerDetails.style.visibility = Visibility.Hidden;
        towerDetails.SetEnabled(false);
        cardsPanel.SetActive(true);
    }

    private void HandleUpgrade(int value)
    {
        if (_selectedTower == null) return;
        switch (value)
        {
            case 0:
                if (_selectedTower.TryUpgradeAttack())
                    HandleTowerSelected(_selectedTower);
                break;
            case 1:
                if (_selectedTower.TryUpgradeRange())
                    HandleTowerSelected(_selectedTower);
                break;
            case 2:
                if (_selectedTower.TryUpgradeAttackSpeed())
                    HandleTowerSelected(_selectedTower);
                break;
            default:
                break;
        }
    }

    private void HandleTowerSelected(Tower tower)
    {
        cardsPanel.SetActive(false);

        _selectedTower = tower;
        var data = tower.GetTowerData();
        towerName.text = data.towerName;
        towerImg.style.backgroundImage = new StyleBackground(data.towerSprite);
        attackText.text = $"Attack Damage: {data.towerDmg}";
        rangeText.text = $"Range: {data.towerRange}";
        attackSpeedText.text = $"Attack Speed: {data.towerAttackInterveal}";

        towerDetails.style.visibility = Visibility.Visible;
        towerDetails.SetEnabled(true);
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
