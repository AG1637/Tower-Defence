using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private LayerMask PlacementCheckMask;
    [SerializeField] private LayerMask PlacementCollideMask;
    [SerializeField] private Camera PlayerCamera;

    private GameObject selectedTower;
    public GameObject cannotPlaceTowerText;
    public GameObject cannotAffordTowerText;
    public TextMeshProUGUI archerCostText;
    public TextMeshProUGUI magicCostText;
    public TextMeshProUGUI cannonCostText;
    private bool showText = false;
    private bool canAffordTower;
    public float textTimer = 2;

    [Header("Tower Costs")]
    public int archerTowerCost = 100;
    public int magicTowerCost = 150;
    public int cannonTowerCost = 200;

    private void Start()
    {
        cannotPlaceTowerText.SetActive(false);
        cannotAffordTowerText.SetActive(false);
        archerCostText.text = archerTowerCost.ToString();
        magicCostText.text = magicTowerCost.ToString(); 
        cannonCostText.text = cannonTowerCost.ToString();
    }

    void Update()
    {
        if (GameManager.instance.paused) //reference to GameManager and checks if paused - if paused, do not allow tower placement
        {
            return;
        }
        if (canAffordTower == false)
        {
            return;
        }
        if (canAffordTower == true)
        {
            if(selectedTower != null)
            {
            Ray camray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit HitInfo;
            if (Physics.Raycast(camray, out HitInfo, 300f, PlacementCollideMask))
            {
                selectedTower.transform.position = HitInfo.point;//moves tower to mouse position
            }

            if(Input.GetKeyDown(KeyCode.Q))//allows player to cancel tower placement by pressing Q
            {
                Destroy(selectedTower);
                selectedTower = null;
                return;
            }

            if (Input.GetMouseButtonDown(0) && HitInfo.collider.gameObject != null)
            {
                if (HitInfo.collider.gameObject.CompareTag("CanPlaceTowers"))//checks if tower is being placed on valid ground
                {
                    BoxCollider TowerCollider = selectedTower.gameObject.GetComponent<BoxCollider>();
                    TowerCollider.isTrigger = true;
                    Vector3 BoxCenter = selectedTower.gameObject.transform.position + TowerCollider.center;
                    Vector3 HalfExtents = TowerCollider.size / 2;
                    if (!Physics.CheckBox(BoxCenter, HalfExtents, Quaternion.identity, PlacementCheckMask, QueryTriggerInteraction.Ignore))
                    {
                        //Debug.Log("Tower Placed");
                        GameManager.instance.towersPlaced += 1;
                        TowerCollider.isTrigger = false;
                        selectedTower.GetComponent<TowerBehaviour>().canShoot = true;
                        selectedTower = null;
                    }
                    else
                    {
                        //Add text on screen that tells player that they cannot place a tower in that position
                        Debug.Log("Cannot Place Tower Here");
                        showText = true;
                        if (showText == true)
                        {
                            cannotPlaceTowerText.SetActive(true);
                            Invoke("HideTowerText", 2f);
                        }
                    }                
                }
                else 
                {
                    Debug.Log("Cannot Place Tower Here");
                    showText = true;
                    if (showText == true)
                    {
                        cannotPlaceTowerText.SetActive(true);
                        Invoke("HideText", 2f);
                    }
                }
            }
            }
        }
        
    }

    public void SetTowerToPlace(GameObject tower)
    {
        selectedTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
    }

    public void HideText()
    {
        showText = false;
        cannotPlaceTowerText.SetActive(false);
        cannotAffordTowerText.SetActive(false);
    }
    
    public void ArcherTowerCost()
    {
        if(GameManager.instance.coinsRemaining >= archerTowerCost)
        {
            canAffordTower = true;
            GameManager.instance.coinsRemaining -= archerTowerCost;
        }
        else
        {
            cannotAffordTowerText.SetActive(true);
            Invoke("HideText", 2f);
            canAffordTower = false;
            selectedTower = null;
        }
    }
    public void MagicTowerCost()
    {
        if (GameManager.instance.coinsRemaining >= magicTowerCost)
        {
            canAffordTower = true;
            GameManager.instance.coinsRemaining -= magicTowerCost;
        }
        else
        {
            cannotAffordTowerText.SetActive(true);
            Invoke("HideText", 2f);
            canAffordTower = false;
            selectedTower = null;
        }
    }

    public void CannonTowerCost()
    {
        if (GameManager.instance.coinsRemaining >= cannonTowerCost)
        {
            canAffordTower = true;
            GameManager.instance.coinsRemaining -= cannonTowerCost;
        }
        else
        {
            cannotAffordTowerText.SetActive(true);
            Invoke("HideText", 2f);
            canAffordTower = false; 
            selectedTower = null;
        }
    }
}
