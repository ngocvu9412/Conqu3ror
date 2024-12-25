using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentShowUI : MonoBehaviour
{
    [Header ("Layout Settings")]
    [SerializeField] float itemSpace = 10;

    float itemWidth;

    [Header ("UI Elements")]
    [SerializeField] Transform EquipmentContainer;
    [SerializeField] GameObject EquipmentPrefab;
    [SerializeField] GameObject Slot1;
    [SerializeField] GameObject Slot2;

    [SerializeField] GameObject Slot3;

    [SerializeField] GameObject Slot4;
    [SerializeField] List<(Equipment,int)> HighestLevelEquip;

    
    void Start ()
    {
        GenerateEquipmentShowUI();
    }
    void GenerateEquipmentShowUI ()
    {
        //Xóa item template
        itemWidth = EquipmentPrefab.GetComponent<RectTransform>().sizeDelta.x;

        //Tạo items
        for (int i=0; i < 6; i++)
        {
            EquipmentUI uiEquipment = Instantiate (EquipmentPrefab, EquipmentContainer).GetComponent <EquipmentUI> ();
            uiEquipment.SetEquipmentPos(Vector2.right*(itemWidth+itemSpace)*i);
            //Lưu vị trí cũ
            uiEquipment.previous_position = uiEquipment.GetComponent<RectTransform>().anchoredPosition;      
            //Di chuyển item đến vị trí
            
        }
        SetSlotEquipmentInfo();
    }
    public void SetSlotEquipmentInfo()
    {
        if(EquipmentContainer.childCount > 6)
        {
            HighestLevelEquip = GameDataManager.Ins.GetHighestLevelEquipments();
            for( int i = 0; i< HighestLevelEquip.Count;i++)
            {
                Equipment Equipment = HighestLevelEquip[i].Item1;
                ChangeUsedWeaponToItsHigherLevel(Equipment);
                EquipmentUI uiEquipment = EquipmentContainer.GetChild(i+6).GetComponent<EquipmentUI>();
                int equipmentSlot=GameDataManager.Ins.IsEquipmentUsed(Equipment);
                if(equipmentSlot != -1)
                {
                    switch (equipmentSlot)
                    {
                        case 0:
                        {
                            uiEquipment.GetComponent<RectTransform>().anchoredPosition = Slot1.GetComponent<RectTransform>().anchoredPosition;
                        }
                        break;
                        case 1:
                        {
                            uiEquipment.GetComponent<RectTransform>().anchoredPosition = Slot2.GetComponent<RectTransform>().anchoredPosition;
                        }
                        break;
                        case 2:
                        {
                            uiEquipment.GetComponent<RectTransform>().anchoredPosition = Slot3.GetComponent<RectTransform>().anchoredPosition;
                        }
                        break;
                        case 3:
                        {
                            uiEquipment.GetComponent<RectTransform>().anchoredPosition = Slot4.GetComponent<RectTransform>().anchoredPosition;
                        }
                        break;
                    }
                }
                //Thêm tên cho item trong Hierachy
                uiEquipment.gameObject.name = "Equipment - " + Equipment.name + " - Lv" + Equipment.Level; 
                
                //Thêm thông tin
                if(Equipment.GetSprite() == null) Debug.Log("Image Set Fail");
                uiEquipment.SetEquipmentImage (Equipment.GetSprite());
                uiEquipment.SetLevelText (Equipment.Level);
                uiEquipment.SetIndex (HighestLevelEquip[i].Item2);  
                uiEquipment.SetCanDrag(true); 
                uiEquipment.SetInfoValid(true);
            }
        }
    }
    public void ChangeUsedWeaponToItsHigherLevel(Equipment equipment)
    {
        Equipment[]PlayerUsedEquip = GameDataManager.Ins.GetPlayerUsedEquips();
        for(int i =0; i<PlayerUsedEquip.Length;i++)
        {
            if(PlayerUsedEquip[i].name == equipment.name && PlayerUsedEquip[i].Level < equipment.Level)
            {
                GameDataManager.Ins.RemoveEquipmentUsed(GameDataManager.Ins.GetPlayerEquipments().IndexOf(PlayerUsedEquip[i]));
                GameDataManager.Ins.AddEquipmentToSlot(i,GameDataManager.Ins.GetPlayerEquipments().IndexOf(equipment));
            }
        }
    }
}
