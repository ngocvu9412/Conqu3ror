using UnityEngine;

public class CharacterShopUI : MonoBehaviour
{
    [Header ("Layout Settings")]
    [SerializeField] float itemSpace = 10;

    float itemWidth;

    [Header ("UI Elements")]
    [SerializeField] Transform CharacterItemContainer;
    [SerializeField] GameObject ItemPrefab;
    [Space(20)]
    [SerializeField] CharactersDatabase CharacterDatabase;
    
    void Start ()
    {
        GenerateShopItemUI();
    }
    void GenerateShopItemUI ()
    {
        //Xóa item template
        itemWidth = ItemPrefab.GetComponent<RectTransform>().sizeDelta.x;

        //Tạo items
        for (int i=0; i < CharacterDatabase.CharactersCount; i++)
        {
            Character Character = CharacterDatabase.GetCharacter(i);
            CharacterItemUI uiItem = Instantiate (ItemPrefab, CharacterItemContainer).GetComponent <CharacterItemUI> ();

            //Di chuyển item đến vị trí
            Debug.Log(uiItem.GetComponent<RectTransform>().anchoredPosition);
            uiItem.SetItemPos(Vector2.right*(itemWidth+itemSpace)*i);

            //Thêm tên cho item trong Hierachy
            uiItem.gameObject.name = "Item - " + Character.name; 
            
            //Thêm thông tin
            uiItem.SetCharacterImage (Character.Image);
            uiItem.SetCharacterPrice (Character.Price);

            //Xử lý mua hàng
            if(Character.Unlocked == true) 
            {
                uiItem.SetCharacterAsUnlocked();
            }
            else 
            {
                uiItem.OnItemPurchase (i, OnItemPurchased);
            }
            CharacterItemContainer.GetComponent<RectTransform> ().sizeDelta = Vector2.right*(itemWidth+itemSpace)*CharacterDatabase.CharactersCount;
        }
    }
    CharacterItemUI GetItemUI (int index)
	{
		return CharacterItemContainer.GetChild (index).GetComponent <CharacterItemUI> ();
	}
    void OnItemPurchased (int index)
	{
        Character character = CharacterDatabase.GetCharacter (index);
        CharacterItemUI uiItem = GetItemUI(index);
        if (GameDataManager.Ins.CanSpendCoins( character.Price))
        {
            GameDataManager.Ins.SpendCoins( character.Price);
            GameDataManager.Ins.AddUnlockedCharacter( CharacterDatabase.GetIndex(character));
            character.Unlocked = true;
            uiItem.SetCharacterAsUnlocked();
            GoldSharedUI.Ins.UpdateCoinsUIText();
        }
	}
}
