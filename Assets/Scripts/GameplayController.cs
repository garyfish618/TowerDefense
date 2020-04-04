using UnityEngine;

public class GameplayController : MonoBehaviour
{
    private GameObject[] Enemies;
    public GameObject BasicEnemy;
    public GameObject CannonOnlyEnemy;
    public GameObject HeavyEnemy;
    public GameObject WeakFastEnemy;
    public GameObject BasicTank;
    public GameObject AdvancedTank;
    public GameObject BasicAirplane;
    public GameObject AdvancedAirplane;
    public UIController ui;
    public bool TestMode;
    public int EnemiesLeft;


    public GameObject[] Waypoints;
    


    //Game states
    public bool BuyPhase;
    public bool PlayPhase;
    public bool GameOver;

    // Start is called before the first frame update
    void Start()
    {
        BuyPhase = true;
        PlayPhase = false;
        GameOver = false;

        

    }



    void Update()
    {
        if (EnemiesLeft != 0 && PlayPhase)
        {
            for (int i = 0; i < Enemies.Length; i++)
            {
                if (Enemies[i] == null)
                {
                    continue;
                }

                int EnemyX = Mathf.FloorToInt(Enemies[i].transform.position.x);
                int EnemyY = Mathf.FloorToInt(Enemies[i].transform.position.y);

                int WayX = Mathf.FloorToInt(Waypoints[Enemies[i].GetComponent<Enemy>().CurrentWayPoint].transform.position.x);
                int WayY = Mathf.FloorToInt(Waypoints[Enemies[i].GetComponent<Enemy>().CurrentWayPoint].transform.position.y);



                if (EnemyX != WayX)
                {


                    if (EnemyX < WayX)
                    {
                        Enemies[i].transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        Enemies[i].transform.Translate(Vector3.down * Enemies[i].GetComponent<Enemy>().speed);
                    }

                    else
                    {
                        Enemies[i].transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        Enemies[i].transform.Translate(Vector3.up * Enemies[i].GetComponent<Enemy>().speed);
                    }

                }

                else if (EnemyY != WayY)
                {
                    if (EnemyY < WayY)
                    {
                        Enemies[i].transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        Enemies[i].transform.Translate(Vector3.right * Enemies[i].GetComponent<Enemy>().speed);
                    }

                    else
                    {
                        Enemies[i].transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                        Enemies[i].transform.Translate(Vector3.left * Enemies[i].GetComponent<Enemy>().speed);
                    }

                }

                else
                {
                    if (Enemies[i].GetComponent<Enemy>().CurrentWayPoint == Waypoints.Length - 1)
                    {
                        if (ui.HealthBar.value - Enemies[i].GetComponent<Enemy>().damage <= 0) {
                            PlayPhase = false;
                            GameOver = true;
                        }


                        ui.HealthBar.value -= Enemies[i].GetComponent<Enemy>().damage;
                        Destroy(Enemies[i]);
                        Enemies[i] = null;

                        EnemiesLeft--;

                    }

                    //If both are equal then we have reached the waypoint. Need to move to next waypoint
                    if (Enemies[i] != null)
                    {
                        Enemies[i].GetComponent<Enemy>().CurrentWayPoint++;
                    }

                }

            }
        }

        else if (EnemiesLeft == 0 && PlayPhase) {
            PlayPhase = false;
            BuyPhase = true;

            ui.NextLevel();
            ui.EnterBuyPhase();
        
        }

    }

    public void StartGame() {
        //Only if we are in Buy Phase can we move to Play phase. If the game is over, simply ignore request to start next level
        if (BuyPhase == true) {
            BuyPhase = false;
            PlayPhase = true;
            SpawnEnemies();
            return;
        }

        return;

    
    }


    private void SpawnEnemies() {
        
        if (TestMode)
        {
            Enemies = new GameObject[1];

            Enemies[0] = Instantiate(BasicTank) as GameObject;
            EnemiesLeft = 1;
            return;
        }

        //TODO: CHANGE HARD CODED
        Enemies = new GameObject[8];

        GameObject BasicEnemyIn = Instantiate(BasicEnemy) as GameObject;
        GameObject CannonOnlyEnemyIn = Instantiate(CannonOnlyEnemy) as GameObject;
        GameObject HeavyEnemyIn = Instantiate(HeavyEnemy) as GameObject;
        GameObject WeakFastEnemyIn = Instantiate(WeakFastEnemy) as GameObject;
        GameObject BasicTankIn = Instantiate(BasicTank) as GameObject;
        GameObject AdvancedTankIn = Instantiate(AdvancedTank) as GameObject;
        GameObject BasicAirplaneIn = Instantiate(BasicAirplane) as GameObject;
        GameObject AdvancedAirplaneIn = Instantiate(AdvancedAirplane) as GameObject;

        Enemies[0] = BasicEnemyIn;
        Enemies[1] = CannonOnlyEnemyIn;
        Enemies[2] = HeavyEnemyIn;
        Enemies[3] = WeakFastEnemyIn;
        Enemies[4] = BasicTankIn;
        Enemies[5] = AdvancedTankIn;
        Enemies[6] = BasicAirplaneIn;
        Enemies[7] = AdvancedAirplaneIn;
        EnemiesLeft = 8;

    }





}
