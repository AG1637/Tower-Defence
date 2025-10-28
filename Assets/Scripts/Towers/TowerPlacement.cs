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

    private GameObject SelectedTower;
    public float textTimer = 2;
    public GameObject CannotPlaceTowerText;
    private bool showText = false;

    private void Start()
    {
        CannotPlaceTowerText.SetActive(false);
    }

    void Update()
    {
        if (GameLoopManager.Instance.Paused)
        {
            return;
        }
        if(SelectedTower != null)
        {
            Ray camray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit HitInfo;
            if (Physics.Raycast(camray, out HitInfo, 300f, PlacementCollideMask))
            {
                SelectedTower.transform.position = HitInfo.point;
            }

            if (Input.GetMouseButtonDown(0) && HitInfo.collider.gameObject != null)
            {
                //if ((!HitInfo.collider.gameObject.CompareTag("CannotPlaceTowers") && !HitInfo.collider.gameObject.CompareTag("Path")) &&
                if (HitInfo.collider.gameObject.CompareTag("CanPlaceTowers"))
                {
                    BoxCollider TowerCollider = SelectedTower.gameObject.GetComponent<BoxCollider>();
                    TowerCollider.isTrigger = true;
                    //BoxCollider PathCollider = Path.gameObject.GetComponentInChildren<BoxCollider>();
                    //PathCollider.isTrigger = true;

                    Vector3 BoxCenter = SelectedTower.gameObject.transform.position + TowerCollider.center;
                    Vector3 HalfExtents = TowerCollider.size / 2;
                    if (!Physics.CheckBox(BoxCenter, HalfExtents, Quaternion.identity, PlacementCheckMask, QueryTriggerInteraction.Ignore))
                    {
                        TowerCollider.isTrigger = false;
                        //PathCollider.isTrigger = false;
                        SelectedTower = null;
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
        if (SelectedTower != null) //maybe not needed
        {
            Destroy(SelectedTower);
        }
        SelectedTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
    }

    public void HideTowerText()
    {
        showText = false;
        CannotPlaceTowerText.SetActive(false);
    }       
}
