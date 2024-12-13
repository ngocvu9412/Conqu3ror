using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EquipmentShowUI : MonoBehaviour
{
    [Header ("Layout Settings")]
    [SerializeField] float itemSpace = 10;

    float itemWidth;

    [Header ("UI Elements")]
    [SerializeField] GameObject EquipmentMenuUI;
    [SerializeField] Transform EquipmentContainer;
    [SerializeField] GameObject EquipmentPrefab;
    [Space(20)]
    [SerializeField] EquipmentShopDatabase EquipmentDB;
    void Start ()
    {
        GenerateEquipmentShowUI();
    }
    void GenerateEquipmentShowUI ()
    {
        //Xóa item template
        itemWidth = EquipmentContainer.GetChild (0).GetComponent<RectTransform>().sizeDelta.x;
        Destroy(EquipmentContainer.GetChild (0).gameObject);
        EquipmentContainer.DetachChildren();

        //Tạo items
        for (int i=0; i < EquipmentDB.EquipmentsCount; i++)
        {
            Equipment Equipment = EquipmentDB.GetEquipment(i);
            EquipmentUI uiEquipment = Instantiate (EquipmentPrefab, EquipmentContainer).GetComponent <EquipmentUI> ();

            //Di chuyển item đến vị trí
            Debug.Log(uiEquipment.GetComponent<RectTransform>().anchoredPosition);
            uiEquipment.SetEquipmentPos(Vector2.right*(itemWidth+itemSpace)*i);

            //Thêm tên cho item trong Hierachy
            uiEquipment.gameObject.name = "Equipment - " + Equipment.name; 
            
            //Thêm thông tin
            if(Equipment.image == null) Debug.Log("Image Set Fail");
            uiEquipment.SetEquipmentImage (Equipment.image);

            if (Equipment.isPurchased){
                uiEquipment.SetEquipmentPurchasedStatus(true);
            }
            else {
                uiEquipment.SetEquipmentPurchasedStatus(false);
            }

            //Chỉnh kích cỡ item container
            EquipmentContainer.GetComponent<RectTransform> ().sizeDelta = Vector2.right*(itemWidth+itemSpace)*EquipmentDB.EquipmentsCount;
        }
    }
}
