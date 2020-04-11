using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerController : MonoBehaviour
{
    public GameObject BasicRocket;
    public GameObject AdvancedRocket;
    public GameObject BigRocket;
    public GameObject BasicCannon;
    public GameObject AdvancedCannon;

    private GameObject PlacementMode;
    private Tilemap baseLayer;
    private PersistenceController contr;

    // Start is called before the first frame update
    void Start()
    {
        contr = PersistenceController.Instance;
        baseLayer = GameObject.Find("/Grid/BaseMap").GetComponent<Tilemap>();

        if(contr.TopTowersActive.Count != 0) {
            foreach(KeyValuePair<string, int> tower in contr.TopTowersActive) {
                SpawnTower(tower.Key.Split('.')[0], contr.TopSpots[tower.Value]);
            }  
        }

        if(contr.BottomTowersActive.Count != 0) {
            foreach(KeyValuePair<string, int> tower in contr.BottomTowersActive) {
                SpawnTower(tower.Key.Split('.')[0], contr.BottomSpots[tower.Value]); // Splits off the ending tower number to have unique keys
            }
        }
        
    }

    void Update()
    {
        if(contr.GameOver) {
            foreach(GameObject obj in contr.Towers) {
                Destroy(obj);
            }
        }

    }

    private void SpawnTower(string type, Vector3 position) {


        if (type == "BasicRocket")
        {
            contr.Towers[contr.TowerCount] = Instantiate(BasicRocket, position, Quaternion.identity);
        }

        else if (type == "AdvancedRocket")
        {
            contr.Towers[contr.TowerCount] = Instantiate(AdvancedRocket, position, Quaternion.identity);

        }

        else if (type == "BigRocket")
        {
            contr.Towers[contr.TowerCount] = Instantiate(BigRocket, position, Quaternion.identity);


        }

        else if (type == "BasicCannon")
        {
            contr.Towers[contr.TowerCount] = Instantiate(BasicCannon, position, Quaternion.identity);
        }

        else
        {
            contr.Towers[contr.TowerCount] = Instantiate(AdvancedCannon, position, Quaternion.identity);
        }

        
    }

    public void PlaceTower(string type, int position)
    {
        //Position = 0 - Place above path
        //Position = 1 - Place below path

        if (position == 0)
        {
            //Find an unused position

            int TowerSpot;
            while(true)
            {
                TowerSpot = Random.Range(0, contr.TopSpots.Length);

                if(contr.TopSpotsTaken[TowerSpot]) {
                    continue;
                }

                break;
            }
            
            SpawnTower(type, contr.TopSpots[TowerSpot]);
            contr.TopSpotsTaken[TowerSpot] = true;
            contr.TopTowersActive.Add(type + "." + TowerSpot , TowerSpot);
            
        }

        else
        {
           //Find an unused position

            int TowerSpot;
            while(true)
            {
                TowerSpot = Random.Range(0, contr.BottomSpots.Length);

                if(contr.BottomSpotsTaken[TowerSpot]) {
                    continue;
                }

                break;
            }
            
            SpawnTower(type, contr.BottomSpots[TowerSpot]);
            contr.BottomSpotsTaken[TowerSpot] = true;
            contr.BottomTowersActive.Add(type + "." + TowerSpot , TowerSpot);
        }

        contr.TowerCount++;
    }
}
