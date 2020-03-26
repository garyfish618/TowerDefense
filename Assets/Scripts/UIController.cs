using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider HealthBar;
    public Text Money;
    public Button Purchase;
    public Button Cancel;
    public GameObject PurchasePanel;
    public TowerController Towers;
    public Text SelectTowerText;
    public Dropdown SelectTower;

    private bool PurchaseOpen;
    private bool PlacementMode;
    private string PlacementType;

    // Start is called before the first frame update
    void Start()
    {
        PurchasePanel.SetActive(false);
        PurchaseOpen = false;
        PlacementMode = false;
        SelectTowerText.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void BuyingItemPressed(Button button)
    {
        PlacementMode = true;
        PurchaseOpen = false;
        PurchasePanel.SetActive(false); //Just to hide
        SelectTowerText.gameObject.SetActive(true);
        PlacementType = button.name;
    }

    public void PurchasePressed()
    {
        if (PurchaseOpen || PlacementMode)
        {
            // If window is already open or we are trying to place a tower, just ignore request
            return;
        }

        else
        {
            //TODO: Add check to make sure level is currently not playing
            PurchasePanel.SetActive(true);
            PurchaseOpen = true;
        }
    
    }

    public void CancelPurchasePressed()
    {
        if (PurchaseOpen)
        {
            PurchasePanel.SetActive(false);
            PurchaseOpen = false;
        }
    }

    public void ConfirmPlacementPressed()
    {
       int choice = SelectTower.value;
       PlacementMode = false;
       SelectTowerText.gameObject.SetActive(false);

        if (choice == 0)
        {
            Towers.PlaceTower(PlacementType,0);
        }

        else
        {
            Towers.PlaceTower(PlacementType,1);
        }
        PlacementType = null;
        return;
    }

    public void CancelPlacementPressed()
    {
        PurchaseOpen = false;
        PlacementMode = false;
        SelectTowerText.gameObject.SetActive(false);
        PurchasePressed();
    }




}




