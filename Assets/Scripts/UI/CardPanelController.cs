using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardPanelController : MonoBehaviour
{
    public static CardPanelController Instance = null;
    public VisualTreeAsset cardAsset;
    private const string towersPath = "Towers";
    private TowerDataSO[] towers;
    private TileComponent selectedTile = null;
    ScrollView sv;
    private VisualElement closeBTN;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable() {
        var root = GetComponent<UIDocument>().rootVisualElement;
        sv = root.Q<ScrollView>("ScrollView");
        towers = Resources.LoadAll<TowerDataSO>(towersPath);
        closeBTN = root.Q<VisualElement>("CloseBTN");
        closeBTN.AddManipulator(new Clickable(evt => HidePanel()));
        //AddTowersToScrollView();
    }
    private void AddTowersToScrollView()
    {
        sv.Clear();
        foreach (var tower in towers)
        {
            if (tower.isPerchuasable && tower.minLevelRequired <= GameManager.Instance.GetLevel())
            {
                var towerCard = cardAsset.Instantiate();
                towerCard.Q<Label>("Title").text = $"{tower.towerName}";
                towerCard.Q<VisualElement>("Image").style.backgroundImage = new StyleBackground(tower.image);
                towerCard.Q<Label>("Price").text = $"Price: {tower.price}";
                towerCard.Q<VisualElement>("Container").AddManipulator(new Clickable(evt => HandlePickCard(tower)));
                sv.Add(towerCard);
            }
        }
    }
    public void ChooseTower(TileComponent tile)
    {
        if(selectedTile == null && gameObject.activeSelf)
        {
            selectedTile = tile;
            ShowPanel();
        }
    }
    private void HandlePickCard(TowerDataSO selectedTower)
    {
        if(GameManager.Instance.TryBuyTower(selectedTower, selectedTile.transform.position + Vector3.up * 0.25f))
        {
            selectedTile.SetPlaced(true);
            selectedTile = null;
        }
        HidePanel();
    }
    private void ShowPanel()
    {
        closeBTN.SetEnabled(true);
        closeBTN.style.visibility = Visibility.Visible;
        AddTowersToScrollView();
        sv.AddToClassList("moveUp");
    }

    private void HidePanel()
    {
        selectedTile = null;

        closeBTN.SetEnabled(false);
        closeBTN.style.visibility = Visibility.Hidden;
        sv.RemoveFromClassList("moveUp");
    }
}
