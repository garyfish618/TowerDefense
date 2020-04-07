using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.CodeDom;

public class UIController : MonoBehaviour
{
    public Slider HealthBar;
    public Text MoneyText;
    public Button PurchaseButton;
    public Button StartButton;
    public GameObject PurchasePanel;
    public TowerController Towers;
    public Text SelectTowerText;
    public Dropdown SelectTower;
    public int Money;
    public GameplayController game;
    public int level;
    public Text CurrentLevel;
    public Text GameOverText;
    public GameObject TowersFull;


    private bool PurchaseOpen;
    private bool PlacementMode;
    private string PlacementType;
    private int TopTurretsPlaced;
    private int BottomTurretsPlaced;

    private const int TOP_SPOTS = 15;
    private const int BOTTOM_SPOTS = 22;

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


    public void GameOver() 
    {
        GameOverText.gameObject.SetActive(true);
    
    
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");

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

        if (TopTurretsPlaced == TOP_SPOTS && SelectTowerText.transform.GetChild(0).gameObject.GetComponent<Dropdown>().options.Count == 2)
        {

            //Remove option of selecting top spots
            SelectTowerText.transform.GetChild(0).gameObject.GetComponent<Dropdown>().options.RemoveAt(0);
            SelectTowerText.transform.GetChild(0).gameObject.GetComponent<Dropdown>().value = 1;

        }

        else if (BottomTurretsPlaced == BOTTOM_SPOTS && SelectTowerText.transform.GetChild(0).gameObject.GetComponent<Dropdown>().options.Count == 2)
        {
            //Remove option of selecting bottom spots
            SelectTowerText.transform.GetChild(0).gameObject.GetComponent<Dropdown>().options.RemoveAt(1);
            SelectTowerText.transform.GetChild(0).gameObject.GetComponent<Dropdown>().value = 0;
        }

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
            TowersFull.SetActive(false);
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

        if (choice == 0 && TopTurretsPlaced != TOP_SPOTS)
        {
            TopTurretsPlaced++;
            Towers.PlaceTower(PlacementType,0);
        }

        else
        {
            BottomTurretsPlaced++;
            Towers.PlaceTower(PlacementType,1);
        }

        if (BottomTurretsPlaced == BOTTOM_SPOTS && TopTurretsPlaced == TOP_SPOTS)
        {
            PurchaseButton.gameObject.SetActive(false);
            TowersFull.SetActive(true);
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

        //If there are no more open turret spots, do not show the purchase button
        if (TopTurretsPlaced != TOP_SPOTS && BottomTurretsPlaced != BOTTOM_SPOTS) { 
            PurchaseButton.gameObject.SetActive(true);
            
        }


    }

    public void NextLevel() {
        level++;
        CurrentLevel.text = level.ToString();
    }




}




