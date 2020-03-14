using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectPlacementController : MonoBehaviour
{
    private Tilemap towerLayer;
    private Tilemap baseLayer;
    private Tile basicTowerTile;
    public Sprite basicTower;

    // Start is called before the first frame update
    void Start()
    {
        towerLayer = GameObject.Find("/Grid/TowerLayer").GetComponent<Tilemap>();
        baseLayer = GameObject.Find("/Grid/BaseMap").GetComponent<Tilemap>();


        basicTowerTile = ScriptableObject.CreateInstance<Tile>();
        basicTowerTile.sprite = basicTower;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int pos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            pos.z = 0;

            string baseSprite = baseLayer.GetSprite(pos).ToString();

            //Cant place on ground tiles
            if (baseSprite == "towerDefense_tilesheet_167 (UnityEngine.Sprite)" || baseSprite == "towerDefense_tilesheet_93 (UnityEngine.Sprite)")
            {
                return;            
            }

            towerLayer.SetTile(pos, basicTowerTile);
        }
    }
}