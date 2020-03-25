using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider HealthBar;
    public Text Money;
    public Button Purchase;
    public Button Cancel;
    public GameObject BasicTower;
    private bool PurchaseOpen;
    public GameObject PurchasePanel;
    public TowerController Towers;

    // Start is called before the first frame update
    void Start()
    {
        PurchasePanel.SetActive(false);
        PurchaseOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void BuyingItemPressed(Button button)
    {
        if (button.name == "BasicRocket") {
            Towers.PlaceTower("BasicRocket");
        }

        else if (button.name == "AdvancedRocket")
        {
            Debug.Log("b");
        }

        else if (button.name == "BigRocket")
        {
            Debug.Log("c");
        }

        else if (button.name == "BasicCannon")
        {
            Debug.Log("d");
        }

        else if (button.name == "AdvancedCannon")
        {
            Debug.Log("e");
        }
    }

    public void PurchasePressed()
    {
        if (PurchaseOpen)
        {
            // If window is already open just ignore request
            return;
        }

        else
        {
            //TODO: Add check to make sure level is currently not playing
            PurchasePanel.SetActive(true);
            PurchaseOpen = true;
        }
    
    }

    public void CancelPressed()
    {
        if (PurchaseOpen)
        {
            PurchasePanel.SetActive(false);
            PurchaseOpen = false;
        }
    }

}




