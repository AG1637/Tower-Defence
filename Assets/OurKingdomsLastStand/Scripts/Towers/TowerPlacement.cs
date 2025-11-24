using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;
using System.Collections;

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

    private bool archer;
    private bool magic;
    private bool cannon;

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
                if (archer == true)
                {
                    GameManager.instance.coinsRemaining += archerTowerCost;
                    archer = false;
                }
                else if (magic == true)
                {
                    GameManager.instance.coinsRemaining += magicTowerCost;
                    magic = false;
                }
                else if (cannon == true)
                {
                    GameManager.instance.coinsRemaining += cannonTowerCost;
                    cannon = false;
                }
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
                        archer = false;
                        magic = false;
                        cannon = false;
                    }
                    else
                    {
                        //Add text on screen that tells player that they cannot place a tower in that position
                        Debug.Log("Cannot Place Tower Here");
                        showText = true;
                        if (showText == true)
                        {
                            cannotPlaceTowerText.SetActive(true);
                            StartCoroutine(HideText());
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
                        StartCoroutine(HideText());
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

    IEnumerator HideText()
    {
        yield return new WaitForSeconds(2);
        showText = false;
        cannotPlaceTowerText.SetActive(false);
        cannotAffordTowerText.SetActive(false);
    }
    
    public void ArcherTowerCost(GameObject tower)
    {
        if(GameManager.instance.coinsRemaining >= archerTowerCost)
        {
            canAffordTower = true;
            GameManager.instance.coinsRemaining -= archerTowerCost;
            SetTowerToPlace(tower);
            archer = true;
        }
        else
        {
            cannotAffordTowerText.SetActive(true);
            StartCoroutine(HideText());
            canAffordTower = false;
            selectedTower = null;
        }
    }
    public void MagicTowerCost(GameObject tower)
    {
        if (GameManager.instance.coinsRemaining >= magicTowerCost)
        {
            canAffordTower = true;
            GameManager.instance.coinsRemaining -= magicTowerCost;
            SetTowerToPlace(tower);
            magic = true;
        }
        else
        {
            cannotAffordTowerText.SetActive(true);
            StartCoroutine(HideText());
            canAffordTower = false;
            selectedTower = null;
        }
    }

    public void CannonTowerCost(GameObject tower)
    {
        if (GameManager.instance.coinsRemaining >= cannonTowerCost)
        {
            canAffordTower = true;
            GameManager.instance.coinsRemaining -= cannonTowerCost;
            SetTowerToPlace(tower);
            cannon = true;
        }
        else
        {
            cannotAffordTowerText.SetActive(true);
            StartCoroutine(HideText());
            canAffordTower = false; 
            selectedTower = null;
        }
    }
}
