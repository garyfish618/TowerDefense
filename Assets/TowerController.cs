using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerController : MonoBehaviour
{
    public GameObject BasicRocket;
    private GameObject PlacementMode;
    private Tilemap baseLayer;

    // Start is called before the first frame update
    void Start()
    {
        baseLayer = GameObject.Find("/Grid/BaseMap").GetComponent<Tilemap>();
    }

    void Update()
    {
        if ((PlacementMode != null) && Input.GetMouseButtonDown(0))
        {
            int towerSizeX = Mathf.FloorToInt(PlacementMode.GetComponent<Renderer>().bounds.size.x);
            int towerSizeY = Mathf.FloorToInt(PlacementMode.GetComponent<Renderer>().bounds.size.y);

            float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            float mouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

            Vector3Int center = new Vector3Int();
            Vector3Int right = new Vector3Int();
            Vector3Int left = new Vector3Int();
            Vector3Int down = new Vector3Int();
            Vector3Int up = new Vector3Int();

            int range = 0;

            center.Set(Mathf.FloorToInt(mouseX), Mathf.FloorToInt(mouseY), 0);
            right.Set(Mathf.FloorToInt(mouseX) + range, Mathf.FloorToInt(mouseY), 0);
            left.Set(Mathf.FloorToInt(mouseX) - range, Mathf.FloorToInt(mouseY), 0);
            down.Set(Mathf.FloorToInt(mouseX), Mathf.FloorToInt(mouseY) - range, 0);
            up.Set(Mathf.FloorToInt(mouseX), Mathf.FloorToInt(mouseY) + range, 0);

            Vector3Int[] positions = { center, right, left, down, up };

            for (int i = 0; i < positions.Length; i++)
            {
               Sprite baseSprite = baseLayer.GetSprite(positions[i]);

                if (baseSprite == null || baseSprite.ToString() == "towerDefense_tilesheet_93 (UnityEngine.Sprite)" || baseSprite.ToString() == "towerDefense_tilesheet_231 (UnityEngine.Sprite)")
                {
                    //TODO: Add message saying you cannot place a tower here
                    return;
                }
            }

            //Cant place on ground tiles
            //for (int i = 0; i < positions.Length; i++)
            //{
            //string baseSprite = baseLayer.GetSprite(positions[i]).ToString();

            //if (baseSprite == "towerDefense_tilesheet_167 (UnityEngine.Sprite)" || baseSprite == "towerDefense_tilesheet_93 (UnityEngine.Sprite)")
            //{
            //TODO: Add message saying you cannot place a tower here
            //return;
            //}
            //}
            Instantiate(BasicRocket, new Vector3(mouseX, mouseY, -3), Quaternion.identity);

        }
    }

    public void PlaceTower(string TowerName)
    {
        if (TowerName == "BasicRocket")
        {
            PlacementMode = BasicRocket;

        }
    }
}
