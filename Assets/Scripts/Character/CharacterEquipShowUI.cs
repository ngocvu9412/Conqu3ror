using UnityEngine;


public class CharacterEquipShowUI : MonoBehaviour
{
    [Header ("Layout Settings")]
    [SerializeField] float itemSpace = 10;

    float itemWidth;

    [Header ("UI Elements")]
    [SerializeField] Transform EquipmentItemContainer;
    [SerializeField] GameObject ItemPrefab;    
    void Start ()
    {
        GenerateCharacterEquipUI();
    }
    void GenerateCharacterEquipUI ()
    {
        //Xóa item template
        itemWidth = ItemPrefab.GetComponent<RectTransform>().sizeDelta.x;

        //Tạo items
        for (int i=0; i < 4; i++)
        {
            CharacterEquipmentUI uiItem = Instantiate (ItemPrefab, EquipmentItemContainer).GetComponent <CharacterEquipmentUI> ();

            //Di chuyển item đến vị trí
            uiItem.SetEquipmentPos(Vector2.right*(itemWidth+itemSpace)*i);
        }
        SetUsedEquipInfo();
    }
    void SetUsedEquipInfo()
    {
        Equipment[] playerUsedEquips = GameDataManager.GetPlayerUsedEquips();
        for (int i=0; i < playerUsedEquips.Length; i++)
        {
            CharacterEquipmentUI uiItem=EquipmentItemContainer.GetChild(i+1).GetComponent<CharacterEquipmentUI>();

            //Thêm tên cho item trong Hierachy
            uiItem.gameObject.name = "Item - " + playerUsedEquips[i].name; 
            
            //Thêm thông tin
            uiItem.SetEquipmentImage (playerUsedEquips[i].GetSprite());
            uiItem.SetLevelText (playerUsedEquips[i].Level);
        }
    }
}
