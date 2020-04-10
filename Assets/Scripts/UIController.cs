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

    public Text SoundBut;
    public Text CurrentLevel;
    public Text GameOverText;
    public Text DestroyItem;
    public Text TownFullItem;
    public Text TownHalfItem;
    public Text TownQuarterItem;
    public GameObject TowersFull;
    public GameObject Inventory;
    public GameObject RoundSummary;
    public InputField RoundText;

    // Non-UI Elements 
    public TowerController Towers;
    public GameplayController game;
    public InventoryController inv;

    //Private fields
    private bool PurchaseOpen;
    private bool PlacementMode;
    private bool RoundSummaryOpen;
    private string PlacementType;

    private Dictionary<string,int> awards;

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

        awards = new Dictionary<string, int>();

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

        foreach(KeyValuePair<string,int> item in contr.Inventory) {
            for(int i = 1; i <= item.Value; i++){
                UpdateItem(item.Key);
            }
        }

        ChangeSound(contr.SoundAudible);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GameOver() 
    {
        Inventory.SetActive(false);
        GameOverText.gameObject.SetActive(true);
    
    
    }

    public void InventoryPressed()
    {
        if(Inventory.activeSelf) {
            Inventory.SetActive(false);
            return;
        }

        Inventory.SetActive(true);
    }

    public void MainMenu()
    {
        if( (contr.BuyPhase || contr.GameOver) && !PurchaseOpen ) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ItemPressed(Button but) {
        inv.UseItem(but.name);
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

    public void CloseRoundPressed()
    {
        RoundSummary.SetActive(false);
        RoundSummaryOpen = false;
    }

    public void PurchasePressed()
    {

        if (RoundSummaryOpen || PurchaseOpen || PlacementMode || !(contr.BuyPhase))
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

    public void SoundPressed()
    {
       ChangeSound(!contr.SoundAudible);

    }

    public void ChangeSound(bool NewSoundState)
    {
        if(NewSoundState)
        {
            contr.SoundAudible = true;
            SoundBut.text = "Sound OFF";
            return;
        }

        contr.SoundAudible = false;
        SoundBut.text = "Sound ON";


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

        RoundSummaryOpen = true;
        RoundSummary.SetActive(true);

        string RoundSum = "";

        if(awards.ContainsKey("TownFull")) {
            RoundSum += "Full Town Repair: " + awards["TownFull"] + "x\n";
        }

        if(awards.ContainsKey("TownHalf")) {
            RoundSum += "Half Town Repair: " + awards["TownHalf"] + "x\n";
        }

        if(awards.ContainsKey("TownQuarter")) {
            RoundSum += "Quarter Town Repair: " + awards["TownQuarter"] + "x\n";
        }
        
        if(awards.ContainsKey("DestroyEnemies")) {
            RoundSum += "Destroy all enemies: " + awards["DestroyEnemies"] + "x\n";
        }

        RoundText.text = RoundSum;
        //Reset awards
        awards = new Dictionary<string,int>();
    }

    //Queue up the items received from active round to show at end
    public void ItemReceived(string name) {
        if(awards.ContainsKey(name)) {
            awards[name]++;
        }

        else {
            awards.Add(name, 1);
        }
    }

    public void SetHealth(float health) {

        if(health + contr.health > 1) {
            contr.health = 1.0f;
        }
        contr.health = health;
        HealthBar.value = contr.health;
    }

    public void NextLevel() {
        contr.level++;
        CurrentLevel.text = contr.level.ToString();
    }

    public void UpdateItem(string name) {
        
        if(name == "DestroyEnemies") {
            DestroyItem.text = "Destroy all Enemies - " + contr.Inventory["DestroyEnemies"] + "x"; 
        }

        if(name == "TownFull") {
            TownFullItem.text = "Town Repair (Full) - " + contr.Inventory["TownFull"] + "x";
        }

        if(name == "TownHalf") {
            TownHalfItem.text = "Town Repair (Half) - " + contr.Inventory["TownHalf"] + "x"; 
        }

        if(name == "TownQuarter") {
            TownQuarterItem.text = "Town Repair (Quarter) - " + contr.Inventory["TownQuarter"] + "x";
        }

    }




}




