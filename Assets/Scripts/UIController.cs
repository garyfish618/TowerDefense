using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider HealthBar;
    public Text MoneyText;
    public Button PurchaseButton;
    public Button StartButton;
    public Button Cancel;
    public GameObject PurchasePanel;
    public TowerController Towers;
    public Text SelectTowerText;
    public Dropdown SelectTower;
    public int Money;
    public GameplayController game;
    public int level;
    public Text CurrentLevel;

    private bool PurchaseOpen;
    private bool PlacementMode;
    private string PlacementType;

    private Dictionary<string, int> prices;

    // Start is called before the first frame update
    void Start()
    {
        //Price table for towers
        prices = new Dictionary<string, int>();
        prices.Add("BasicRocket", 100);
        prices.Add("AdvancedRocket", 250);
        prices.Add("BigRocket", 400);
        prices.Add("BasicCannon", 700);
        prices.Add("AdvancedCannon", 900);

        PurchasePanel.SetActive(false);
        PurchaseOpen = false;
        PlacementMode = false;
        SelectTowerText.gameObject.SetActive(false);

        MoneyText.text = "$" + Money;
        CurrentLevel.text = level.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void BuyingItemPressed(Button button)
    {
        int price = 0;
        prices.TryGetValue(button.name, out price);

        if (price > Money) {
            //If user doesnt have enough money, ignore request
            return;
        
        }

        PlacementMode = true;
        PurchaseOpen = false;
        PurchasePanel.SetActive(false); //Just to hide
        SelectTowerText.gameObject.SetActive(true);
        PlacementType = button.name;
    }

    public void PurchasePressed()
    {
        if (PurchaseOpen || PlacementMode || !(game.BuyPhase))
        {
            // If window is already open, we are trying to place a tower, or we are not in buy phase, just ignore request
            return;
        }

        else
        {
            PurchasePanel.SetActive(true);
            PurchaseOpen = true;
        }
    
    }

    public void StartPressed() {
        if (PurchaseOpen || PlacementMode || !(game.BuyPhase))
        {
            // If window is already open, we are trying to place a tower, or we are not in buy phase, just ignore request
            return;
        }

        else {
            StartButton.gameObject.SetActive(false);
            PurchaseButton.gameObject.SetActive(false);
            game.StartGame();
        
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
        int price = 0;
        prices.TryGetValue(PlacementType, out price);
        RemoveMoney(price);
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


    public void AddMoney(int val) {
        Money += val;

        MoneyText.text = "$" + Money;
    }


    public void RemoveMoney(int val) {
        Money -= val;
        MoneyText.text = "$" + Money;

    }

    public void EnterBuyPhase() {
        StartButton.gameObject.SetActive(true);
        PurchaseButton.gameObject.SetActive(true);

    }

    public void NextLevel() {
        level++;
        CurrentLevel.text = level.ToString();
    }




}




