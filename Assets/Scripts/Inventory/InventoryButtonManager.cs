using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryButtonManager : MonoBehaviour
{
    [Header ("Inventory Buttons")]
    [SerializeField] 
    private Button CloseButton;
    [SerializeField] 
    private GameObject InventoryWindow;
    [SerializeField] 
    private Button InventoryButton;


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
        }

        // Toggle the active state of the shop window
        InventoryWindow.SetActive(!isActive);
    }
}
