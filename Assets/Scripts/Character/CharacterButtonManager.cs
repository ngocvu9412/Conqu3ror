using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterButtonManager : MonoBehaviour
{
    [Header ("Character Buttons")]
    [SerializeField] 
    private Button CloseButton;
    [SerializeField] 
    private GameObject CharacterWindow;
    [SerializeField] 
    private Button CharacterButton;
    [SerializeField] GameObject CharacterEquipShow;

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
            CharacterEquipShow.GetComponent<CharacterEquipShowUI>().SetUsedEquipInfo();
        }

        CharacterWindow.SetActive(!isActive);
        if(isActive)
        {
            CharacterWindow.GetComponent<RectTransform>().localScale = Vector3.zero; // Đặt kích thước ban đầu là 0
            CharacterWindow.GetComponent<RectTransform>().DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).SetUpdate(true); // Tăng kích thước lên 1 với hiệu ứng mượt

            Time.timeScale = 0;
        }
    }
}
