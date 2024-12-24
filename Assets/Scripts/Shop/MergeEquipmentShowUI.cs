using System.Collections.Generic;
using UnityEngine;

public class MergeEquipmentShowUI : MonoBehaviour
{
    [Header ("Layout Settings")]
    [SerializeField] float itemSpace = 10;

    float itemWidth;
    float itemHeight;

    [Header ("UI Elements")]
    [SerializeField] Transform MergeEquipmentContainer;
    [SerializeField] GameObject MergeEquipmentPrefab;
    [SerializeField] GameObject InventoryEquip;
    
    void Start ()
    {
        GenerateMergeEquipmentShowUI();
    }
    void GenerateMergeEquipmentShowUI ()
    {
        //Xóa item template
        itemWidth = MergeEquipmentPrefab.GetComponent<RectTransform>().sizeDelta.x;
        itemHeight = MergeEquipmentPrefab.GetComponent<RectTransform>().sizeDelta.y;

        //Tạo items
        for (int i=0; i < 25; i++)
        {
            MergeEquipmentUI uiMergeEquipment = Instantiate (MergeEquipmentPrefab, MergeEquipmentContainer).GetComponent <MergeEquipmentUI> ();
            uiMergeEquipment.SetEquipmentPos(Vector2.down*(itemHeight+itemSpace)*(i/5));
            uiMergeEquipment.SetEquipmentPos(Vector2.right*(itemWidth+itemSpace)*(i%5));
            //Lưu vị trí cũ
            uiMergeEquipment.previous_position = uiMergeEquipment.GetComponent<RectTransform>().anchoredPosition;      
            //Di chuyển item đến vị trí
            
        }
        SetSlotMergeEquipmentInfo();
    }
    public void SetSlotMergeEquipmentInfo()
    {
        int j =0;
        List<Equipment> PlayerEquipments = GameDataManager.GetPlayerEquipments();
        for( int i = 0 ; i< PlayerEquipments.Count;i++)
        {
            Equipment Equipment = PlayerEquipments[i];
            MergeEquipmentUI uiMergeEquipment = MergeEquipmentContainer.GetChild(j).GetComponent<MergeEquipmentUI>();
            int equipmentSlot=GameDataManager.IsEquipmentUsed(Equipment);
            if(equipmentSlot == -1) 
            { 
                uiMergeEquipment.CanMerge = true;
            }
            else continue;
            //Thêm tên cho item trong Hierachy
            uiMergeEquipment.gameObject.name = "Equipment - " + Equipment.name + " - Lv" + Equipment.Level; 
            
            //Thêm thông tin
            if(Equipment.GetSprite() == null) Debug.Log("Image Set Fail");
            uiMergeEquipment.SetEquipmentImage (Equipment.GetSprite());
            uiMergeEquipment.SetLevelText (Equipment.Level);
            uiMergeEquipment.SetIndex (i);  
            uiMergeEquipment.SetCanDrag(true); 
            uiMergeEquipment.SetInfoValid(true);
            j++;
        }
    }
    public void ResetMergeEquipUI()
    {
        if(MergeEquipmentContainer.childCount < 25)
        return;
        for( int i = 0; i < 25; i++)
        {
            MergeEquipmentContainer.GetChild(i).GetComponent<MergeEquipmentUI>().Initialize();
        }
        SetSlotMergeEquipmentInfo();
    }
}
