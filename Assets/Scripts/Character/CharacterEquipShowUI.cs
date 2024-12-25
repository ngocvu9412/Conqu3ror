using UnityEngine;


public class CharacterEquipShowUI : MonoBehaviour
{
    [Header ("Layout Settings")]
    [SerializeField] float itemSpace = 10;

    float itemWidth;

    [Header ("UI Elements")]
    [SerializeField] Transform CharacterEquipmentContainer;
    [SerializeField] GameObject UsedEquipPrefab;    
    void Start ()
    {
        GenerateCharacterEquipUI();
    }
    void GenerateCharacterEquipUI ()
    {
        //Xóa item template
        itemWidth = UsedEquipPrefab.GetComponent<RectTransform>().sizeDelta.x;

        //Tạo items
        for (int i=0; i < 4; i++)
        {
            CharacterEquipmentUI uiItem = Instantiate (UsedEquipPrefab, CharacterEquipmentContainer).GetComponent <CharacterEquipmentUI> ();

            //Di chuyển item đến vị trí
            uiItem.SetEquipmentPos(Vector2.right*(itemWidth+itemSpace)*i);
        }
        SetUsedEquipInfo();
    }
    public void SetUsedEquipInfo()
    {
        if(CharacterEquipmentContainer.childCount > 1)
        {
            Equipment[] playerUsedEquips = GameDataManager.Ins.GetPlayerUsedEquips();
            for (int i=0; i < playerUsedEquips.Length; i++)
            {
                if(playerUsedEquips[i].Equals(default(Equipment))) continue;
                CharacterEquipmentUI uiItem=CharacterEquipmentContainer.GetChild(i+1).GetComponent<CharacterEquipmentUI>();
    
                //Thêm tên cho item trong Hierachy
                uiItem.gameObject.name = "Item - " + playerUsedEquips[i].name; 
                
                //Thêm thông tin
                uiItem.SetEquipmentImage (playerUsedEquips[i].GetSprite());
                uiItem.SetLevelText (playerUsedEquips[i].Level);
            }
        }
    }
}
