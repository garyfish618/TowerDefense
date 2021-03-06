﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    public int CurrentWayPoint;
    public float damage;
    public Slider HealthBar;
    public int SpawnCooldown;
    public int worth;
    public bool JustSpawned; // This tells our gameplay controller that this enemy just spawned and their needs to be a wait before it moved
    public bool OnWait; // Tells our gameplay controller that this enemy is currently on a waiting period and does not need to be moved
    public bool targeted;
    

    private int OriginalHealth;
    private UIController ui;
    private AudioManager aud;
    private GameplayController game;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar.value = 1;
        OriginalHealth = health;
        ui = GameObject.Find("UIController").GetComponent<UIController>();
        game = GameObject.Find("GameplayController").GetComponent<GameplayController>();
        isDead = false;
        aud = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col) {
        //Only register projectile collisions
        if (col.gameObject.tag == "Projectile" || col.gameObject.tag == "Projectile-Double") {

            Destroy(col.gameObject);
            ReduceHealth(col.gameObject.GetComponent<Rocket>().RocketDamage);
        }


        
    }


    public void ReduceHealth(int dmg) {

        if (health - dmg <= 0 && !isDead) {
            
            RemoveEnemy();

            return;
        }

        health -= dmg;
        //Set the visual health bar
        float newHealth = (float)health / (float)OriginalHealth;
        HealthBar.value = newHealth;
    }

    public void RemoveEnemy() {
        ui.AddMoney(worth);
        isDead = true;

        if (transform.GetChild(1).gameObject.tag == "Explosive") {
                StartCoroutine(Explode());
                return;
        }
        
        game.EnemiesLeft--;
        Destroy(transform.gameObject);
        
    }


    IEnumerator Explode() {
        transform.GetChild(1).gameObject.GetComponent<Animator>().Play("Explosion");
        yield return new WaitForSeconds(0.5f);
        game.EnemiesLeft--;
        aud.Play("Explosion");
        Destroy(transform.gameObject);
    }

}
