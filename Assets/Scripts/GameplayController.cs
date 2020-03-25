using UnityEngine;

public class GameplayController : MonoBehaviour
{
    GameObject[] Enemies;
    public GameObject BasicEnemy;
    public GameObject[] Waypoints;
    private int CurrentWayPoint;
    private bool done;
    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        //TODO: Update to multiple enemies
        // enemies = new GameObject[5];

        //for (i = 0; i < enemies.Length; i++)
        //{
        //  enemies[i] = new basicEnemy();
        //}

        BasicEnemy = Instantiate(BasicEnemy);
        done = false;

    }



    void Update()
    {

        if (!done)
        {
            Debug.Log("Here");
            int EnemyX = Mathf.FloorToInt(BasicEnemy.transform.position.x);
            int EnemyY = Mathf.FloorToInt(BasicEnemy.transform.position.y);

            int WayX = Mathf.FloorToInt(Waypoints[CurrentWayPoint].transform.position.x);
            int WayY = Mathf.FloorToInt(Waypoints[CurrentWayPoint].transform.position.y);



            if (EnemyX != WayX)
            {
                

                if (EnemyX < WayX)
                {
                    BasicEnemy.transform.Translate(Vector3.down * speed);
                }

                else
                {
                    BasicEnemy.transform.Translate(Vector3.up * speed);
                }

            }

            else if (EnemyY != WayY)
            {
                if (EnemyY < WayY)
                {
                    BasicEnemy.transform.Translate(Vector3.right * speed);
                }

                else
                {
                    BasicEnemy.transform.Translate(Vector3.left * speed);
                }

            }

            else
            {
                if (CurrentWayPoint == Waypoints.Length - 1)
                {
                    Destroy(BasicEnemy);
                    done = true;
                }

                //If both are equal then we have reached the waypoint. Need to move to next waypoint
                CurrentWayPoint++;
        
            }
        }
    }





}
