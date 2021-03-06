﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayController : MonoBehaviour
{
    public GameObject BasicEnemy;
    public GameObject CannonOnlyEnemy;
    public GameObject HeavyEnemy;
    public GameObject WeakFastEnemy;
    public GameObject BasicTank;
    public GameObject AdvancedTank;
    public GameObject BasicAirplane;
    public GameObject AdvancedAirplane;
    public UIController ui;
    public int EnemiesLeft;

    public InventoryController inventory;

    public GameObject[] Waypoints;
    private GameObject[] Enemies;
    private PersistenceController contr;


    void Start() {
        contr = PersistenceController.Instance;

    }

    void FixedUpdate()
    {
        if (EnemiesLeft != 0 && contr.PlayPhase)
        {
            for (int i = 0; i < Enemies.Length; i++)
            {
                // If the enemy is null it has already been destroyed, continue to next enemy
                if (Enemies[i] == null)
                {
                    continue;
                }

                Enemy enemy = Enemies[i].GetComponent<Enemy>();

                //If we are currently on a cooldown do not spawn/move any further enemies
                if (enemy.OnWait) 
                {
                    break;
                }

                //If an enemy just spawned we want to wait before spawning another
                if (enemy.JustSpawned)
                {
                    StartCoroutine(WaitForSpawn(enemy));
                    enemy.OnWait = true;
                    break;
                }

                if (!Enemies[i].activeSelf)
                {
                    Enemies[i].SetActive(true);

                }

                int EnemyX = Mathf.FloorToInt(Enemies[i].transform.position.x);
                int EnemyY = Mathf.FloorToInt(Enemies[i].transform.position.y);

                int WayX = Mathf.FloorToInt(Waypoints[enemy.CurrentWayPoint].transform.position.x);
                int WayY = Mathf.FloorToInt(Waypoints[enemy.CurrentWayPoint].transform.position.y);



                if (EnemyX != WayX)
                {


                    if (EnemyX < WayX)
                    {
                        Enemies[i].transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        Enemies[i].transform.Translate(Vector3.down * enemy.speed);
                    }

                    else
                    {
                        Enemies[i].transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        Enemies[i].transform.Translate(Vector3.up * enemy.speed);
                    }

                }

                else if (EnemyY != WayY)
                {
                    if (EnemyY < WayY)
                    {
                        Enemies[i].transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        Enemies[i].transform.Translate(Vector3.right * enemy.speed);
                    }

                    else
                    {
                        Enemies[i].transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                        Enemies[i].transform.Translate(Vector3.left * enemy.speed);
                    }

                }

                else
                {
                    if (Enemies[i].GetComponent<Enemy>().CurrentWayPoint == Waypoints.Length - 1 && Enemies[i].GetComponent<Enemy>().health > 0)
                    {
                        if (ui.HealthBar.value - enemy.damage <= 0) { 
                            contr.health = 0;
                            ui.SetHealth(contr.health);
                            contr.PlayPhase = false;
                            contr.GameOver = true;
                            DestroyAllEnemies();
                            Enemies = null;
                            EnemiesLeft = 0;
                            ui.GameOver(); 
                            return;
                        }
                        
                        ui.SetHealth(contr.health - enemy.damage);
                        Destroy(Enemies[i]);
                        Enemies[i] = null;

                        EnemiesLeft--;

                    }

                    //If both are equal then we have reached the waypoint. Need to move to next waypoint
                    if (Enemies[i] != null)
                    {
                        enemy.CurrentWayPoint++;
                    }

                }

            }
        }

        else if (EnemiesLeft == 0 && contr.PlayPhase) {
            contr.PlayPhase = false;
            contr.BuyPhase = true;

            //Chances for power up's
            int chance = Random.Range(1,51); 

            //2% chance for full town repair
            if (chance == 1) {
                
                GiveAward("TownFull");
                ui.NextLevel();
                ui.EnterBuyPhase();
                return;
            }

            //10% chance for half town repair
            chance = Random.Range(1,11);
            if (chance == 1) {
                GiveAward("TownHalf");
                ui.NextLevel();
                ui.EnterBuyPhase();
                return;
            }

            //2% chance for destroy all enemies
            chance = Random.Range(1,51);
            if (chance == 1) {
                GiveAward("DestroyEnemies");
                ui.NextLevel();
                ui.EnterBuyPhase();
                return;
            }

            //40% chance for quarter town repair
            chance = Random.Range(1, 11);
            if(chance <= 4) {
                GiveAward("TownQuarter");
                ui.NextLevel();
                ui.EnterBuyPhase();
                return;
            }

            ui.NextLevel();
            ui.EnterBuyPhase();

            
        
        }

    }

    public void DestroyAllEnemies() {
        contr.PlayPhase = false;

        foreach(GameObject enemy in Enemies) {
            if(enemy == null) {
                continue;
            }
            Destroy(enemy);
        }
        EnemiesLeft = 0;
        contr.PlayPhase = false;
        contr.BuyPhase = true;
        ui.NextLevel();
        ui.EnterBuyPhase();
    }

    public void StartGame() {
        //Only if we are in Buy Phase can we move to Play phase. If the game is over, simply ignore request to start next level
        if (contr.BuyPhase == true) {
            contr.BuyPhase = false;
            SpawnEnemies();
            return;
        }

        return;

    
    }

    public void GiveAward(string name) {
        if(contr.GameOver) {
            return;
        }

        inventory.AddItem(name);
        ui.ItemReceived(name);
    }

    IEnumerator WaitForSpawn(Enemy en)
    {
        if(contr.level >= 15) {
            yield return new WaitForSeconds(0.5f);
            en.OnWait = false;
            en.JustSpawned = false;
            
        }

        if(contr.level >= 5) {
            yield return new WaitForSeconds(1);
            en.OnWait = false;
            en.JustSpawned = false;
        }
             
        yield return new WaitForSeconds(en.SpawnCooldown); // Wait some time and then clear enemy for movement
        en.OnWait = false;
        en.JustSpawned = false;

    }

    private void SpawnEnemies()
    {

        if (contr.TestMode)
        {
            Enemies = new GameObject[8];

            Enemies[0] = Instantiate(BasicTank) as GameObject;
            Enemies[1] = Instantiate(AdvancedTank) as GameObject;
            Enemies[2] = Instantiate(BasicAirplane) as GameObject;
            Enemies[3] = Instantiate(AdvancedAirplane) as GameObject;
            Enemies[4] = Instantiate(BasicEnemy) as GameObject;
            Enemies[5] = Instantiate(HeavyEnemy) as GameObject;
            Enemies[6] = Instantiate(WeakFastEnemy) as GameObject;
            Enemies[7] = Instantiate(CannonOnlyEnemy) as GameObject;


            EnemiesLeft = 8;


            for(int i = 0; i < Enemies.Length; i++) {
                Enemies[i].SetActive(false);
                Enemies[i].GetComponent<Enemy>().JustSpawned = true;
            }

            contr.PlayPhase = true;
            return;
        }

        //Levels
        switch (contr.level) {

            case 1:
                Enemies = new GameObject[6];
                EnemiesLeft = 6;

                for (int i = 0; i < Enemies.Length; i++)
                {
                    Enemies[i] = Instantiate(BasicEnemy) as GameObject;
                    Enemies[i].SetActive(false);
                    Enemies[i].GetComponent<Enemy>().JustSpawned = true;
                }

                break;

            case 2:
                Enemies = new GameObject[10];
                EnemiesLeft = 10;

                for (int i = 0; i < 6; i++)
                {
                    Enemies[i] = Instantiate(BasicEnemy) as GameObject;
                    Enemies[i].SetActive(false);
                    Enemies[i].GetComponent<Enemy>().JustSpawned = true;
                }

                for (int i = 6; i < 9; i++)
                {
                    Enemies[i] = Instantiate(HeavyEnemy) as GameObject;
                    Enemies[i].SetActive(false);
                    Enemies[i].GetComponent<Enemy>().JustSpawned = true;
                }

                Enemies[9] = Instantiate(WeakFastEnemy) as GameObject;
                Enemies[9].SetActive(false);
                Enemies[9].GetComponent<Enemy>().JustSpawned = true;

                break;

            case 3:
                Enemies = new GameObject[12];
                EnemiesLeft = 11;

                for (int i = 0; i < 6; i++)
                {
                    Enemies[i] = Instantiate(BasicEnemy) as GameObject;
                    Enemies[i].SetActive(false);
                    Enemies[i].GetComponent<Enemy>().JustSpawned = true;
                }

                for (int i = 6; i < 9; i++)
                {
                    Enemies[i] = Instantiate(HeavyEnemy) as GameObject;
                    Enemies[i].SetActive(false);
                    Enemies[i].GetComponent<Enemy>().JustSpawned = true;
                }

                Enemies[9] = Instantiate(WeakFastEnemy) as GameObject;
                Enemies[9].SetActive(false);
                Enemies[9].GetComponent<Enemy>().JustSpawned = true;

                Enemies[10] = Instantiate(BasicTank) as GameObject;
                Enemies[10].SetActive(false);
                Enemies[10].GetComponent<Enemy>().JustSpawned = true;

                break;

            case 4:
                Enemies = new GameObject[18];
                EnemiesLeft = 17;

                Enemies[0] = Instantiate(WeakFastEnemy) as GameObject;
                Enemies[0].SetActive(false);
                Enemies[0].GetComponent<Enemy>().JustSpawned = true;

                Enemies[1] = Instantiate(CannonOnlyEnemy) as GameObject;
                Enemies[1].SetActive(false);
                Enemies[1].GetComponent<Enemy>().JustSpawned = true;

                for (int i = 2; i < 5; i++)
                {
                    Enemies[i] = Instantiate(BasicAirplane) as GameObject;
                    Enemies[i].SetActive(false);
                    Enemies[i].GetComponent<Enemy>().JustSpawned = true;

                }

                Enemies[5] = Instantiate(AdvancedTank) as GameObject;
                Enemies[5].SetActive(false);
                Enemies[5].GetComponent<Enemy>().JustSpawned = true;

                for (int i = 6; i < 13; i++)
                {
                    Enemies[i] = Instantiate(BasicEnemy) as GameObject;
                    Enemies[i].SetActive(false);
                    Enemies[i].GetComponent<Enemy>().JustSpawned = true;
                }

                for (int i = 13; i < 16; i++)
                {
                    Enemies[i] = Instantiate(HeavyEnemy) as GameObject;
                    Enemies[i].SetActive(false);
                    Enemies[i].GetComponent<Enemy>().JustSpawned = true;
                }


                Enemies[16] = Instantiate(BasicTank) as GameObject;
                Enemies[16].SetActive(false);
                Enemies[16].GetComponent<Enemy>().JustSpawned = true;

                break;

            //Use random enemy generation
            default:
                Enemies = new GameObject[contr.RandomEnemies];
                EnemiesLeft = contr.RandomEnemies;

                for(int i = 0; i < contr.RandomEnemies; i++) {
                    //Random enemy
                    switch(Random.Range(1,9)) {
                        
                        case 1:
                            Enemies[i] = Instantiate(BasicTank) as GameObject;
                            break;

                        case 2:
                            Enemies[i] = Instantiate(AdvancedTank) as GameObject;
                            break;

                        case 3:
                            Enemies[i] = Instantiate(BasicAirplane) as GameObject;
                            break;

                        case 4:
                            Enemies[i] = Instantiate(AdvancedAirplane) as GameObject;
                            break;

                        case 5:
                            Enemies[i] = Instantiate(BasicEnemy) as GameObject;
                            break;


                        case 6:
                            Enemies[i] = Instantiate(CannonOnlyEnemy) as GameObject;
                            break;

                        case 7:
                            Enemies[i] = Instantiate(HeavyEnemy) as GameObject;
                            break;

                        case 8:
                            Enemies[i] = Instantiate(WeakFastEnemy) as GameObject;
                            break;

                    }
                    Enemies[i].SetActive(false);
                    Enemies[i].GetComponent<Enemy>().JustSpawned = true;
                }

                //Difficulty scaling
                int HealthMultiplier = 1;
                float SpeedMultiplier = 1.0f;
                float MoneyMultiplier = 1.0f;
                
                if(contr.level >= 15) {
                    HealthMultiplier = 2;
                    SpeedMultiplier = 2.0f;
                    MoneyMultiplier = 0.5f;
                }
                
                if(contr.level >= 20) {
                    HealthMultiplier = 3;
                    SpeedMultiplier = 2.5f;
                    MoneyMultiplier = 0.25f;
                }

                for(int i = 0; i < Enemies.Length; i++) {
                    Enemy en = Enemies[i].GetComponent<Enemy>();
                    en.health *= HealthMultiplier;
                    en.speed *= SpeedMultiplier;
                    en.worth = Mathf.FloorToInt(en.worth * MoneyMultiplier);
                }

                contr.RandomEnemies += 5;
                break;


        }

        contr.PlayPhase = true;
    }


}
