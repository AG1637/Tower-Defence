using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TowerUIManager : MonoBehaviour
{
    public static TowerUIManager instance;

    [Header("UI")]
    public GameObject panel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI sellValueText;
    public Button upgradeButton;
    public Button sellButton;

    TowerBehaviour selectedTower;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        if (panel != null) panel.SetActive(false);
    }

    void Update()
    {
        if (panel != null && panel.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) //ignore if clicked on UI
                return; 

            // Raycast from camera to check what was clicked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                var tower = hit.collider.GetComponentInParent<TowerBehaviour>();
                if (tower == null || tower != selectedTower)
                {                    
                    Hide();
                }
            }
            else
            {
                Hide();
            }
        }
    }

    public void ShowForTower(TowerBehaviour tower)
    {
        if (tower == null) return;
        selectedTower = tower;

        if (panel != null) panel.SetActive(true);

        RefreshUI();
    }

    void RefreshUI()
    {
        if (selectedTower == null) return;

        nameText.text = selectedTower.towerName;
        levelText.text = "Level: " + selectedTower.level;
        fireRateText.text = "Fire Rate: " + selectedTower.fireRate.ToString("F2");
        rangeText.text = "Range: " + selectedTower.GetRange().ToString("F1");
        damageText.text = "Damage: " + selectedTower.bulletDamage;
        upgradeCostText.text = "Upgrade: " + selectedTower.upgradeCost + " coins";
        sellValueText.text = "Sell: " + selectedTower.sellValue + " coins";

        // disable/enable upgrade button based on coins
        bool canAfford = GameManager.instance != null && GameManager.instance.coinsRemaining >= selectedTower.upgradeCost;
        upgradeButton.interactable = canAfford;
    }

    public void OnUpgradePressed()
    {
        if (selectedTower == null) return;
        if (GameManager.instance.coinsRemaining >= selectedTower.upgradeCost)
        {
            GameManager.instance.coinsRemaining -= selectedTower.upgradeCost;
            selectedTower.Upgrade();
            RefreshUI();
        }
        else
        {
            Debug.Log("Not enough coins to upgrade");
        }
    }

    public void OnSellPressed()
    {
        if (selectedTower == null) return;
        selectedTower.Sell();
        Hide();
    }

    public void Hide()
    {
        selectedTower = null;
        if (panel != null) panel.SetActive(false);
    }
}