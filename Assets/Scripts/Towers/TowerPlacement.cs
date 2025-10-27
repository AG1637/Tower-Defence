using UnityEngine;

public class TowerPlacement : MonoBehaviour
{

    [SerializeField] private LayerMask PlacementCheckMask;
    [SerializeField] private LayerMask PlacementCollideMask;
    [SerializeField] private Camera PlayerCamera;

    private GameObject SelectedTower;

    void Update()
    {
        if(SelectedTower != null)
        {
            Ray camray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit HitInfo;
            if (Physics.Raycast(camray, out HitInfo, 100f, PlacementCollideMask))
            {
                SelectedTower.transform.position = HitInfo.point;
            }

            if (Input.GetMouseButtonDown(0) && HitInfo.collider.gameObject != null)
            {
                if (!HitInfo.collider.gameObject.CompareTag("CannotPlaceTowers"))
                {
                    BoxCollider TowerCollider = SelectedTower.gameObject.GetComponent<BoxCollider>();
                    TowerCollider.isTrigger = true;

                    Vector3 BoxCenter = SelectedTower.gameObject.transform.position + TowerCollider.center;
                    Vector3 HalfExtents = TowerCollider.size / 2;
                    if (!Physics.CheckBox(BoxCenter, HalfExtents, Quaternion.identity, PlacementCheckMask, QueryTriggerInteraction.Ignore))
                    {
                        TowerCollider.isTrigger = false;
                        SelectedTower = null;
                    }
                    else
                    {
                        //Add text on screen that tells player that they cannot place a tower in that position
                        Debug.Log("Cannot Place Tower Here");
                    }
                }

            }
        }
    }

    public void SetTowerToPlace(GameObject tower)
    {
        SelectedTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
    }
}
