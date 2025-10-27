using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup Menu;

    public void OpenClosePanel()
    {
        Menu.alpha = Menu.alpha > 0 ? 0 : 1;
    }
}
