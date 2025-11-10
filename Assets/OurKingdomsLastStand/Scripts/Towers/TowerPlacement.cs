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
    public float textTimer = 2;
    public GameObject CannotPlaceTowerText;
    private bool showText = false;


    private void Start()
    {
        CannotPlaceTowerText.SetActive(false);
    }

    void Update()
    {
        if (GameLoopManager.instance.paused) //reference to GameLoopManager and checks if paused - if paused, do not allow tower placement
        {
            return;
        }
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
                        Debug.Log("Tower Placed");
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
                            CannotPlaceTowerText.SetActive(true);
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
                        CannotPlaceTowerText.SetActive(true);
                        Invoke("HideTowerText", 2f);
                    }
                }
            }
        }
    }

    public void SetTowerToPlace(GameObject tower)
    {
        selectedTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
    }

    public void HideTowerText()
    {
        showText = false;
        CannotPlaceTowerText.SetActive(false);
    }       
}
