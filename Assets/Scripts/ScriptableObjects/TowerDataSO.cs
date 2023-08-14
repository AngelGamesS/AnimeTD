using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TowerData",menuName ="ScriptableObjects/Tower")]
public class TowerDataSO : ScriptableObject
{
    public string towerName;
    public string description;
    public Sprite image;
    public GameObject prefab;
    public int price;
    public int minLevelRequired = 1;
    public bool isPerchuasable = false;
}
