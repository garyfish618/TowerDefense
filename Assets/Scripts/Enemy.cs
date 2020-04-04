using System.Collections;
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
    public int worth;
    

    private int OriginalHealth;
    private UIController ui;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col) {

        if (col.gameObject.tag == "Projectile") {

            Destroy(col.gameObject);

            ReduceHealth(col.gameObject.GetComponent<Rocket>().RocketDamage);
        }


        
    }


    public void ReduceHealth(int dmg) {

        if (health - dmg <= 0 && !isDead) {
            ui.AddMoney(worth);
            isDead = true;

            if (transform.GetChild(1).gameObject.tag == "Explosive") {
                StartCoroutine(Explode());
                return;

            }
            game.EnemiesLeft--;
            Destroy(transform.gameObject);
        }

        health -= dmg;
        //Set the visual health bar
        float newHealth = (float)health / (float)OriginalHealth;
        HealthBar.value = newHealth;
    }


    IEnumerator Explode() {
        transform.GetChild(1).gameObject.GetComponent<Animator>().Play("Explosion");
        yield return new WaitForSeconds(0.5f);
        game.EnemiesLeft--;
        Destroy(transform.gameObject);

    }

}
