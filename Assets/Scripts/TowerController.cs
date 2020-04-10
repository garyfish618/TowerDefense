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

    public  Vector3[] OrigTopSpots = {
        new Vector3(-20.59f,-11.75f, -0.7f),
        new Vector3(-20.59f, -8.75f, -0.7f),
        new Vector3(-20.59f, -5.75f, -0.7f),
        new Vector3(-20.59f, -2.75f, -0.7f),
        new Vector3(-20.59f,  1.75f, -0.7f),
        new Vector3(-20.59f,  4.75f, -0.7f),
        new Vector3(-20.59f,  7.75f, -0.7f),
        new Vector3(2.22f, 8.0f, -0.7f),
        new Vector3(2.22f, 5.0f, -0.7f),
        new Vector3(2.22f, 2.0f, -0.7f),
        new Vector3(2.22f, -1.0f, -0.7f),
        new Vector3(11.11f, 8.0f, -0.7f),
        new Vector3(11.11f, 5.0f, -0.7f),
        new Vector3(11.11f, 2.0f, -0.7f),
        new Vector3(11.11f, -1.0f, -0.7f)
    };

    public Vector3[] OrigBottomSpots = {
        new Vector3(-12.25f,0.6f,-0.7f),
        new Vector3(-12.25f,-2.4f,-0.7f),
        new Vector3(-12.25f,-5.4f,-0.7f),
        new Vector3(-12.25f,-8.4f,-0.7f),
        new Vector3(-12.25f,-11.4f,-0.7f),
        new Vector3(-12.25f, 3.6f,-0.7f),
        new Vector3(-6.0f, 3.6f, -0.7f),
        new Vector3(-6.0f, 0.6f, -0.7f),
        new Vector3(-6.0f, -2.4f, -0.7f),
        new Vector3(-6.0f, -5.4f, -0.7f),
        new Vector3(-3.0f, -10.6f, -0.7f),
        new Vector3(0, -10.6f, -0.7f),
        new Vector3(3.0f, -10.6f, -0.7f),
        new Vector3(6.0f, -10.6f, -0.7f),
        new Vector3(9.0f, -10.6f, -0.7f),
        new Vector3(12.0f, -10.6f, -0.7f),
        new Vector3(15.0f, -10.6f, -0.7f),
        new Vector3(19.41f, -7f, -0.7f),
        new Vector3(19.41f, -4.0f, -0.7f),
        new Vector3(19.41f, -1.0f, -0.7f),
        new Vector3(19.41f,  2.0f, -0.7f),
        new Vector3(22.41f,  3.7f, -0.7f),
    };

    // Start is called before the first frame update
    void Start()
    {
        contr = PersistenceController.Instance;
        baseLayer = GameObject.Find("/Grid/BaseMap").GetComponent<Tilemap>();

        if(contr.TopTowersActive.Count != 0) {
            foreach(KeyValuePair<string, int> tower in contr.TopTowersActive) {
                SpawnTower(tower.Key.Split('.')[0], OrigTopSpots[tower.Value]);
            }  
        }

        if(contr.BottomTowersActive.Count != 0) {
            foreach(KeyValuePair<string, int> tower in contr.BottomTowersActive) {
                SpawnTower(tower.Key.Split('.')[0], OrigBottomSpots[tower.Value]); // Splits off the ending tower number to have unique keys
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

        contr.TowerCount++;
    }

    public void PlaceTower(string type, int position)
    {
        //Position = 0 - Place above path
        //Position = 1 - Place below path

        if (position == 0)
        {
            //Find an unused position
            //TopTracker - 1 gurantees that we will only choose spots in the array that have not been chosen

            int TowerSpot = Mathf.FloorToInt(Random.Range(0.0f, ((float) contr.TopTracker - 1)));
            //type + TowerSpot number so no identical keys
            SpawnTower(type, contr.TopSpots[TowerSpot]);
            contr.TopTracker--;
            Vector3 temp = contr.TopSpots[contr.TopTracker];
            contr.TopSpots[contr.TopTracker] = contr.TopSpots[TowerSpot];
            contr.TopSpots[TowerSpot] = temp;
            contr.TopTowersActive.Add(type + "." + TowerSpot.ToString(), TowerSpot);
        }

        else
        {
            int TowerSpot = Mathf.FloorToInt(Random.Range(0.0f, ((float)contr.BottomTracker - 1)));
            SpawnTower(type, contr.BottomSpots[TowerSpot]);
            contr.BottomTracker--;
            Vector3 temp = contr.BottomSpots[contr.BottomTracker];
            contr.BottomSpots[contr.BottomTracker] = contr.BottomSpots[TowerSpot];
            contr.BottomSpots[TowerSpot] = temp;
            contr.BottomTowersActive.Add(type + "." + TowerSpot.ToString(), TowerSpot);
        }
    }
}
