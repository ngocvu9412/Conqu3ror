using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterButtonManager : MonoBehaviour
{
    [Header ("Character Buttons")]
    [SerializeField] 
    private Button CloseButton;
    [SerializeField] 
    private GameObject CharacterWindow;
    [SerializeField] 
    private Button CharacterButton;

    void Start()
    {
        // Đảm bảo tắt cửa sổ Character lúc mới vào
        CharacterWindow.SetActive(false);
    }

    // Hàm gọi mở cửa sổ Character
    public void ToggleCharacterWindow()
    {
        bool isActive = CharacterWindow.activeSelf;

        // Toggle the Character window visibility
        if (isActive)
        {
            CharacterButton.interactable=true;
            CloseButton.interactable=false;
        }
        else
        {
            CharacterButton.interactable=false;
            CloseButton.interactable=true;
        }

        CharacterWindow.SetActive(!isActive);
    }
}
