using System.Collections;
using System.Collections.Generic;
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
    // [SerializeField] 
    // private GameObject itemsWindow;
    // [SerializeField] 
    // private Button itemsButton;


    void Start()
    {
        // Ensure that the shop window is hidden at the start
        shopWindow.SetActive(false);

        // Ensure the equipment window is hidden at the start
        equipmentWindow.SetActive(false);

        // // Ensure the items window is hidden at the start
        // itemsWindow.SetActive(false);
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
            ShopButton.interactable=false;
            CloseButton.interactable=true;
            // Always open the equipment Window when the Shop is opened, and close the Items Window
            OpenEquipmentWindow();
        }

        // Toggle the active state of the shop window
        shopWindow.SetActive(!isActive);
    }

    // This method will be called when the equipment button is clicked
    public void ToggleEquipmentWindow()
    {
        // // Close the Items Window if it's open
        // if (itemsWindow != null && itemsWindow.activeSelf)
        // {
        //     itemsWindow.SetActive(false);
        //     Debug.Log("Closing Items Window.");
        // }

        // Toggle the equipment Window visibility
        if (equipmentWindow != null && !equipmentWindow.activeSelf)
        {
            equipmentWindow.SetActive(true);
            Debug.Log("Opening equipment Window.");
        }
    }

    // This method will be called when the Items button is clicked
    // public void ToggleItemsWindow()
    // {
    //     // Close the equipment Window if it's open
    //     if (equipmentWindow != null && equipmentWindow.activeSelf)
    //     {
    //         equipmentWindow.SetActive(false);
    //         Debug.Log("Closing equipment Window.");
    //     }

    //     // Toggle the Items Window visibility
    //     if (itemsWindow != null && !itemsWindow.activeSelf)
    //     {
    //         itemsWindow.SetActive(true);
    //         Debug.Log("Opening Items Window.");
    //     }
    // }

    // Always open the equipment Window when the Shop is opened
    private void OpenEquipmentWindow()
    {
        // Make sure the equipment Window is open, and close the Items Window if it's open
        if (equipmentWindow != null && !equipmentWindow.activeSelf)
        {
            equipmentWindow.SetActive(true);
            Debug.Log("Opening equipment Window.");
        }

        // // Close the Items Window if it's open
        // if (itemsWindow != null && itemsWindow.activeSelf)
        // {
        //     itemsWindow.SetActive(false);
        //     Debug.Log("Closing Items Window.");
        // }
    }
}
