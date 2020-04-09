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
    //UI Elements
    public Slider HealthBar;
    public Text MoneyText;
    public Button PurchaseButton;
    public Button StartButton;
    public GameObject PurchasePanel;
    public Text SelectTowerText;
    public Dropdown SelectTower;
    public Text CurrentLevel;
    public Text GameOverText;
    public GameObject TowersFull;

    // Non-UI Elements 
    public TowerController Towers;
    public GameplayController game;

    //Private fields
    private bool PurchaseOpen;
    private bool PlacementMode;
    private string PlacementType;

    private PersistenceController contr;
    

    //Constants/Reference data
    private const int TOP_SPOTS = 15;
    private const int BOTTOM_SPOTS = 22;
    private Dictionary<string, int> prices = new Dictionary<string, int> 
    {
        {"BasicRocket", 100},
        {"AdvancedRocket", 250},
        {"BigRocket", 400},
        {"BasicCannon", 700},
        {"AdvancedCannon", 900}


    };

    // Start is called before the first frame update
    void Start()
    {

        PurchasePanel.SetActive(false);
        PurchaseOpen = false;
        PlacementMode = false;
        SelectTowerText.gameObject.SetActive(false);
       
       //Persistent data
        contr = PersistenceController.Instance;
        MoneyText.text = "$" + contr.money;
        CurrentLevel.text = contr.level.ToString();
        HealthBar.value = contr.health;

        if(contr.GameOver) {
            GameOverText.gameObject.SetActive(true);
            
        }
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
        if( (contr.BuyPhase || contr.GameOver) && !PurchaseOpen ) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void BuyingItemPressed(Button button)
    {
        int price = 0;
        prices.TryGetValue(button.name, out price);

        if (price > contr.money) {
            //If user doesnt have enough contr.money, ignore request
            return;
        
        }

        PlacementMode = true;
        PurchaseOpen = false;
        PurchasePanel.SetActive(false); //Just to hide

        if (contr.TopTurretsPlaced == TOP_SPOTS && SelectTowerText.transform.GetChild(0).gameObject.GetComponent<Dropdown>().options.Count == 2)
        {

            //Remove option of selecting top spots
            SelectTowerText.transform.GetChild(0).gameObject.GetComponent<Dropdown>().options.RemoveAt(0);
            SelectTowerText.transform.GetChild(0).gameObject.GetComponent<Dropdown>().value = 1;

        }

        else if (contr.BottomTurretsPlaced == BOTTOM_SPOTS && SelectTowerText.transform.GetChild(0).gameObject.GetComponent<Dropdown>().options.Count == 2)
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

        if (PurchaseOpen || PlacementMode || !(contr.BuyPhase))
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
        if (PurchaseOpen || PlacementMode || !(contr.BuyPhase))
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

        if (choice == 0 && contr.TopTurretsPlaced != TOP_SPOTS)
        {
            contr.TopTurretsPlaced++;
            Towers.PlaceTower(PlacementType,0);
        }

        else
        {
            contr.BottomTurretsPlaced++;
            Towers.PlaceTower(PlacementType,1);
        }

        if (contr.BottomTurretsPlaced == BOTTOM_SPOTS && contr.TopTurretsPlaced == TOP_SPOTS)
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
        contr.money += val;

        MoneyText.text = "$" + contr.money;
    }


    public void RemoveMoney(int val) {
        contr.money -= val;
        MoneyText.text = "$" + contr.money;

    }

    public void EnterBuyPhase() {
        StartButton.gameObject.SetActive(true);

        //If there are no more open turret spots, do not show the purchase button
        if (contr.TopTurretsPlaced != TOP_SPOTS && contr.BottomTurretsPlaced != BOTTOM_SPOTS) { 
            PurchaseButton.gameObject.SetActive(true);
            
        }


    }

    public void NextLevel() {
        contr.level++;
        CurrentLevel.text = contr.level.ToString();
    }




}




