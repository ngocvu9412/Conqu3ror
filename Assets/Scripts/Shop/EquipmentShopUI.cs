using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EquipmentShopUI : MonoBehaviour
{
    [Header ("Layout Settings")]
    [SerializeField] float itemSpace = 10;

    float itemHeight;
    public PlayerData playerdata;


    [Header ("UI Elements")]
    [SerializeField] Transform EquipmentItemContainer;
    [SerializeField] GameObject ItemPrefab;
    [Space(20)]
    [SerializeField] EquipmentShopDatabase EquipmentDatabase;
    [SerializeField] GameObject MergeShow;
    
    void Start ()
    {
        GenerateShopItemUI();
    }
    void GenerateShopItemUI ()
    {
        //Xóa item template
        itemHeight = EquipmentItemContainer.GetChild (0).GetComponent<RectTransform>().sizeDelta.y;
        Destroy(EquipmentItemContainer.GetChild (0).gameObject);
        EquipmentItemContainer.DetachChildren();

        //Tạo items
        for (int i=0; i < EquipmentDatabase.EquipmentsCount; i++)
        {
            Equipment equipment = EquipmentDatabase.GetEquipment(i);
            EquipmentItemUI uiItem = Instantiate (ItemPrefab, EquipmentItemContainer).GetComponent <EquipmentItemUI> ();

            //Di chuyển item đến vị trí
            Debug.Log(uiItem.GetComponent<RectTransform>().anchoredPosition);
            uiItem.SetItemPos(Vector2.down*(itemHeight+itemSpace)*i);

            //Thêm tên cho item trong Hierachy
            uiItem.gameObject.name = "Item - " + equipment.name; 
            
            //Thêm thông tin
            uiItem.SetEquipmentName (equipment.name);
            uiItem.SetEquipmentDes (equipment.description);
            uiItem.SetEquipmentImage (equipment.GetSprite());
            uiItem.SetEquipmentPrice (equipment.price);


            //Xử lý mua hàng
            uiItem.OnItemPurchase (equipment, OnItemPurchased);

            //Chỉnh kích cỡ item container
            EquipmentItemContainer.GetComponent<RectTransform> ().sizeDelta = Vector2.up*(itemHeight+itemSpace)*EquipmentDatabase.EquipmentsCount;
        }
    }
    void OnItemPurchased (Equipment equipment)
	{
        if(GameDataManager.Ins.GetPlayerEquipments().Count < 25)
        {
            if (GameDataManager.Ins.CanSpendCoins( equipment.price))
            {
                GameDataManager.Ins.SpendCoins( equipment.price);
                GameDataManager.Ins.AddEquipment( equipment);
                GoldSharedUI.Ins.UpdateCoinsUIText();
            }
            MergeShow.GetComponent<MergeEquipmentShowUI>().ResetMergeEquipUI();
        }
        else Debug.Log("Full Inventory");
	}
}
