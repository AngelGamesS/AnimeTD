using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public TileType type;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum TileType{
    None,
    Water,
    Grass,
    EnemyPath
}
