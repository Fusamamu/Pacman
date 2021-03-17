using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    public Grid         mainGrid;
    public Tilemap      wall_TileMap;

    public GameObject   BigCoin_prefab;
    public GameObject   Coin_prefab;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = wall_TileMap.GetCellCenterLocal(new Vector3Int(1, 1, 1));

        Instantiate(BigCoin_prefab, pos, Quaternion.identity, mainGrid.transform);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
