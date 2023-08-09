using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour
{

    public TileType type;
    public GameObject overEffect;
    public bool placeable;
    private bool towerWasPlaced;
        
    // Start is called before the first frame update
    void Start()
    {
        towerWasPlaced = false;
        if(type == TileType.EnemyPath){
            placeable = false;
        }
    }

    private void OnMouseEnter()
    {
        if(overEffect != null)
            overEffect.SetActive(true);
    }
    private void OnMouseExit()
    {
        if (overEffect != null)
            overEffect.SetActive(false);
    }

    private void OnMouseUp()
    {
        if (towerWasPlaced == false && type != TileType.EnemyPath)
            CardPanelController.Instance.ChooseTower(this);
    }

    public void SetPlaced(bool status) => towerWasPlaced = true;
}

public enum TileType{
    None,
    Water,
    Grass,
    EnemyPath
}
