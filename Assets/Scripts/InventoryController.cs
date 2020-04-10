using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameplayController game;
    public UIController ui;

    private PersistenceController contr;
    

    // Start is called before the first frame update
    void Start()
    {
        contr = PersistenceController.Instance;
    }

    public void AddItem(string name) {

        if(contr.Inventory.ContainsKey(name)) {
            contr.Inventory[name] += 1;
            ui.UpdateItem(name);
        }     
    }

    public void RemoveItem(string name) {
         
        if(contr.Inventory.ContainsKey(name) && contr.Inventory[name] != 0) {
            contr.Inventory[name] -= 1;
            ui.UpdateItem(name);
        } 

    }

    public void UseItem(string name)  {
        float healthIncr = 0;

        if(!contr.Inventory.ContainsKey(name) || contr.Inventory[name] == 0 ) {
            return;
        }

        if(name == "DestroyEnemies") {
            game.DestroyAllEnemies();
            RemoveItem(name);
            return;
        }

        if(name == "TownFull") {
            healthIncr = 1.0f;
        }

        if(name == "TownHalf") {
            healthIncr = 0.5f;
        }

        if(name == "TownQuarter") {
            healthIncr = 0.25f;
        }

        if(healthIncr + contr.health >= 1.0f) {
            ui.SetHealth(1.0f);
        }

        else {
            ui.SetHealth(contr.health + healthIncr);
        }

        RemoveItem(name);
    }

}
