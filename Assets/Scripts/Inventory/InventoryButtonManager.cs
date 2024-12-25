using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class InventoryButtonManager : MonoBehaviour
{
    [Header ("Inventory Buttons")]
    [SerializeField] private Button CloseButton;
    [SerializeField] private GameObject InventoryWindow;
    [SerializeField] private Button InventoryButton;
    [SerializeField] GameObject InventoryEquipShow;


    void Start()
    {
        // Ensure that the Inventory window is hidden at the start
        InventoryWindow.SetActive(false);
    }

    // This method will be called when the button is clicked to toggle the shop window
    public void ToggleInventoryWindow()
    {
        bool isActive = InventoryWindow.activeSelf;

        // Toggle the Inventory window visibility
        if (isActive)
        {
            InventoryButton.interactable=true;
            CloseButton.interactable=false;
        }
        else
        {
            InventoryButton.interactable=false;
            CloseButton.interactable=true;
            InventoryEquipShow.GetComponent<EquipmentShowUI>().SetSlotEquipmentInfo();
        }

        // Toggle the active state of the shop window
        InventoryWindow.SetActive(!isActive);
        if(isActive)
        {
            InventoryWindow.GetComponent<RectTransform>().localScale = Vector3.zero; // Đặt kích thước ban đầu là 0
            InventoryWindow.GetComponent<RectTransform>().DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetUpdate(true); // Tăng kích thước lên 1 với hiệu ứng mượt

            Time.timeScale = 0;
        }
    }
}
