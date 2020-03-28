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
    private int TopTracker;
    private int BottomTracker;

    private  Vector3[] TopSpots = {
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

    private Vector3[] BottomSpots = {
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

    private List<GameObject> TowersActive;

    // Start is called before the first frame update
    void Start()
    {
        //Top and Bottom trackers keep track of the last used tower spot. Any index beyond these trackers is either invalid or is being used.
        TopTracker = TopSpots.Length - 1;
        BottomTracker = BottomSpots.Length - 1;


        baseLayer = GameObject.Find("/Grid/BaseMap").GetComponent<Tilemap>();


        TowersActive = new List<GameObject>();
    }

    void Update()
    {
     
    }

    public void PlaceTower(string type, int position)
    {
        //Position = 0 - Place above path
        //Position = 1 - Place below path

        if (position == 0)
        {
            //Find an unused position
            //TopTracker - 1 gurantees that we will only choose spots in the array that have not been chosen

            int TowerSpot = Mathf.FloorToInt(Random.Range(0.0f, ((float) TopTracker - 1)));


            if (type == "BasicRocket")
            {
                TowersActive.Add(Instantiate(BasicRocket, TopSpots[TowerSpot], Quaternion.identity));
            }

            else if (type == "AdvancedRocket")
            {
                TowersActive.Add(Instantiate(AdvancedRocket, TopSpots[TowerSpot], Quaternion.identity));

            }

            else if (type == "BigRocket")
            {
                TowersActive.Add(Instantiate(BigRocket, TopSpots[TowerSpot], Quaternion.identity));


            }

            else if (type == "BasicCannon")
            {
                TowersActive.Add(Instantiate(BasicCannon, TopSpots[TowerSpot], Quaternion.identity));
            }

            else
            {
                TowersActive.Add(Instantiate(AdvancedCannon, TopSpots[TowerSpot], Quaternion.identity));
            }
            
             TopTracker--;
             Vector3 temp = TopSpots[TopTracker];
             TopSpots[TopTracker] = TopSpots[TowerSpot];
             TopSpots[TowerSpot] = temp;
              
            
        }

        else
        {
            int TowerSpot = Mathf.FloorToInt(Random.Range(0.0f, ((float)BottomTracker - 1)));

            if (type == "BasicRocket")
            {
                TowersActive.Add(Instantiate(BasicRocket, BottomSpots[TowerSpot], Quaternion.identity));
            }

            else if (type == "AdvancedRocket")
            {
                TowersActive.Add(Instantiate(AdvancedRocket, BottomSpots[TowerSpot], Quaternion.identity));
            }

            else if (type == "BigRocket")
            {
                TowersActive.Add(Instantiate(BigRocket, BottomSpots[TowerSpot], Quaternion.identity));
            }

            else if (type == "BasicCannon")
            {
                TowersActive.Add(Instantiate(BasicCannon, BottomSpots[TowerSpot], Quaternion.identity));
            }

            else
            {
                TowersActive.Add(Instantiate(AdvancedCannon, BottomSpots[TowerSpot], Quaternion.identity));
            }

            BottomTracker--;
            Vector3 temp = BottomSpots[BottomTracker];
            BottomSpots[BottomTracker] = BottomSpots[TowerSpot];
            BottomSpots[TowerSpot] = temp;

        }
    }
}
