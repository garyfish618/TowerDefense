using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceController : MonoBehaviour
{
    public static PersistenceController Instance { get; private set; } // Set instance from ONLY within this class

    //Values below tied to instance not class
    
    //Game States
    public bool BuyPhase;
    public bool PlayPhase;
    public bool GameOver;
    
    public int RandomEnemies;
    public int level;
    public int money;
    public int TopTurretsPlaced;
    public int BottomTurretsPlaced;
    public float health;
    public Dictionary<string, int> Inventory;
    public Dictionary<string, int> TopTowersActive;
    public Dictionary<string, int> BottomTowersActive;
    //Top and Bottom trackers keep track of the last used tower spot. Any index beyond these trackers is either invalid or is being used.
    public int TopTracker;
    public int BottomTracker;
    public bool TestMode;
    public bool SoundAudible;

    public GameObject[] Towers;
    public int TowerCount;
    public  Vector3[] TopSpots = {
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

    public Vector3[] BottomSpots = {
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



    private void Awake()
    {
        if (Instance == null)
        {
            StartGame();
            DontDestroyOnLoad(gameObject); // gameObject = the game object this script lives on
        }

        else //Gives singleton property. stops unity from trying to duplicate and create more instances
        {
            Destroy(gameObject);
        }
    }


    public void StartGame() {
         Instance = this; // Set to the instance that is being created
            BuyPhase = true;
            PlayPhase = false;
            GameOver = false;
            RandomEnemies=20;
            TopTurretsPlaced=0;
            BottomTurretsPlaced=0;
            level = 1;
            money=700;
            health=1.0f;
            TopTracker = TopSpots.Length - 1;
            BottomTracker = BottomSpots.Length - 1;
            TopTowersActive = new Dictionary<string,int>();
            BottomTowersActive = new Dictionary<string,int>();
            SoundAudible = true;
            Inventory = new Dictionary<string,int>() {
                {"TownFull", 0},
                {"TownHalf", 0},
                {"TownQuarter", 0},
                {"DestroyEnemies", 0}
            };

            Towers = new GameObject[37];
            TowerCount = 0;


    }
}
