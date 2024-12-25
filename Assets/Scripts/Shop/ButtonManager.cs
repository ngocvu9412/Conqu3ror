using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [Header ("Shop Buttons")]
    [SerializeField] 
    private Button CloseButton;
    [SerializeField] 
    private GameObject shopWindow;
    [SerializeField] 
    private Button ShopButton;
    [SerializeField] 
    private GameObject equipmentWindow;
    [SerializeField] 
    private Button equipmentButton;
    [SerializeField] GameObject MergeEquipShow;
    [SerializeField] 
    private GameObject CharactersWindow;
    [SerializeField] 
    private Button CharactersButton;


    void Start()
    {
        // Ensure that the shop window is hidden at the start
        shopWindow.SetActive(false);

        // Ensure the equipment window is hidden at the start
        equipmentWindow.SetActive(false);
        MergeEquipShow.SetActive(false);

        // Ensure the items window is hidden at the start
        CharactersWindow.SetActive(false);
    }

    // This method will be called when the button is clicked to toggle the shop window
    public void ToggleShopWindow()
    {
        bool isActive = shopWindow.activeSelf;

        // Toggle the shop window visibility
        if (isActive)
        {
            ShopButton.interactable=true;
            CloseButton.interactable=false;
        }
        else
        {
            MergeEquipShow.GetComponent<MergeEquipmentShowUI>().ResetMergeEquipUI();
            ShopButton.interactable=false;
            CloseButton.interactable=true;
            // Always open the equipment Window when the Shop is opened, and close the Characters Window
            OpenEquipmentWindow();
        }

        // Toggle the active state of the shop window
        shopWindow.SetActive(!isActive);
    }

    // This method will be called when the equipment button is clicked
    public void ToggleEquipmentWindow()
    {
        // // Close the Characters Window if it's open
        if (CharactersWindow != null && CharactersWindow.activeSelf)
        {
            CharactersWindow.SetActive(false);
            CharactersButton.GetComponentInChildren<Text>().color = Color.white;
            Debug.Log("Closing Characters Window.");
        }

        // Toggle the equipment Window visibility
        if (equipmentWindow != null && !equipmentWindow.activeSelf)
        {
            equipmentWindow.SetActive(true);
            MergeEquipShow.SetActive(true);
            equipmentButton.GetComponentInChildren<Text>().color = Color.yellow;
            Debug.Log("Opening equipment Window.");
        }
    }

    // This method will be called when the Characters button is clicked
    public void ToggleCharactersWindow()
    {
        // Close the equipment Window if it's open
        if (equipmentWindow != null && equipmentWindow.activeSelf)
        {
            equipmentButton.GetComponentInChildren<Text>().color = Color.white;
            equipmentWindow.SetActive(false);
            MergeEquipShow.SetActive(false);
            Debug.Log("Closing equipment Window.");
        }

        // Toggle the Characters Window visibility
        if (CharactersWindow != null && !CharactersWindow.activeSelf)
        {
            CharactersButton.GetComponentInChildren<Text>().color = Color.yellow;
            CharactersWindow.SetActive(true);
            Debug.Log("Opening Characters Window.");
        }
    }

    // Always open the equipment Window when the Shop is opened
    private void OpenEquipmentWindow()
    {
        // Make sure the equipment Window is open, and close the Characters Window if it's open
        if (equipmentWindow != null && !equipmentWindow.activeSelf)
        {
            equipmentWindow.SetActive(true);
            MergeEquipShow.SetActive(true);
            equipmentButton.GetComponentInChildren<Text>().color = Color.yellow;
            Debug.Log("Opening equipment Window.");
        }

        // Close the Characters Window if it's open
        if (CharactersWindow != null && CharactersWindow.activeSelf)
        {
            CharactersWindow.SetActive(false);
            CharactersButton.GetComponentInChildren<Text>().color = Color.white;
            Debug.Log("Closing Characters Window.");
        }
    }
}
