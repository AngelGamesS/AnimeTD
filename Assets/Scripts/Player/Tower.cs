using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public LayerMask tileMask;
    public GameObject rangeObject;

    private void Start()
    {
        LookAtNearestPath();
    }

    private void LookAtNearestPath()
    {
        var tiles = Physics.OverlapSphere(transform.position, 10f, tileMask);
        TileComponent closestTile = GetClosestPathTile(tiles);

        Vector3 fixedTilePos = new Vector3(closestTile.transform.position.x, transform.position.y, closestTile.transform.position.z);
        Vector3 dir = fixedTilePos - transform.position;

        transform.LookAt(dir);

    }

    private TileComponent GetClosestPathTile(Collider[] tiles)
    {
        float nearestTileDist = 100f;
        TileComponent closestTile = null;
        foreach (var tile in tiles)
        {
            TileComponent tileComp = tile.GetComponent<TileComponent>();
            if (tileComp != null && tileComp.type == TileType.EnemyPath)
            {
                var tempDist = Vector3.Distance(tileComp.transform.position, transform.position);
                if (tempDist < nearestTileDist)
                {
                    nearestTileDist = tempDist;
                    closestTile = tileComp;
                }
            }
        }

        return closestTile;
    }

    private void OnMouseEnter()
    {
        if (rangeObject != null)
            rangeObject.SetActive(true);
    }
    private void OnMouseExit()
    {
        if (rangeObject != null)
            rangeObject.SetActive(false);
    }
}
